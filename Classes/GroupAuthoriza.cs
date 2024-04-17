using SBM_SYSTEM_MONITOR;
using System;
using System.Web;
using System.Web.Mvc;

namespace SBM_SYSTEM_MONITOR.Classes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class GroupAuthorizeAttribute : AuthorizeAttribute
    {
        public string GroupCookieName { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                var userGroups = COOKIES.GetCookies(GroupCookieName);
                if (userGroups != null)
                {
                    if (userGroups.ToUpper() == "SYSTEM-ADMIN" || userGroups.ToUpper() == "SUPER-ADMIN")
                    {
                        return true;
                    }
                }
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = 403;
                httpContext.Response.TrySkipIisCustomErrors = true;
                httpContext.Response.Redirect("~/Error/Index", endResponse: true);
                return false;
            }
            else
            {
                return isAuthorized;
            }
        }
    }
}