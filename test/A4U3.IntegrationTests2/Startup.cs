using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using A4U3.TestTools;
using Microsoft.Extensions.DependencyInjection;
using A4U3.EFContext;
using Microsoft.EntityFrameworkCore;
using A4U3.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using cloudscribe.Web.Pagination;
using A4U3.Web.Services;
using A4U3.Domain.Interfaces;

namespace A4U3.IntegratiopnTests2
{
    /// <summary>
    /// Copied from the web startup. But if used by our unit tests, the routing fails.
    /// </summary>
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var path = Utility.ReturnRoot();

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // MVC6 associate the ConfigOptions section from the  json config  with the ConfigOptions class
            services.Configure<ConfigOptions>(Configuration.GetSection("ConfigOptions"));

            var connection = Configuration["ConfigOptions:ConnectionString"];
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            //services.AddApplicationInsightsTelemetry(Configuration);

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
            services.AddScoped<IStaticData, Repository.StaticDataProvider>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            //app.UseApplicationInsightsExceptionTelemetry();

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
