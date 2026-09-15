using A4U3.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace A4U3.EFContext
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConfigOptions>(Configuration.GetSection("ConfigOptions"));
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ConfigOptions:ConnectionString"]));

        }

        public void Configure()
        {
            // EF context library does not configure an application pipeline here.
        }
    }
}
