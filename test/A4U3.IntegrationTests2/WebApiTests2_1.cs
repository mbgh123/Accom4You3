using A4U3.Domain.Models;
using A4U3.IntegrationTests2.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace A4U3.IntegrationTests2

{
    /// <summary>
    /// The item under test is the Web Api functionality.
    /// There is another test project A4U2.IntegrationTests that covers this (news up controller class)
    /// But the approach here is to create a web client and pass in a url.
    /// 
    /// The client is implemented using Microsoft.AspNet.Hosting and Microsoft.AspNet.TestHost
    /// that provides TestServer class.
    /// 
    /// For each test in a xunit class file, the constructor is run prior is each test.
    /// This may be a performace problem if setup is takes time.
    /// To address this issue xunit offers testficure classes, see WebApiTest3
    /// 
    /// Unlike A4U2.IntegrationTests which copied the web startup code.
    /// Here, configuration is performed by referencing the web startup file.
    /// 
    /// NB the database to be used is defined via the Startup and conn string in appsettings
    /// We are using the web startup but have our own appsettings file.
    /// </summary>
    public class WebApiTests2_1     {

        TestServer _server;

        public WebApiTests2_1()
        {
            // RC1
            // NB. Using the Startup from the web site for configuration.
            // But it wil look for a appsettings.json in this test project....
            // ...that was the RC1 behaviour, RTM differs.
            //
            // RTM
            // If we use the web startup, env.ContentRootPath will be 
            // "C:\\Projects\\A4U3B\\test\\A4U3.IntegrationTest\\bin\\Debug\\Net452"
            // so the apsettings.json wont be found (contains conn string) and the db context will fail.
            //
            // If I copy the web startup to this project, we can fiddle with the path to get to the parent
            // directory containing the appsetting, that problem solved.
            // However, the routing to api/properties now fails, we get a not found.
            //
            // HACK So I've reverted to using the web startup, but hacked the path to check for /bin/
            // if found, drop down a few levels to the project directory.

                       
            // use the web startup
            _server = new TestServer(new WebHostBuilder().UseStartup<A4U3.Web.Startup>());
            
            // use our own startup
            //_server = new TestServer(new WebHostBuilder().UseStartup<A4U3.IntegratiopnTests2.Startup>());
        }

        [Fact]
        public async void Get_Properties_Returns_Data()
        {
            using (var client = _server.CreateClient().AcceptJson())
            {
                var response = await client.GetAsync("/api/Properties/");


                // check that requested was correctly routed to the controller
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var result = await response.Content.ReadAsJsonAsync<List<Property>>();

                // check the return data
                Assert.NotNull(result);
                Assert.NotEmpty(result);
            }
        }

        [Fact]
        public void Get_Properties_Returns_Data_NOT_Asynch()
        {
            using (var client = _server.CreateClient().AcceptJson())
            {
                var response = client.GetAsync("/api/Properties/").Result;


                // check that requested was correctly routed to the controller
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsJsonAsync<List<Property>>().Result;

                // check the return data
                Assert.NotNull(result);
                Assert.NotEmpty(result);
            }
        }
    }
}
