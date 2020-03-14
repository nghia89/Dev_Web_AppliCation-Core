using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using WebAppCore.Application.Dapper.Implementation;
using WebAppCore.Application.Dapper.Interfaces;
using WebAppCore.Application.Implementation;
using WebAppCore.Application.Interfaces;
using WebAppCore.Authorization;
using WebAppCore.Data.EF;
using WebAppCore.Data.Entities;
using WebAppCore.Extensions;
using WebAppCore.Helpers;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Implemention;
using WebAppCore.Repository.Interfaces;
using WebAppCore.Services;
using WebAppCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace WebAppCore
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
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                o => o.MigrationsAssembly("WebAppCore.Data.EF")));

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddMemoryCache();
            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            services.AddRecaptcha(new RecaptchaOptions
            {
                SiteKey = Configuration["Recaptcha:SiteKey"],
                SecretKey = Configuration["Recaptcha:SecretKey"]
            });
            services.AddSession(options =>
            {
                // time out secction
                options.IdleTimeout = TimeSpan.FromHours(2);
                //
                options.Cookie.HttpOnly = true;
            });

            //gzip
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddResponseCompression(options =>
            {
                IEnumerable<string> MimeTypes = new[]
                        {
							 // General
							 "text/plain",
                             "text/html",
                             "text/css",
                             "font/woff2",
                             "application/javascript",
                             "image/x-icon",
                             "image/png"
                         };
                options.EnableForHttps = true;
                options.MimeTypes = MimeTypes;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });

            //services.AddImageResizer();
            services.AddAutoMapper(typeof(Startup));

            // Add application services.
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

            //services.AddSingleton(Mapper.Configuration);
            //services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(),sp.GetService));

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddTransient<DbInitializer>();

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();
            //services.AddMvc();

            services.AddRazorPages();
            services.AddControllers()
            .AddNewtonsoftJson(options =>
                  options.SerializerSettings.ContractResolver =
                     new DefaultContractResolver());

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            //cho phép các domain khác ở ngoài truy cập vào được
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("http://shophouseware.azurewebsites.net")
                        .AllowCredentials();
                }));
            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));

            //Service
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IBillService, BillService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IPageService, PageService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<ISlideShowService, SlideShowService>();
            services.AddTransient<IAnnouncementUserService, AnnouncementUserService>();
            services.AddTransient<ISystemConfigService, SystemConfigService>();

            ///   Repository
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<ISlideRepository, SlideRepository>();
            services.AddTransient<IAnnouncementRepository, AnnouncementRepository>();
            services.AddTransient<IAnnouncementUserRepository, AnnouncementUserRepository>();

            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            //loggerFactory.AddFile("Logs/structures-{Date}.txt");

            if (env.EnvironmentName == Environments.Production)
            {
                loggerFactory.AddFile("Logs/structures-{Date}.txt");
            }
            if (env.EnvironmentName == Environments.Development)
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
                //app.UseExceptionHandler("/Home/Index");
            }
            app.UseImageResizer();
            app.UseResponseCompression();
            //hạn chế tất cả các file nằm trong thư mục root đều không chạy qua Middleware tiếp theo
            app.UseStaticFiles();
            //app.UseMinResponse();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseCors("CorsPolicy");
            //app.UseSignalR(hubs =>
            //{
            //    hubs.MapHub<SignalRHub>("/signalRHub");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalRHub>("/signalRHub");

                endpoints.MapControllerRoute(
                  name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute("Product", "{alias}-c.{id}.html", new {controller= "Product", action= "Catalog" });  
                endpoints.MapControllerRoute("Product", "{alias}-p.{id}.html", new {controller= "Product", action= "Detail" });  
                endpoints.MapControllerRoute("Blogs", "{name}.{id}.html", new {controller= "Blogs", action= "Detail" });
                endpoints.MapControllerRoute("Blogs", "tintuc.html", new {controller= "Blogs", action= "Index" });

                endpoints.MapControllerRoute(
                 name: "areasRouter",
                   pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}