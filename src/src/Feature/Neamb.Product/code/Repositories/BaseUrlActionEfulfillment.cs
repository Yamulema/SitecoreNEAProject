using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	[Service(typeof(IBaseUrlActionEfulfillment))]
	public class BaseUrlActionEfulfillment : IBaseUrlActionEfulfillment
	{
		public string GetBaseUrlPartner()
		{
			return ConstantsNeamb.UrlApiProducts;
		}
	}
}