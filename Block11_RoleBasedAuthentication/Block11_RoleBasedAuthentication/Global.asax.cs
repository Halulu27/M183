using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Block11_RoleBasedAuthentication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string[] Roles;
        public static Dictionary<string, object> UserRoles;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Roles = new[] {"Administrator", "User"};
            UserRoles = new Dictionary<string, object> {{"admin", "Administrator"}, {"user", "User"}};
        }
    }
}
