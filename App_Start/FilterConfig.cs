using System.Web;
using System.Web.Mvc;

namespace SBM_SYSTEM_MONITOR
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
