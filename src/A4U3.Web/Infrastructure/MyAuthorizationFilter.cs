using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace A4U3.Web.Infrastructure
{
    /// <summary>
    /// Work in progress: How do I disable [Authorize] during integration testing?
    /// </summary>
    public class MyAuthorizationFilter : AuthorizeFilter
    {
        public MyAuthorizationFilter(AuthorizationPolicy policy) : base(policy)
        {
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // If we are integration testing then ignore any [Authorize] attributes
            if (true)
            {
                return Task.FromResult(0);
            }

            return base.OnAuthorizationAsync(context);
        }
    }
}
