using System.Web.Mvc;
using System.Web.Routing;

namespace WebSPA_AngularJS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultAny",
                url: "{alias}",
                defaults: new { controller = "Home", action = "Index", alias = string.Empty }
            );
        }
    }
}
