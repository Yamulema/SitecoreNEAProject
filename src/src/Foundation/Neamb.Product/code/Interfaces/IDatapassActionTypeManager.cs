using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Interfaces
{
	public interface IDatapassActionTypeManager
	{
		OperationResult GetUrlDatapass(DatapassModel datapassModel);
	}
}