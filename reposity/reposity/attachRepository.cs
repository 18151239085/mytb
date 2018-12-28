using Dapper;
using DapperEx;
using Microsoft.AspNetCore.Http;
using Models;
using Newtonsoft.Json;
using SmartMap.NetPlatform.Core;
using SmartMap.NetPlatform.Core.Helper;
using SmartMap.NetPlatform.Core.KendoUI;
using SmartMap.NetPlatform.Models;
using SmartMap.NetPlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class attachRepository : RepositoriesBase<attach>
	{
		public attachRepository(PlatConnectionString connectString) : base(connectString) { }
		public IList<attach> GetPicById(string keyValue)
		{

			if (keyValue != "" && keyValue != null && keyValue != "0")
			{
				#region 查询脚本
				String sql = String.Format("select * from attach t where t.yw_id = '" + keyValue + "'");
				#endregion
				using (var db = Connection)
				{
					IList<attach> file = db.DbConnecttion.Query<attach>(sql, null).ToList();
					if (file != null)
					{
						return file;
					}
				}
			}
			return null;
		}
		public virtual bool DeleteFile(DbBase dbs, string newsid, string fileName)
		{
			var db = dbs.DbConnecttion;
			string sql = string.Format("delete from attach where yw_id='{0}' and tp_name='{1}'", newsid, fileName);
			var f = db.Execute(sql);
			return f > 0;
		}
		public bool AddFile(attach Entity, string Name, string fileName)
		{
			using (var db = Connection)
			{
				string Sql = string.Format("select * from attach a where a.yw_id='{0}'and tp_name='{1}'", Entity.yw_id, fileName);
				attach d = db.DbConnecttion.Query<attach>(Sql).FirstOrDefault();
				if (d == null)
				{
					//string sql = string.Format("delete from MH_ATTACH where MH_BUSINESS_ID='{0}'", d.MH_BUSINESS_ID);
					//db.DbConnecttion.Execute(sql);
					string id = Entity.yw_id;
					Entity.id = Guid.NewGuid().ToString();
					Entity.creater = Name;
					db.Insert<attach>(Entity);//找不到改数据就插入

				}
				else
				{
					Entity.MODIFIER = Name;//否则就修改
					Entity.MODIFY_TIME = DateTime.Now;
					DeepCopyHelper.CloneObject(Entity, ref d);
					db.Update<attach>(d);
				}
				return true;
			}
		}
		public virtual IList<attach> GetPicList()
		{
			#region 查询脚本
			String sql = String.Format("select * from attach t");
			#endregion
			using (var db = Connection)
			{
				IList<attach> datas = db.DbConnecttion.Query<attach>(sql, null).ToList();
				return datas;
			}
		}
		public virtual List<attach> GetATTACH(string keyValue)
		{
			#region 查询脚本
			String sql = String.Format("select * from attach a where a.yw_id= '" + keyValue + "'");
			#endregion
			using (var db = Connection)
			{
				List<attach> file = db.DbConnecttion.Query<attach>(sql, null).ToList();
				if (file.Count>0)
				{
					return file;
				}
			}
			return new List<attach>();
		}
	}
}
