using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Neambc.Seiumb.Feature.Rakuten.Pipelines {
    public class RegisterRakutenSeiumbWebApiRoutes {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("SignUpSeiumb",
                "api/{controller}/{action}",
                new
                {
                    controller = "SeiumbRakutenRoute",
                    action = "SignUpSeiumb"
                });
            RouteTable.Routes.MapRoute("GetStoreLinkPartnerSeiumb",
                "api/{controller}/{action}",
                new
                {
                    controller = "SeiumbRakutenRoute",
                    action = "GetStoreLinkPartnerSeiumb"
                });
        }

    }
}