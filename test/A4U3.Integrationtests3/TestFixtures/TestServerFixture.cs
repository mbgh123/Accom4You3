using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using A4U3.IntegrationTests3.Extensions;
using A4U3.Repository;
using Microsoft.Extensions.DependencyInjection;
using A4U3.Domain.Interfaces;

namespace A4U3.IntegrationTests3.TestFixtures
{
    public class TestServerFixture : IDisposable
    {
        public TestServer TestServer { get; }
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            // We know we can reuse the web site's startup file config. But we want an in memory
            // database. So we'll copy the web site's startup into this project and modify it.
            // (I'd prefer to use the web's and just override the db config, but how?)
            //
            // Answer: Move the assignment of sqlDB/inMemory from the startup to the
            // OnConfiguring on the DbContext. Supply OnConfiguring with options set from
            // a local appsettings file.
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
