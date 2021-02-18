using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace SecondHandAuth.Commons
{
    public class SessionFilter : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;

            //if Session == null => Login page
            if (ctx.Session[Commons.Constants.USER_SESION] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Login" }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}