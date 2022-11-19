using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Neambc.Neamb.Feature.Rakuten.Pipelines {
    public class RegisterRakutenWebApiRoutes {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("RedirectStorePartner",
                "api/{controller}/{action}",
                new {
                    controller = "RakutenRoute",
                    action = "GetStoreLinkPartner"
                });
            RouteTable.Routes.MapRoute("SignUp",
                "api/{controller}/{action}",
                new
                {
                    controller = "RakutenRoute",
                    action = "SignUp"
                });
        }

    }
}