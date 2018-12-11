using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using Repository;
using SmartMap.NetPlatform.Core;
using SmartMap.NetPlatform.Core.Controllers;
using SmartMap.NetPlatform.Core.Helper;

namespace mytb.Controllers
{
	[Area("hou")]
	public class HTDLController : PlatBaseController
	{
		private readonly ryRepository ryRepository;
		public HTDLController(IOptions<List<PlatConnectionString>> configs, IOptions<FtpConnectionString> ftpConnection)
		{
			this._configs = configs;
			this.ryRepository = new ryRepository(getConnectionByName("Portal"));
		}
		public IActionResult login()
        {
			return View();
        }
		public string htlogin()
		{
			ry ry = new ry();
			var updateResult = TryUpdateModelAsync<ry>(ry);
			if (ModelState.IsValid)
			{
				//检查用户信息
				var user = ryRepository.Check(ry.yhm, ry.mm);
				if (user != null)
				{
					HttpContext.Session.Clear();
					//记录session
					HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
					HttpContext.Session.Set("CurrentUserModel", ByteConvertHelper.Object2Bytes(ry));

					return "success";
				}
				else
				{
					return "用户名或密码错误需要修改";
				}
			}
			foreach (var key in ModelState.Keys)
			{
				if (ModelState[key].Errors.Count > 0)
				{
					ViewBag.ErrorInfo = JsonConvert.SerializeObject(new { Key = key, ErrorMessage = ModelState[key].Errors[0].ErrorMessage });
					break;
				}
			}
			return "fail";
		}
	}
}
