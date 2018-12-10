
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using DapperEx;
using SmartMap.NetPlatform.Core;
using Dapper;
using SmartMap.NetPlatform.Core.KendoUI;
using SmartMap.NetPlatform.Core.Layui;
using Repositorys;
using System.Reflection;

namespace Repository
{
	/// <summary>
	/// dapper目前不太好泛型实现
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RepositoriesBase<T> : IRepository<T> where T : class
	{
		public PlatConnectionString connectionString;

		public RepositoriesBase(PlatConnectionString connectString)
		{
			connectionString = connectString;
		}

		public DbBase Connection
		{
			get
			{
				return new DbBase(connectionString);
			}
		}

		/// <summary>
		/// add by sgy 2018-03-23增加对layerui的grid表格的支持
		/// </summary>
		/// <param name="post"></param>
		/// <param name="sorts"></param>
		/// <param name="sql"></param>
		/// <returns></returns>
		public virtual LayuiGridResult GridData(LayuiGridPost post)
		{
			using (var db = Connection)
			{
				var d = SqlQuery<T>.Builder(db).KendoUIWhere(post.filter);

				if (post.Sort != null && post.Sort.Count > 0)
				{
					foreach (Sort s in post.Sort)
					{
						d.OrderBy(s.Dir, s.Field);
					}
				}
				long dc = 0;
				var result = db.Page<T>(post.Page, post.Take, out dc, d);
				post = null;
				d = null;
				db.Dispose();
				return new LayuiGridResult() { Code = 0, Msg = "", Count = (int)dc, Data = result };
			}
		}

		public virtual DataSourceResult GridData(int take, int page, IList<Sort> sorts, Filter filter)
		{
			using (var db = Connection)
			{
				var d = SqlQuery<T>.Builder(db).KendoUIWhere(filter);
				if (sorts != null && sorts.Count > 0)
				{
					foreach (Sort s in sorts)
					{
						d.OrderBy(s.Dir, s.Field);
					}
				}
				long dc = 0;
				var result = db.Page<T>(page, take, out dc, d);
				sorts = null;
				filter = null;
				d = null;
				db.Dispose();
				return new DataSourceResult() { Data = result, Total = (int)dc };
			}
			//return null;
		}

		public virtual bool Insert(DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null)
		{
			return dbs.Insert<T>(t, transaction, commandTimeout);
		}

		public virtual bool InsertBatch(DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null)
		{
			return dbs.InsertBatch<T>(lt, transaction, commandTimeout);
		}

		public virtual bool Delete(DbBase dbs, SqlQuery sql = null)
		{
			return dbs.Delete<T>(sql);
		}

		/// <summary>
		/// 需自己实现，不同表key字段不一样
		/// </summary>
		/// <param name="dbs"></param>
		/// <param name="keyValue"></param>
		/// <returns></returns>
		public virtual bool Delete(DbBase dbs, T t)
		{
			throw new NotImplementedException();
		}

		//public virtual bool Update(DbBase dbs, T t, SqlQuery sql = null)
		//{
		//    return dbs.Update<T>(t,sql);
		//}

		public virtual bool Update(DbBase dbs, T t, IDbTransaction transaction = null, SqlQuery sql = null)
		{
			return dbs.Update<T>(t, sql, transaction);
		}

		public virtual bool Update(DbBase dbs, T t, IList<string> updateProperties, SqlQuery sql = null)
		{
			return dbs.Update<T>(t, updateProperties, sql);
		}

		public virtual T SingleOrDefault(DbBase dbs, SqlQuery sql)
		{
			return dbs.SingleOrDefault<T>(sql);
		}

		public virtual IList<T> Page(DbBase dbs, int pageIndex, int pageSize, out long dataCount, SqlQuery sqlQuery = null)
		{
			return dbs.Page<T>(pageIndex, pageSize, out dataCount, sqlQuery);
		}

		public virtual IList<T> Query(DbBase dbs, SqlQuery sql = null)
		{
			return dbs.Query<T>(sql);
		}

		public virtual long Count(DbBase dbs, SqlQuery sql = null)
		{
			return dbs.Count<T>(sql);
		}

		public virtual bool Update(T entity, string keyValue)
		{
			throw new NotImplementedException();
		}

		//public T DynamicToModel(T model, dynamic row)//将动态变量转换为实体变量
		//{

		//    if (row != null)
		//    {
		//        Type modelType = model.GetType();//反射--获得实体类的类型
		//        foreach (var property in (IDictionary<String, Object>)row)
		//        {
		//            string name = property.Key;
		//            object value = property.Value;
		//            //查找实体是否存在列表相同的公共属性
		//            PropertyInfo proInfo = modelType.GetProperty(name);//通过名字拿到属性
		//            if (proInfo != null && value != DBNull.Value && value != null)
		//            {
		//                if (typeof(System.DateTime) == value.GetType())
		//                {
		//                    proInfo.SetValue(model, value, null);//通过属性给实体类赋值
		//                }
		//                else
		//                {
		//                    proInfo.SetValue(model, Convert.ChangeType(value, proInfo.PropertyType), null);
		//                }

		//            }
		//        }
		//    }
		//    return model;
		//}
	}
	public class RepositoriesBase : IRepository
	{
		public PlatConnectionString connectionString;

