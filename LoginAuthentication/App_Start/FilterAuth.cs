using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace LoginAuthenticationAuthorization.App_Start
{
    [AttributeUsage(AttributeTargets.Class |
    AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class FilterAuth : ActionFilterAttribute
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var path = filterContext.HttpContext.Request.CurrentExecutionFilePath;
            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString();
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString();
                        
            if (!controller.Equals("Home")  && !controller.Equals("Auth"))
            {
                if (HttpContext.Current.Session["user"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary{
                                { "controller", "Auth" },
                                { "action", "Login" }
                        });
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}