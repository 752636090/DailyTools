using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NReco.VideoInfo;

namespace VideoDurationWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    //await context.Response.WriteAsync("Hello World!");

                    string folderPath = @"F:\视频\unity游戏开发\图形学games101";
                    DirectoryInfo directory = new DirectoryInfo(folderPath);
                    FileInfo[] files = directory.GetFiles("*", searchOption: SearchOption.AllDirectories);
                    int fileCount = files.Length;
                    foreach (FileInfo file in files)
                    {
                        FFProbe ffProbe = new FFProbe();
                        MediaInfo videoInfo = ffProbe.GetMediaInfo(file.FullName);
                        await context.Response.WriteAsync(videoInfo.Duration.ToString());
                    }

                    await context.Response.WriteAsync("完成");
                });
            });
        }
    }
}
