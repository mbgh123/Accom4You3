using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace A4U3.Web.Infrastructure
{
    public class MyAuthorizationAttribute : AuthorizeAttribute
    {
        //TODO-low how does authorize filter work in MVC6?

        /// <summary>
        /// NB. This authorize filter method will run before any other defines action/result methods
        /// </summary>
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    Log(filterContext.RouteData);
        //}
        //private void Log(System.Web.Routing.RouteData routeData)
        //{
        //    Debug.WriteLine("MY-AUTHORIZE {0}::{1} ", routeData.Values["controller"]
        //                                    , routeData.Values["action"] );
        //}
    }
}