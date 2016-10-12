using A4U3.Domain.Models;
using A4U3.IntegrationTests2.Extensions;
using A4U3.IntegrationTests2.TestFixtures;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace A4U3.IntegrationTests2

{
    /// <summary>
    /// The item under test is the Web Api functionality.
    /// There is another test class WebApiTest2.
    /// But difference here is use of xunit TestFixture class to avoid repeated creation 
    /// of the test server.
    /// 
    /// The client is implemented using Microsoft.AspNet.Hosting and Microsoft.AspNet.TestHost
    /// that provides TestServer class.
    /// 
    /// For each test in a xunit class file, the constructor is run prior is each test.
    /// This may be a performace problem if setup is takes time.
    /// To address this issue xunit offers testficure classes
    /// 
    /// NB the database to be used is defined via the Startup and conn string in appsettings
    /// 
    /// </summary>
    public class WebApiTests2_2 : IClassFixture<TestServerFixture>
    {
        private readonly HttpClient _client;

        public WebApiTests2_2(TestServerFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async void Get_Properties_Returns_Data3()
        {

            var response = await _client.GetAsync("/api/Properties/");
            var result = await response.Content.ReadAsJsonAsync<List<Property>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
