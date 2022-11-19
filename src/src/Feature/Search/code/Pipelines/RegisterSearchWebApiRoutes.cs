using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Neambc.Seiumb.Feature.Search.Pipelines
{
    public class RegisterSearchWebApiRoutes
    {
#pragma warning disable RECS0154 // Parameter is never used
        public void Process(PipelineArgs args)
#pragma warning restore RECS0154 // Parameter is never used
        {

            #region Marketplace stores
            RouteTable.Routes.MapRoute("SeiumbStoreSearch", "api/{controller}/{action}", new { controller = "SeiumbStoreSearch", action = "Search" });
            #endregion
        }
    }
}