using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAppCore.Data.EF;
using Microsoft.Extensions.DependencyInjection;

namespace WebAppCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbInitializer = services.GetService<DbInitializer>();
                    dbInitializer.Seed().Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetService<ILogger<Program>>();
                    logger.LogError(ex,"Đã xảy ra lỗi trong khi tạo cơ sở dữ liệu");
                }
            }
            host.Run();


        }

	

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration(ConfigConfiguration)
				.UseStartup<Startup>();

		static void ConfigConfiguration(WebHostBuilderContext ctx,IConfigurationBuilder config)
		{
			config.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json",optional: false,reloadOnChange: true)
				.AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json",optional: true,reloadOnChange: true);

		}

	}
}
