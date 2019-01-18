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
using reposity.MyGrid;
using SmartMap.NetPlatform.Core;
using SmartMap.NetPlatform.Core.Controllers;
using SmartMap.NetPlatform.Core.Helper;

namespace mytb.Controllers
{
	[Area("hou")]
	public class yhglController : PlatBaseController
	{
		private readonly ryRepository ryRepository;
		public yhglController(IOptions<List<PlatConnectionString>> configs, IOptions<FtpConnectionString> ftpConnection)
		{
			this._configs = configs;
			this.ryRepository = new ryRepository(getConnectionByName("Portal"));
		}
		public IActionResult index()
        {
			return View();
        }
		public IActionResult GetUserList()
		{
			MyGrid post = MyGrid.getMyGrid(this);
			return this.Json(ryRepository.GridDataList(post.Take, post.Page, post.Sort, post.filter));
		}
	}
}
