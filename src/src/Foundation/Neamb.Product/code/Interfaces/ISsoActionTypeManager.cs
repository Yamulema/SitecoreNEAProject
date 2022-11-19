using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Interfaces
{
	public interface ISsoActionTypeManager
	{
		OperationResult GetUrlSso(SsoModel ssoModel);
	}
}