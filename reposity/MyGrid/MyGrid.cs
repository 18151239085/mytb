using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using SmartMap.NetPlatform.Core.KendoUI;
using Microsoft.AspNetCore.Mvc;

namespace reposity.MyGrid
{
    public class MyGrid : IDisposable
    {
        private Logger log = LogManager.GetLogger("KendoGridPost");

        private bool disposedValue;

        public int Page
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int Skip
        {
            get;
            set;
        }

        public int Take
        {
            get;
            set;
        }

        public IList<Sort> Sort
        {
            get;
            set;
        }

        public Filter filter
        {
            get;
            set;
        }
        static public MyGrid getMyGrid(Controller Controller) {
            try
            {
                var memery = new System.IO.MemoryStream();
                // Controller.Request.Body是post请求体,将请求体复制memery
                Controller.Request.Body.CopyTo(memery);
                memery.Position = 0;
                //二进制文件,文本
                string Content = new System.IO.StreamReader(memery, System.Text.UTF8Encoding.UTF8).ReadToEnd();
                memery.Position = 0;
                Controller.Request.Body = memery;
                // 将json字符串变成实体类
                MyGrid tmp=JsonConvert.DeserializeObject<MyGrid>(Content);
                return tmp;
            }
            catch
            {
                return new MyGrid();
            }
        }
        public MyGrid()
        {
            this.Page = 1;
            this.PageSize = 15;
            this.Skip = 0;
            this.Take = 5;
        }

        ~MyGrid()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.Sort = null;
                    this.filter = null;
                    if (this.log != null)
                    {
                        this.log = null;
                    }
                }
                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
