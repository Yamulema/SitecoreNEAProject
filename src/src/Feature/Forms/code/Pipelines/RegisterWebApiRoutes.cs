using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Neambc.Seiumb.Feature.Forms.Pipelines
{
    public class RegisterWebApiRoutes
    {
#pragma warning disable RECS0154 // Parameter is never used
		public void Process(PipelineArgs args)
#pragma warning restore RECS0154 // Parameter is never used
		{
            RouteTable.Routes.MapRoute("forms", "api/{controller}/{action}", new { controller = "Forms", action = "GetDataCalculator" });
            RouteTable.Routes.MapRoute("loginmodal", "api/{controller}/{action}", new { controller = "LoginForm", action = "LoginFormAjax" });
        }
    }
}