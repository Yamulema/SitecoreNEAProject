using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Neambc.Neamb.Feature.Feature.Pipelines
{
	public class RegisterWebApiRoutes
	{
#pragma warning disable RECS0154 // Parameter is never used
		public void Process(PipelineArgs args)
#pragma warning restore RECS0154 // Parameter is never used
		{
			RouteTable.Routes.MapRoute("uploadavatar", "api/{controller}/{action}",
				new {controller = "Avatar", action = "UploadAvatar" });
			RouteTable.Routes.MapRoute("uploadavatargeneralerror", "api/{controller}/{action}",
				new { controller = "Avatar", action = "UploadAvatarGeneralError" });
			RouteTable.Routes.MapRoute("uploadavatarerrorsize", "api/{controller}/{action}",
				new { controller = "Avatar", action = "UploadAvatarErrorSize" });

		}
	}
}