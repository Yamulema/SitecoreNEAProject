using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Interfaces
{
	public interface IProductManager
	{
		void ExecuteMdsLoggingProcessCta(string productcode);
		void ExecuteMdsLoggingProcessView(string programCode);
		void ExecuteMdsLoggingProcessMaterial(string materialid);
		AccountUserBase GetAccountUser(AccountMembership accountMembership);
		ProductCustomDimension GetProductDimensions(Item renderingItem);
        void SetGoalsProducts(string productId, string goalId);
    }
}