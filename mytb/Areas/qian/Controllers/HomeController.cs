using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Repository;
using SmartMap.NetPlatform.Core;
using SmartMap.NetPlatform.Core.Controllers;
using SmartMap.NetPlatform.Core.KendoUI;
using SmartMap.NetPlatform.Core.Helper;
using reposity.MyGrid;
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
		//获取帖子列表
		public IActionResult gettzlist()
		{
            MyGrid post = MyGrid.getMyGrid(this);
            return this.Json(tzbRepository.GridtzDataList(post.Take, post.Page, post.Sort));
		}

    }
}
