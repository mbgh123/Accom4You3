using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A4U3.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using A4U3.Domain.Models;

namespace A4U3.Repository
{
    /// <summary>
    /// I was caching the result in Session, which was of dubious value.
    /// Anyhow, when it came the controller integration testing, the DI system needed to
    /// activate IHttpContextAccessor. It did this, but the context was null hence null ref exception.
    /// So lets forget caching in Session and simplfy
    /// </summary>
    public class StaticDataProvider : IStaticData
    {
        private IOptions<ConfigOptions> _configOptions;
        //private IHttpContextAccessor _httpContextAccessor;

        public StaticDataProvider(IOptions<ConfigOptions> configOptions
                                  //  , IHttpContextAccessor httpContextAccessor
            )
        {
            _configOptions = configOptions;
            //_httpContextAccessor = httpContextAccessor;
        }
    public IEnumerable<string> GetRobotList()
        {
            //TODO-low - Do we need to cache this at all?
            //In A4U was using application state, could not find in MVC6 so using Session
            //But is there any need? If there is, probably better caching option?

            //string robotString = _httpContextAccessor.HttpContext.Session.GetString("robots");

            //if (robotString == null)
            //{
            //    robotString = _configOptions.Value.Robots;

            //    _httpContextAccessor.HttpContext.Session.SetString("robots", robotString);
            //}


            // No caching, just access the config every time
            string robotString = _configOptions.Value.Robots;

            List<string> robots = robotString.Split(',').ToList();

            return robots;
        }
    }
}
