using A4U3.IntegrationTests2.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace A4U3.IntegrationTests2.TestFixtures
{
    public class TestServerFixture : IDisposable
    {
        public TestServer TestServer { get; }
        public HttpClient Client { get; }

        public TestServerFixture()
        {

            // RC1
            //TestServer = new TestServer(TestServer.CreateBuilder().UseStartup<A4U2.Web.Startup>());
            //Client = TestServer.CreateClient().AcceptJson();


            // RTM
            TestServer = new TestServer(new WebHostBuilder().UseStartup<A4U3.Web.Startup>());

            Client = TestServer.CreateClient().AcceptJson();

        }

        public void Dispose()
        {
            TestServer.Dispose();
            Client.Dispose();
        }
    }
}
