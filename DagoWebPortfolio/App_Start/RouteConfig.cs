using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DagoWebPorfolio
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default_Admin",
               url: "Admin",
               defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Admin",
               url: "Admin/{controller}/{action}/{id}",
               defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "_Welcome", id = UrlParameter.Optional }
            );
        }
    }
}
