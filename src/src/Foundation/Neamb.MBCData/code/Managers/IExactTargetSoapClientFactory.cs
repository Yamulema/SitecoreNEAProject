using Neambc.Neamb.Foundation.MBCData.ExactTargetService;

namespace Neambc.Neamb.Foundation.MBCData.Managers {

	public interface IExactTargetSoapClientFactory {
		IExactTargetSoapClient CreateClient();
	}

}