using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Model;

namespace Neambc.Neamb.Foundation.Product.Pipelines
{
	public interface IPipelineService
	{
		ResultPipeline RunProcessPipelines(string productCode, AccountUserBase accountUser, string pipelineName,
			string returnUrl = "");
	}
}