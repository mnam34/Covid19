using DataAccess;
using ElmahCore.Mvc;
using Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using System;
using Web.Helpers;
namespace Web
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
            services.AddControllersWithViews();
            services.AddMvc()
               .AddJsonOptions(options =>
               {
                   //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                   //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               })
               .AddMvcOptions(options =>
               {
                   options.FormatterMappings.SetMediaTypeMappingForFormat(
                     "docx", MediaTypeHeaderValue.Parse("application/ms-word"));
                   options.RespectBrowserAcceptHeader = true;
               })
               ;
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = new Newtonsoft.Json.ReferenceLoopHandling();
                });
            //services.AddControllersWithViews();
            //services.AddRazorPages();
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new MyViewLocationExpander());
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);

            });
            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //var connection = @"Data Source=./;Initial Catalog=PaperlessOffice;Persist Security Info=True;User ID=sa;Password=OHT7^%$#@!;MultipleActiveResultSets=true";
            //services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"), x => { x.MigrationsAssembly("Web"); x.CommandTimeout(1000); }));
            services.AddDbContext<DataContext>(options =>
            {
                options.ConfigureWarnings(x => x.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning));
                options
                    .UseLazyLoadingProxies()
                    .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning))
                    .UseSqlServer(Configuration.GetConnectionString("ConnectionString"), x => { x.MigrationsAssembly("Web"); x.CommandTimeout(100000); x.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); });
            }
            );
            //services.AddDbContext<DataContext>(options => options.UseSqlServer(connection, x => x.MigrationsAssembly("Web")));
            services.AddScoped<IRepository, Repository>();
            services.Configure<IISOptions>(options =>
            {

            });
            services.AddElmah(options => options.Path = "elmah");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            AppHttpContext.Services = app.ApplicationServices;
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                   name: "areas",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                 );
            });
            app.Use(async (ctx, next) =>
            {
                ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });
            app.UseElmah();
        }
    }
}
