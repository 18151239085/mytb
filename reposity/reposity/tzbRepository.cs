﻿using Dapper;
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
	public class tzbRepository : RepositoriesBase<tzb>
	{
		public tzbRepository(PlatConnectionString connectString) : base(connectString) { }
		public bool AddNews(tzb Entity, string Name)
		{
			using (var db = Connection)
			{
				string id = Entity.id;
				string executeSql = String.Format("select * from tzb t where t.id = '" + id + "'");
				var conditon = new { MH_NEWS_ID = id };
				tzb d = db.DbConnecttion.Query<tzb>(executeSql, conditon).FirstOrDefault();

				if (d == null)
				{
					Entity.cjr = Name;
					Entity.create_time = DateTime.Now;
					db.Insert<tzb>(Entity);
				}
				else
				{
					
				}
				return true;
			}
		}
		public virtual IList<tzb> GetNewsList(int PageNo, int PageSize)
		{
			#region 查询脚本
			String sql = String.Format("select * from tzb t order by t.create_time desc limit ");
			int startIndex = (PageNo - 1) * PageSize;//起点
			sql += startIndex + "," + PageSize;
			#endregion
			using (var db = Connection)
			{
				IList<tzb> datas = db.DbConnecttion.Query<tzb>(sql, null).ToList();
				return datas;
			}
		}
		public virtual tzb GetById(string keyValue)
		{
			#region 查询脚本
			String sql = String.Format("select * from tzb a where a.id = '" + keyValue + "'");
			#endregion
			using (var db = Connection)
			{
				tzb file = db.DbConnecttion.Query<tzb>(sql, null).First();
				if (file != null)
				{
					return file;
				}
			}
			return null;
		}
	}
}