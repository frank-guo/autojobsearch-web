using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCMovie
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //The following commented code is an example of how to default some particular urls.
            //routes.MapRoute(
            //    name: "Condition",
            //    url: "Condition/{action}/{id}",
            //    defaults: new { controller = "Condition", action = "Index", id = "1" }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "Hello",
                 url: "{controller}/{action}/{name}/{id}"
             );
        }

    }
}
