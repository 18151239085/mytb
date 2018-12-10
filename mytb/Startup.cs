using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SmartMap.NetPlatform.Core;

namespace mytb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.Configure<List<PlatConnectionString>>(Configuration.GetSection("PlatConnectionStrings"));
			services.Configure<FtpConnectionString>(Configuration.GetSection("FtpConnectionString"));
			services.AddSession(options =>
			{
				// 设置 Session 过期时间
				options.IdleTimeout = TimeSpan.FromDays(90);
				options.Cookie.HttpOnly = true;
			});
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			
			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
			app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions()
			{
				FileProvider = new PhysicalFileProvider(
			   Path.Combine(Directory.GetCurrentDirectory(), @"content")),
				RequestPath = new PathString("/content")
			});
			app.UseCookiePolicy();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "area",
					template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				routes.MapRoute(
					name: "default",
					template: "{area=qian}/{controller=Home}/{action=Index}/{id?}");
			});
		}
    }
}
