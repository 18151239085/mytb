using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using Repository;
using SmartMap.NetPlatform.Core;
using SmartMap.NetPlatform.Core.Controllers;
using SmartMap.NetPlatform.Core.Helper;

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
        public bool isLogin() {
            var model = HttpContext.Session.Get("CurrentUser");//获取session
            return model != null;
        }
		public string DORegister()
		{
			ry ry = new ry();
			var updateResult = TryUpdateModelAsync<ry>(ry);
			ry tt= ryRepository.jcsjh(ry.sjh);//查询手机号是否在人员表中出现
			if (tt!=null) {
				return "手机号已存在";
			}//如果tt为null，就代表没有这个手机号，可以接着注册
			ry.id = Guid.NewGuid().ToString();
			ry.create_time = DateTime.Now;
			ry.rylx = 1;
			bool t = this.ryRepository.insertry(ry);
			if (t)
			{
				return "用户未注册，可以正常登陆";
			}
			return "用户名已存在,请重新注册";
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
