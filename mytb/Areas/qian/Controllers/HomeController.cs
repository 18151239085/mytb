using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Repository;
using SmartMap.NetPlatform.Core;
using SmartMap.NetPlatform.Core.Controllers;

namespace mytb.Controllers
{
	[Area("qian")]
	public class HomeController : PlatBaseController
	{
		public HomeController(IOptions<List<PlatConnectionString>> configs, IOptions<FtpConnectionString> ftpConnection)
		{
			this._configs = configs;
		}
		public IActionResult Index()
        {
			return View();
        }
    }
}
