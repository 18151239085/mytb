using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Components
{
	[Area("hou")]
	[ViewComponent(Name = "MenhuTop")]
	public class houTop : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			var model = HttpContext.Session.Get("CurrentUser");
			if (model == null)
			{//未登录
				ViewData["yhm"] = null;
			}
			else
			{//已登录
				string hex = System.Text.Encoding.Default.GetString(model);//获取json数据
				ry sta = JsonConvert.DeserializeObject<ry>(hex);//转换成model数据

				ViewData["yhm"] = sta.yhm;
			}
			return View();
		}
	}
}

