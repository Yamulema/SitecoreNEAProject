using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Neambc.Neamb.Feature.Product.Pipelines
{
	public class RegisterProductWebApiRoutes
	{
#pragma warning disable RECS0154 // Parameter is never used
		public void Process(PipelineArgs args)
#pragma warning restore RECS0154 // Parameter is never used
		{
			RouteTable.Routes.MapRoute("ProductDownloadRouteCtaPipeline", "api/{controller}/{action}", new { controller = "ProductRoute", action = "DownloadEfulfillmentPdfCta" });
			RouteTable.Routes.MapRoute("ProductDownloadMultirowRoutePipeline", "api/{controller}/{action}", new { controller = "ProductRoute", action = "DownloadEfulfillmentPdfMultirow" });
			RouteTable.Routes.MapRoute("ProductNotifyRoutePipeline", "api/{controller}/{action}", new { controller = "ProductRoute", action = "NotifyProductAvailable" });
			RouteTable.Routes.MapRoute("ProductExecuteSingleSignOnMultirow", "api/{controller}/{action}", new { controller = "ProductRoute", action = "ExecuteSingleSignOnMultirow" });
			RouteTable.Routes.MapRoute("ExecuteStoredActionCta", "api/{controller}/{action}", new { controller = "ProductRoute", action = "ExecuteStoredActionCta" });
			RouteTable.Routes.MapRoute("ProductExecuteDatapassMultirow", "api/{controller}/{action}", new { controller = "ProductRoute", action = "ExecuteDatapassMultirow" });
			RouteTable.Routes.MapRoute("ProductExecuteLinkMultirow", "api/{controller}/{action}", new { controller = "ProductRoute", action = "ExecuteLinkMultirow" });
		    RouteTable.Routes.MapRoute("ReminderServiceSetReminder", "api/{controller}/{action}", new { controller = "ReminderService", action = "SetReminder" });
			RouteTable.Routes.MapRoute("ProcessExternalEfulfillment", "api/{controller}/{action}", new { controller = "ProductRoute", action = "ProcessExternalEfulfillment" });
			RouteTable.Routes.MapRoute("NotifyProductAvailableWarm", "api/{controller}/{action}", new { controller = "ProductRoute", action = "NotifyProductAvailableWarm" });
			RouteTable.Routes.MapRoute("ExecuteSweepstakes", "api/{controller}/{action}", new { controller = "Sweepstakes", action = "ExecuteSweepstakes" });
            RouteTable.Routes.MapRoute("ExecuteReminderSeminar", "api/{controller}/{action}", new { controller = "RetirementSeminar", action = "ExecuteReminderSeminar" });
            RouteTable.Routes.MapRoute("ProductExecuteLinkOmni", "api/{controller}/{action}", new { controller = "ProductRoute", action = "ExecuteLinkOmni" });
            RouteTable.Routes.MapRoute("ExecuteGoalTracking", "api/{controller}/{action}", new { controller = "ProductRoute", action = "ExecuteGoalTracking" });
			RouteTable.Routes.MapRoute("ProductMultiOfferAction", "api/{controller}/{action}", new { controller = "ProductMultiOffer", action = "ProductMultiOfferAction" });
		}
    }
}