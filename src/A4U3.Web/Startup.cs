using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using A4U3.Web.Services;
using A4U3.Domain.Interfaces;
using A4U3.Repository;
using A4U3.EFContext;
using A4U3.Domain.Models;
using cloudscribe.Web.Pagination;
using System.IO;

namespace A4U3.Web
{
    public class Startup
    {
        public Startup()
        {
            //HACK to allow use of this file from a unit test project.
            var path = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // MVC6 associate the ConfigOptions section from the  json config  with the ConfigOptions class
            services.Configure<ConfigOptions>(Configuration.GetSection("ConfigOptions"));

            var connection = Configuration["ConfigOptions:ConnectionString"];

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add framework services. Use Newtonsoft.Json for compatibility with TestServer
            services.AddControllersWithViews()
                 .AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                 });

            services.TryAddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();

            // TODO Needed for Session  
            //services.AddCaching();
            services.AddSession();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // My Stuff
            services.AddScoped<IRepository, Repository.RepositoryEF>();
            services.AddScoped<IStaticData, StaticDataProvider>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // Note: environment-specific behavior is handled by host in modern ASP.NET Core apps.

            app.UseStaticFiles();

            // Application Insights middleware is wired via services when the package is present.

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
