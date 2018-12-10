using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperEx;
using SmartMap.NetPlatform.Core.KendoUI;
using System.Data;

namespace Repositorys
{
	public interface IRepository
	{
		DataSourceResult GridData<T>(int take, int skip, IList<Sort> sorts, Filter filter) where T : class;

		bool Insert<T>(DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null) where T : class;

		bool InsertBatch<T>(DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class;

		bool Delete<T>(DbBase dbs, SqlQuery sql = null) where T : class;

		bool Delete<T>(DbBase dbs, T t) where T : class;

		bool Update<T>(DbBase dbs, T t, IDbTransaction transaction = null, SqlQuery sql = null) where T : class;

		bool Update<T>(DbBase dbs, T t, IList<string> updateProperties, SqlQuery sql = null) where T : class;

		T SingleOrDefault<T>(DbBase dbs, SqlQuery sql) where T : class;

		IList<T> Page<T>(DbBase dbs, int pageIndex, int pageSize, out long dataCount, SqlQuery sqlQuery = null) where T : class;

		IList<T> Query<T>(DbBase dbs, SqlQuery sql = null) where T : class;

		long Count<T>(DbBase dbs, SqlQuery sql = null) where T : class;
	}
	/// <summary>
	/// 定义泛型仓储接口,DapperEx中操作的二次封装
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public interface IRepository<TEntity>
	{
		DataSourceResult GridData(int take, int skip, IList<Sort> sorts, Filter filter);

		bool Insert(DbBase dbs, TEntity t, IDbTransaction transaction = null, int? commandTimeout = null);

		bool InsertBatch(DbBase dbs, IList<TEntity> lt, IDbTransaction transaction = null, int? commandTimeout = null);

		bool Delete(DbBase dbs, SqlQuery sql = null);

		bool Delete(DbBase dbs, TEntity t);

		bool Update(DbBase dbs, TEntity t, IDbTransaction transaction = null, SqlQuery sql = null);

		bool Update(DbBase dbs, TEntity t, IList<string> updateProperties, SqlQuery sql = null);

		TEntity SingleOrDefault(DbBase dbs, SqlQuery sql);

		IList<TEntity> Page(DbBase dbs, int pageIndex, int pageSize, out long dataCount, SqlQuery sqlQuery = null);

		IList<TEntity> Query(DbBase dbs, SqlQuery sql = null);

		long Count(DbBase dbs, SqlQuery sql = null);
	}


}
