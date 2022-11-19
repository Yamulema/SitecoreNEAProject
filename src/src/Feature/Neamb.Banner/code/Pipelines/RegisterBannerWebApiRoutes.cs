using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Neambc.Neamb.Feature.Banner.Pipelines
{
	public class RegisterBannerWebApiRoutes
	{
#pragma warning disable RECS0154 // Parameter is never used
		public void Process(PipelineArgs args)
#pragma warning restore RECS0154 // Parameter is never used
		{
			RouteTable.Routes.MapRoute("TopBannerAction", "api/{controller}/{action}", new { controller = "TopSiteBannerController", action = "CloseBanner" });
        }
    }
}