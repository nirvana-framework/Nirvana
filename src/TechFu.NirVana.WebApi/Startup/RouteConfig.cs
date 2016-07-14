using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace TechFu.Nirvana.WebApi.Startup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes,Action<RouteCollection> additionalConfig)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
