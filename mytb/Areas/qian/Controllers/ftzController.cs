using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using Repository;
using SmartMap.NetPlatform.Core;
using SmartMap.NetPlatform.Core.Controllers;
using SmartMap.NetPlatform.Core.Helper;
using SmartMap.NetPlatform.Core.KendoUI;

namespace mytb.Controllers
{
	[Area("qian")]
	public class ftzController : PlatBaseController
	{
		private readonly attachRepository attachRepository;
		private readonly tzbRepository tzbRepository;
		private readonly yjfkRepository yjfkRepository;
		private IOptions<FtpConnectionString> _FtpConnection;
		public ftzController(IOptions<List<PlatConnectionString>> configs, IOptions<FtpConnectionString> ftpConnection)
		{
			this._configs = configs;
			this.attachRepository = new attachRepository(getConnectionByName("Portal"));
			this.tzbRepository = new tzbRepository(getConnectionByName("Portal"));
			this.yjfkRepository = new yjfkRepository(getConnectionByName("Portal"));
			this._FtpConnection = ftpConnection;
		}
		public IActionResult ft(string keyValue)
		{
			tzb Models = new tzb();
			IList<attach> attach = attachRepository.GetPicById(keyValue);
			Models.id = Guid.NewGuid().ToString();
			return View(Tuple.Create(Models, attach));

		}
		public IActionResult DownloadFile(string keyValue, string name)
		{
			FTPHelper ftps = new FTPHelper(_FtpConnection.Value.FtpServerIP, _FtpConnection.Value.FtpRemotePath + "/tbz/" + keyValue, _FtpConnection.Value.FtpUserID, _FtpConnection.Value.FtpPassword);
			FileResult file = ftps.DownLoad(name, this);
			return file;
		}
		public ActionResult DelImg(string name, string keyValue)
		{
			FTPHelper ftp = new FTPHelper(_FtpConnection.Value.FtpServerIP, _FtpConnection.Value.FtpRemotePath + "/tzb/" + keyValue, _FtpConnection.Value.FtpUserID, _FtpConnection.Value.FtpPassword);
			if (name != "")
			{
				try
				{
					ftp.Delete(name);
					using (var db = attachRepository.Connection)
					{
						attachRepository.DeleteFile(db, keyValue, name);
					}
					return Success("删除成功！");
				}
				catch (Exception ex)
				{
					return Content(ex.Message);
				}
			}
			return Content("");
		}
		public IActionResult Preview(string keyValue)
		{
			return View();

		}
		public ActionResult SaveNews(IFormCollection collection)
		{
			tzb news = new tzb();
			Tuple<tzb, attach> Models = Tuple.Create(new tzb(), new attach());
			var model = HttpContext.Session.Get("CurrentUser");//获取session
			if (model==null) {
				return Success("/qian/USER/Login");
			}
			try
			{
				if (ModelState.IsValid)
				{
					var updateResult = TryUpdateModelAsync<Tuple<tzb, attach>>(Models);  //将传递过来的对象转换
					news = Models.Item1;
					using (var db = tzbRepository.Connection)
					{
						string hex = System.Text.Encoding.Default.GetString(model);//获取json数据
						ry sta = JsonConvert.DeserializeObject<ry>(hex);//转换成model数据
						string userID = sta.id;
						string userName = sta.yhm;
						FTPHelper ftps = new FTPHelper(_FtpConnection.Value.FtpServerIP, _FtpConnection.Value.FtpRemotePath + "/" + news.id, _FtpConnection.Value.FtpUserID, _FtpConnection.Value.FtpPassword);
						List<FileStruct> files = ftps.GetFileAndDirectoryList(_FtpConnection.Value.FtpRemotePath + "/tbz/" + news.id + "/");

						if (files != null)
						{
							foreach (var file in files)
							{
								attach attach = new attach();
								attach.tp_name = file.Name;
								attach.yw_id = news.id;
								attach.create_time = DateTime.Now;
								attach.tp_lj = _FtpConnection.Value.FtpRemotePath + "/tbz/" + news.id;
								attachRepository.AddFile(attach, userName, attach.tp_name);
							}
						}

						tzbRepository.AddNews(news, userName);
						if (news.TSTATUS == 0)
						{
							return Success("保存成功。");
						}
						else {
							return Success("未插入成功。");
						}
					}

				}
				else
					return View();
			}
			catch (Exception ex)
			{
				return Error("保存数据库出错");
			}
		}
		public IActionResult getupLoadFile(string keyValue)
		{
			FTPHelper ftp = new FTPHelper(_FtpConnection.Value.FtpServerIP, _FtpConnection.Value.FtpRemotePath + "/tbz/" + keyValue, _FtpConnection.Value.FtpUserID, _FtpConnection.Value.FtpPassword);
			string[] filess = ftp.GetFileList(keyValue);
			List<FileStruct> files = ftp.GetFileAndDirectoryList(_FtpConnection.Value.FtpRemotePath + "/tbz/" + keyValue + "/");

			return this.Json(files);
		}
		//上传文件
		public ActionResult Save(IEnumerable<IFormFile> files, String keyValue)
		{
			FTPHelper ftp = new FTPHelper(_FtpConnection.Value.FtpServerIP, _FtpConnection.Value.FtpRemotePath + "/tbz/" + keyValue, _FtpConnection.Value.FtpUserID, _FtpConnection.Value.FtpPassword);
			if (files != null)
			{
				foreach (var file in files)
				{
					try
					{
						if (file.ContentType.Contains("pdf"))
						{
							ftp.UploadPDF(file);
						}
						else
						{
							int index = file.FileName.LastIndexOf("\\");
							if (index >= 0)
							{
								string filename = file.FileName.Substring(index + 1);
								FormFile file2 = new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, filename);
								ftp.Upload(file2);
								string[] tmp = new string[] { "/", "?", "&", "=" };
								string name = file2.FileName;
								foreach (var item in tmp)
								{
									name = name.Replace(item, "");
								}
								ftp.ReName(file2.FileName, name);
							}
							else
							{
								ftp.Upload(file);
								string[] tmp = new string[] { "/", "?", "&", "=" };
								string name = file.FileName;
								foreach (var item in tmp)
								{
									name = name.Replace(item, "");
								}
								ftp.ReName(file.FileName, name);
							}

						}
					}
					catch (Exception ex)
					{
						return Content(ex.Message);
					}
				}
			}
			return Content("");
		}
		public ActionResult Remove(string[] fileNames, string keyValue)
		{
			FTPHelper ftp = new FTPHelper(_FtpConnection.Value.FtpServerIP, _FtpConnection.Value.FtpRemotePath + "/tbz/" + keyValue, _FtpConnection.Value.FtpUserID, _FtpConnection.Value.FtpPassword);
			if (fileNames != null)
			{
				foreach (var fullName in fileNames)
				{
					try
					{
						ftp.Delete(fullName);
						using (var db = attachRepository.Connection)
						{
							attachRepository.DeleteFile(db, keyValue, fullName);
						}
						return Success("删除成功！");
					}
					catch (Exception ex)
					{
						return Content(ex.Message);
					}
				}
			}
			return Content("");
		}
		public IActionResult tail(string keyValue)
		{//打开当前页即可
		 //帖子和帖子对应的附件
			tzb tzb = this.tzbRepository.GetById(keyValue);
			//一条贴子可能对应多张图片
			List<attach> attach= this.attachRepository.GetATTACH(tzb.id);
			//回复的内容----
			List<yjfk> yjfk = this.yjfkRepository.Gethflb(tzb.id);
			mh_attach_tzb_yjfk mh_attach_tzb_yjfk = new mh_attach_tzb_yjfk();
			mh_attach_tzb_yjfk.tzb = tzb;
			mh_attach_tzb_yjfk.yjfk = yjfk;
			mh_attach_tzb_yjfk.attach = attach;
			return View(mh_attach_tzb_yjfk);
		}
		public string savehf(IFormCollection collection)
		{
			yjfk yjfk = new yjfk();

			try {
				if (ModelState.IsValid) {
					var model = HttpContext.Session.Get("CurrentUser");//获取session
					if (model == null)
					{
						return "nologin";
					}
					else {
						string hex = System.Text.Encoding.Default.GetString(model);//获取json数据
						ry sta = JsonConvert.DeserializeObject<ry>(hex);//转换成model数据
						var updateResult = TryUpdateModelAsync<yjfk>(yjfk);
						yjfk.ry_id = sta.id;
						yjfk.status = 0;
						yjfk.hf_time = DateTime.Now;
						yjfkRepository.AddData(yjfk);
						return "success";
					}
				}
				return "fail";
			}
			catch (Exception ex) {
				return "fail";
			}
		}
	}
}
