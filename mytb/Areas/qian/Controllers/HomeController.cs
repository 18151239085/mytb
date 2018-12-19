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
		private readonly tzbRepository tzbRepository;
		private readonly attachRepository attachRepository;

		public HomeController(IOptions<List<PlatConnectionString>> configs, IOptions<FtpConnectionString> ftpConnection)
		{
			this._configs = configs;
			this.tzbRepository = new tzbRepository(getConnectionByName("Portal"));
			this.attachRepository = new attachRepository(getConnectionByName("Portal"));
		}
		public IActionResult Index()
        {
			IList<tzb> tz = tzbRepository.GetNewsList(1, 10);
			IList<attach> pic = attachRepository.GetPicList();
			zh top = new zh();
			top.tzb = tz;
			top.attach = pic;
			return View(top);
		}
    }
}
