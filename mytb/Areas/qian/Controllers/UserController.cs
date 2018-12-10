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
using Repositorys;
using SmartMap.NetPlatform.Core;
using SmartMap.NetPlatform.Core.Controllers;
using SmartMap.NetPlatform.Core.Helper;
using SmartMap.NetPlatform.Mapping.IRepositories;

namespace mytb.Controllers
{
	[Area("qian")]
	public class UserController : PlatBaseController
	{
		private readonly ryRepository ryRepository;
		public UserController(IOptions<List<PlatConnectionString>> configs, IOptions<FtpConnectionString> ftpConnection)
		{
			this._configs = configs;
			this.ryRepository = new ryRepository(getConnectionByName("Portal"));
		}
		//将注册页面view
		public IActionResult Login()
		{
			return View();
		}
		//将登录页面view
		public IActionResult Register()
		{
			return View();
		}
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();

			return RedirectToAction("Home", "../qian");
		}
		public bool DORegister()
		{
			ry ry = new ry();
			var updateResult = TryUpdateModelAsync<ry>(ry);
			ry.id = Guid.NewGuid().ToString();
			ry.create_time = DateTime.Now;
			ry.rylx = 1;
			bool t = this.ryRepository.insertry(ry);
			return t;
		}
		public string DOLogin()
		{
			ry ry = new ry();
			var updateResult = TryUpdateModelAsync<ry>(ry);
			if (ModelState.IsValid)
			{
				if (ry.yhm != null)
				{
					if (ry.mm!=null) {
						//检查用户信息
						var user = ryRepository.checkuser(ry.yhm, ry.mm);
						if (user != null)
						{
							HttpContext.Session.Clear();
							//记录session
							HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
							HttpContext.Session.Set("CurrentUserModel", ByteConvertHelper.Object2Bytes(ry));
							return "success";
						}
						else {
							return "用户名或密码不能为空。";
						}
					}
					else {
						return "用户名或密码不能为空。";
					}
				}
				else {
					return "用户名或密码不能为空。";
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
