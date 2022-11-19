using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Neambc.Seiumb.Feature.Product.Pipelines
{
    public class RegisterWebApiRoutes
    {
#pragma warning disable RECS0154 // Parameter is never used
		public void Process(PipelineArgs args)
#pragma warning restore RECS0154 // Parameter is never used
		{
            RouteTable.Routes.MapRoute("ProductPipeline", "api/{controller}/{action}", new { controller = "ProductApi", action = "DownloadEfulfillmentPdf" });
            RouteTable.Routes.MapRoute("FakeEfulfillment", "api/{controller}/{action}", new { controller = "ProductApi", action = "ExecuteStoredProcedureOrder" });
			RouteTable.Routes.MapRoute("TrueCarPipeline", "api/{controller}/{action}", new { controller = "ProductApi", action = "GetTrueCarPartner" });
	        RouteTable.Routes.MapRoute("VerifyAuthenticationSeiumb", "api/{controller}/{action}", new { controller = "ProductApi", action = "VerifyAuthenticationSeiumb" });
            RouteTable.Routes.MapRoute("ExecuteLink", "api/{controller}/{action}", new { controller = "ProductApi", action = "ExecuteLink" });

}
    }
}