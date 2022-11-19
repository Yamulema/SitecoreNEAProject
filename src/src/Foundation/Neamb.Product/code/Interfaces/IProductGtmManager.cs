using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Interfaces
{
	public interface IProductGtmManager {
		string GetGtmFunction(ComponentTypeEnum componentType, Item renderingItem, string clickHref, ProductCtaBase cta = null, StatusEnum statusUser = StatusEnum.Cold);
    }
}