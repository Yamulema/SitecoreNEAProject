using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Neambc.Neamb.Project.Web.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("GetUserprofileInformation", "api/{controller}/{action}",
                new { controller = "UserStateController", action = "GetUserprofileInformation" });
        }
    }
}