using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Neambc.Neamb.Feature.GeneralContent.Pipelines
{
	public class RegisterWebApiRoutes
	{
		public void Process(PipelineArgs args)
		{
			RouteTable.Routes.MapRoute("GetLifeInsuranceCalculator", "api/{controller}/{action}", new { controller = "Calculator", action = "GetLifeInsuranceCalculator" });
			RouteTable.Routes.MapRoute("GetLifeInsurancePlanCalculator", "api/{controller}/{action}", new { controller = "Calculator", action = "GetLifeInsurancePlanCalculator" });
		    RouteTable.Routes.MapRoute("GetQuote", "api/{controller}/{action}", new { controller = "Calculator", action = "GetQuote" });
		    RouteTable.Routes.MapRoute("ValidateState", "api/{controller}/{action}", new { controller = "Calculator", action = "ValidateState" });
		    RouteTable.Routes.MapRoute("Migrate", "api/{controller}/{action}", new { controller = "Misc", action = "Migrate" });
		    RouteTable.Routes.MapRoute("RemoveGa", "api/{controller}/{action}", new { controller = "Misc", action = "RemoveGa" });
		    RouteTable.Routes.MapRoute("RemoveVisited", "api/{controller}/{action}", new { controller = "Misc", action = "RemoveVisted" });
            RouteTable.Routes.MapRoute("TrackingGTM", "api/{controller}/{action}", new { controller = "GTMController", action = "Tracking" });
            RouteTable.Routes.MapRoute("ValidateUserToken", "api/{controller}/{action}", new { controller = "ReCaptcha", action = "ValidateUserToken" });
		}
	}
}