		public RepositoriesBase(PlatConnectionString connectString)
		{
			connectionString = connectString;
		}

		public DbBase Connection
		{
			get
			{
				return new DbBase(connectionString);
			}
		}

		public virtual DataSourceResult GridData<T>(int take, int page, IList<Sort> sorts, Filter filter) where T : class
		{
			using (var db = Connection)
			{
				var d = SqlQuery<T>.Builder(db).KendoUIWhere(filter);
				if (sorts != null && sorts.Count > 0)
				{
					foreach (Sort s in sorts)
					{
						d.OrderBy(s.Dir, s.Field);
					}
				}
				long dc = 0;
				var result = db.Page<T>(page, take, out dc, d);
				return new DataSourceResult() { Data = result, Total = (int)dc };
			}
			//return null;
		}

		public virtual bool Insert<T>(DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
		{
			return dbs.Insert<T>(t, transaction, commandTimeout);
		}

		public virtual bool InsertBatch<T>(DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
		{
			return dbs.InsertBatch<T>(lt, transaction, commandTimeout);
		}

		public virtual bool Delete<T>(DbBase dbs, SqlQuery sql = null) where T : class
		{
			return dbs.Delete<T>(sql);
		}

		/// <summary>
		/// 需自己实现，不同表key字段不一样
		/// </summary>
		/// <param name="dbs"></param>
		/// <param name="keyValue"></param>
		/// <returns></returns>
		public virtual bool Delete<T>(DbBase dbs, T t) where T : class
		{
			throw new NotImplementedException();
		}

		//public virtual bool Update(DbBase dbs, T t, SqlQuery sql = null)
		//{
		//    return dbs.Update<T>(t,sql);
		//}

		public virtual bool Update<T>(DbBase dbs, T t, IDbTransaction transaction = null, SqlQuery sql = null) where T : class
		{
			return dbs.Update<T>(t, sql, transaction);
		}

		public virtual bool Update<T>(DbBase dbs, T t, IList<string> updateProperties, SqlQuery sql = null) where T : class
		{
			return dbs.Update<T>(t, updateProperties, sql);
		}

		public virtual T SingleOrDefault<T>(DbBase dbs, SqlQuery sql) where T : class
		{
			return dbs.SingleOrDefault<T>(sql);
		}

		public virtual IList<T> Page<T>(DbBase dbs, int pageIndex, int pageSize, out long dataCount, SqlQuery sqlQuery = null) where T : class
		{
			return dbs.Page<T>(pageIndex, pageSize, out dataCount, sqlQuery);
		}

		public virtual IList<T> Query<T>(DbBase dbs, SqlQuery sql = null) where T : class
		{
			return dbs.Query<T>(sql);
		}

		public virtual long Count<T>(DbBase dbs, SqlQuery sql = null) where T : class
		{
			return dbs.Count<T>(sql);
		}

		public virtual bool Update<T>(T entity, string keyValue) where T : class
		{
			throw new NotImplementedException();
		}


		public virtual IList<T> Query<T>(string sql) where T : class
		{
			using (var db = Connection)
			{
				return db.DbConnecttion.Query<T>(sql).ToList();
			}
		}

		public virtual T GetFirst<T>(string sql) where T : class
		{
			using (var db = Connection)
			{
				return db.DbConnecttion.Query<T>(sql).FirstOrDefault();
			}
		}

		public virtual long Count<T>(SqlQuery sql = null) where T : class
		{
			using (var db = Connection)
			{
				return db.Count<T>(sql);
			}
		}

		public virtual long Coun(string sql)
		{
			using (var db = Connection)
			{
				var cr = db.DbConnecttion.Query(sql).SingleOrDefault();
				return (long)cr.DATACOUNT;
			}
		}
	}
}
