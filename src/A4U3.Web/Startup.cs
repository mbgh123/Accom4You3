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
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using A4U3.Web.Services;
using A4U3.Domain.Interfaces;
using A4U3.Repository;
using A4U3.EFContext;
using A4U3.Domain.Models;
using cloudscribe.Web.Pagination;
using A4U3.TestTools;

namespace A4U3.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //HACK to allow use of this file from a unit test project.
            //var path = env.ContentRootPath;
            var path = Utility.ReturnRoot();


            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            // In Azure portal, the Appsettings section holds an entry for
            // ConfigOptions:ConnectionString
            // That's how the live connection string is picked up.
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

            // Add framework services.
            services.AddMvc()
                 .AddJsonOptions(o =>
                 {
                     o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseSession();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
