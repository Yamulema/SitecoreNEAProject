using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Neambc.Neamb.Feature.Search.Pipelines
{
    public class RegisterSearchWebApiRoutes
    {
#pragma warning disable RECS0154 // Parameter is never used
		public void Process(PipelineArgs args)
#pragma warning restore RECS0154 // Parameter is never used
		{
            RouteTable.Routes.MapRoute("SearchGetContentPages", "api/{controller}/{action}", new { controller = "SearchService", action = "GetContentPages" });
            RouteTable.Routes.MapRoute("GetSuggestions", "api/{controller}/{action}", new { controller = "SearchService", action = "GetSuggestions" });
            RouteTable.Routes.MapRoute("Search", "api/{controller}/{action}", new { controller = "SearchService", action = "Search" });

            #region Marketplace stores
            RouteTable.Routes.MapRoute("StoreSearch", "api/{controller}/{action}", new { controller = "StoreService", action = "Search" });
            #endregion
        }
    }
}