using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc.Filters;

namespace A4U3.Web.Infrastructure
{
    /// <summary>
    /// There is a naming convention here. I add "Attribute" the the class name and I can then
    /// decorate controllers and actions methods with "Log"
    /// </summary>
    public class MyLogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Log("Action executing (before)", filterContext.RouteData);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Log("Action executed (after)", filterContext.RouteData);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //Log("Result executing (before)", filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //Log("Result executed (after)", filterContext.RouteData);
        }

        //private void Log(string p, System.Web.Routing.RouteData routeData)
        //{
        //    Debug.WriteLine("{0}::{1} - {2}", routeData.Values["controller"]
        //                                    , routeData.Values["action"]
        //                                    ,p);
        //}
    }
}