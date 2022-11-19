using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Neambc.Neamb.Feature.Contest.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("AddUserVote", "api/{controller}/{action}",
                new { controller = "VoteService", action = "AddUserVote" });
            RouteTable.Routes.MapRoute("GetUserVote", "api/{controller}/{action}",
                new { controller = "VoteService", action = "GetUserVote" });
            RouteTable.Routes.MapRoute("GetVotes", "api/{controller}/{action}",
                new { controller = "VoteService", action = "GetVotes" });
        }
    }
}