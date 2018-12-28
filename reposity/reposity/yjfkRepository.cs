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
	public class yjfkRepository : RepositoriesBase<yjfk>
	{
		public yjfkRepository(PlatConnectionString connectString) : base(connectString) { }
		public virtual List<yjfk> Gethflb(string keyValue)
		{
			#region 查询脚本
			String sql = String.Format("select * from yjfk a where a.tzb_id = '" + keyValue + "'");
			#endregion
			using (var db = Connection)
			{
				List<yjfk> file = db.DbConnecttion.Query<yjfk>(sql, null).ToList();
				if (file.Count>0)
				{
					return file;
				}
			}
			return new List<yjfk>();
		}
		public bool AddData(yjfk Entity)
		{
			using (var db = Connection)
			{
				string id = Entity.id;
				if (string.IsNullOrEmpty(id))
				{
					Entity.id = Guid.NewGuid().ToString();
					db.Insert<yjfk>(Entity);
				}
				else
				{
					string executeSql = @"select * from yjfk a where a.id='" + id + "'";//占位符
					yjfk d = db.DbConnecttion.Query<yjfk>(executeSql, null).FirstOrDefault();
					if (d != null)
					{
						DeepCopyHelper.CloneObject(Entity, ref d);
						db.Update<yjfk>(d);
					}
					else
					{
						db.Insert<yjfk>(Entity);
					}
				}
				return true;
			}
		}
	}

}
