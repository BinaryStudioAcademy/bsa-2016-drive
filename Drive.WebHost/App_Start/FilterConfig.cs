using System.Web.Mvc;
using Drive.WebHost.Filters;

namespace Drive.WebHost
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new JWTAuthenticationFilter());
        }
    }
}
