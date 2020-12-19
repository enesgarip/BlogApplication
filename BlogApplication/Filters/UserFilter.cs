using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BlogApplication.Filters
{
    public class UserFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? userId = context.HttpContext.Session.GetInt32("id");
            if (!userId.HasValue)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"action","Index" },
                    {"controller","Admin" }
                });
            }
            base.OnActionExecuting(context);
        }
    }
}
