using System.Web.Routing;
using Sitecore.Pipelines;
using Sitecore;
using Neambc.Seiumb.Feature.Language.App_Start;

namespace Neambc.Seiumb.Feature.Language.Infrastructure.Pipelines
{
    public class InitializeRoutes
    {
#pragma warning disable RECS0154 
		/// Parameter is never used
		/// <summary>
		/// Initialize the new route to be called from the Ajax fuction
		/// </summary>
		/// <param name="args"></param>
		public void Process(PipelineArgs args)
#pragma warning restore RECS0154 // Parameter is never used
		{
            if (!Context.IsUnitTesting)
            {
                RouteConfig.RegisterRoutes(RouteTable.Routes);
            }
        }
    }
}