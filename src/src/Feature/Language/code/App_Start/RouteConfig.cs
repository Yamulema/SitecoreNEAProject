using System.Web.Mvc;
using System.Web.Routing;

namespace Neambc.Seiumb.Feature.Language.App_Start
{
    public static class RouteConfig
    {
        /// <summary>
        /// Configure a route (api/feature/language/changelanguage) to be called from an Ajax function 
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("language-changelanguage",
                            "api/feature/language/changelanguage",
                            new { controller = "Language", action = "ChangeLanguage", id = UrlParameter.Optional });
        }
    }
}