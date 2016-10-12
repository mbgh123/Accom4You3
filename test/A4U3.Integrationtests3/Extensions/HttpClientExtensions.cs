using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace A4U3.IntegrationTests3.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Configurate the client to tell the server it only accepts json.
        /// http://dotnetliberty.com/index.php/2015/12/17/asp-net-5-web-api-integration-testing/
        /// </summary>        
        public static HttpClient AcceptJson(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
