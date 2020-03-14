using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using WebAppCore.Data.EF;

namespace WebAppCore
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//BuildWebHost(args).Run();
			var host = CreateWebHostBuilder(args).Build();
			using(var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				try
				{
					var dbInitializer = services.GetService<DbInitializer>();
					dbInitializer.Seed().Wait();
				} catch(Exception ex)
				{
					var logger = services.GetService<ILogger<Program>>();
					logger.LogError(ex,"co loi xay ra.!");
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
