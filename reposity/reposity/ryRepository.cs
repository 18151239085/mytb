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
	public class ryRepository : RepositoriesBase<ry>
	{
		public ryRepository(PlatConnectionString connectString) : base(connectString) { }
		public bool insertry(ry ry) {
			String sql = "select * from ry where yhm='" + ry.yhm + "'";
			using (var db = Connection)
			{
				long dc = 0;
				ry r = db.DbConnecttion.Query<ry>(sql, null).ToList().FirstOrDefault();
				if (r != null) //用户名唯一，如果找到数据，就表示已经被注册了
					return false;
				//r为null
				ry.mm = Md5.GetMD5(ry.mm);
				db.Insert<ry>(ry);
			}
			return true;
		}
		public ry  checkuser(string yhm,string mm) {
			string m= Md5.GetMD5(mm);
			string executeSql = @"select * from ry where yhm=@lname and mm=@pwd";
			using (var db = Connection)
			{
				var condition = new { lname = yhm, pwd = m.ToLower() };
				ry d = db.DbConnecttion.Query<ry>(executeSql, condition).FirstOrDefault();
				return d;
			}
		}
        public ry Check(string yhm, string mm)
        {
            string m = Md5.GetMD5(mm);
            string executeSql = @"select * from ry t where t.rylx=0 and t.yhm=@lname and t.mm=@pwd";
            using (var db = Connection)
            {
                var condition = new { lname = yhm, pwd = m.ToLower() };
                ry d = db.DbConnecttion.Query<ry>(executeSql, condition).FirstOrDefault();
                return d;
            }
        }
		public ry jcsjh(string sjh)
		{
			string executeSql = @"select * from ry t where  t.sjh=@lname";
			using (var db = Connection)
			{
				var condition = new { lname = sjh};
				ry d = db.DbConnecttion.Query<ry>(executeSql, condition).FirstOrDefault();
				return d;
			}
		}
		public virtual DataSourceResult GridDataList(int take, int page, IList<Sort> sorts, Filter filter)
		{
			#region 查询脚本
			string sql = @"select a.*,
                            CASE a.rylx when 0 then '后台' 
                            WHEN 1 then '前端用户' 
                            ELSE '无效用户' end as lx 
                            from ry a 
                            where a.id is not null ";
			#endregion
			using (var db = Connection)
			{
				long dc = 0;
				IList<dynamic> datas = db.Page(page, take, out dc, sql, "order by create_time desc ");
				return new DataSourceResult() { Data = datas, Total = (int)dc };
			}
		}
	}
}
