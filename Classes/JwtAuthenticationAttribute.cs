using SBM_SYSTEM_MONITOR.Classes;
using System;
using System.Web.Mvc;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class JwtAuthenticationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        JwtTokenGenerator JWT = new JwtTokenGenerator();
        
        if (!JWT.IsAuth())
        {
            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Authentication", action = "Login" }));
        }

        base.OnActionExecuting(filterContext);
    }
}
