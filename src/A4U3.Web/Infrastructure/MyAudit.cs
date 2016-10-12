using System;
using A4U3.Domain.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features;

namespace A4U3.Web.Infrastructure
{
    public class MyAudit : IActionFilter
    {
        private IRepository _rep;

        public MyAudit(IRepository repository)
        {
            _rep = repository;
        }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string url = filterContext.HttpContext.Request.GetEncodedUrl();

                string userAgent = filterContext.HttpContext.Request.Headers["User-Agent"].ToString();

                // Original MVC5 code for  ip address
                //string userHostAddress = filterContext.HttpContext.Request.UserHostAddress;

                // This is supposed to work, but GetFeature not recognized. Assembly/reference problem?
                //var ipAddress = filterContext.HttpContext.GetFeature<IHttpConnectionFeature>()?.RemoteIpAddress;

                // Looks like GetFeatures has changed to Features.Get
                //var ipAddress = filterContext.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

                // TODO MVC6 ip address? need to test.
                string userHostAddress = filterContext.HttpContext.Connection.RemoteIpAddress?.ToString();

                string controller = filterContext.RouteData.Values["controller"]?.ToString();
                string action = filterContext.RouteData.Values["action"]?.ToString();

                if (userAgent.ToLower().Contains("appinsights"))
                {
                    // Don't bother recording the AppInsights site checks
                }
                else
                {
                    _rep.Log(url, userAgent, userHostAddress, controller, action);
                }

            }
            catch (Exception ex)
            {
                // swallow
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // not interested in doing anything, but we have to implement the method
        }
    }
}