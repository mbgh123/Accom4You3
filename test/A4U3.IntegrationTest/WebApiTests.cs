
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using A4U3.TestTools;
using A4U3.Repository;
using A4U3.Domain.Interfaces;
using A4U3.Web.Controllers;
using A4U3.EFContext;
using A4U3.Domain.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.IO;
using A4U3.Web.Models.ViewModel;

namespace A4U3.IntegrationTests
{
    /// <summary>
    /// The item under test is the PropertiesController.
    /// 
    /// Using In Memory backing for the EF context.
    /// From http://www.jerriepelser.com/blog/unit-testing-aspnet5-entityframework7-inmemory-database
    /// 
    /// We have do do some configuration first, largely copied from the website's startup.
    /// It may be possible to invoke that startup file rather than copy/pasting (yep, see A4U3.IntegrationTests2)
    /// 
    /// We are newing up the controller (not making web calls, see other test project for that approach)
    /// </summary>
    public class WebApiTests
    {
        private PropertiesController _controller;

        public WebApiTests()
        {
            var path = Utility.ReturnRoot();            

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json");       // local config file needed for static data

            var Configuration = builder.Build();

            var services = new ServiceCollection();

            services.Configure<ConfigOptions>(Configuration.GetSection("ConfigOptions"));


            // The Use of "In Memory" is determined by the dbcontext constructor & appsettings
            services.AddEntityFramework()
                .AddDbContext<ApplicationDbContext>();

            // Dependency Injection
            services.AddScoped<IRepository, A4U3.Repository.RepositoryEF>();
            services.AddScoped<IStaticData, StaticDataProvider>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            //Set up the controller
            var serviceProvider = services.BuildServiceProvider();
            var rep = serviceProvider.GetRequiredService<IRepository>();
            Utility.PopulateDB(rep);

            _controller = new PropertiesController(rep);
        }

        #region Setup data
        // Regarding data in the database we have some options:
        // For every unit test, the db is cleared and data recreated. This ensures each test is independant.
        // Alternatively, build up the data as we go. Unit tests are then may be tied to the results
        // of preceding tests.
        //  The way xunit works, the constructor is run for each test. So we have a fresh db for
        // each test.
        //
        // See Utility for data population code.
        #endregion

        [Fact]
        public void Get_Properties_Returns_Data()
        {
            // Arrange
            //  done in constructor

            // Act
            var result = _controller.GetProperties().ToList();

            // Assert - using fluent notation
            result.Should().BeOfType<List<PropertyVM>>()
                 .Which.Count.Should().Be(10);

            // Assert - normal
            Assert.IsType(typeof(List<PropertyVM>), result);
            Assert.Equal(10, result.Count());
        }
    }
}
