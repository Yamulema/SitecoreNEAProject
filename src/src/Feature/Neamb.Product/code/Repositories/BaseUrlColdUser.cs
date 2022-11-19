using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	[Service(typeof(IBaseUrlColdUser))]
	public class BaseUrlColdUser: IBaseUrlColdUser
	{
		public string GetBaseUrlPartner() {
			var options = LinkManager.GetDefaultUrlBuilderOptions();
			options.AlwaysIncludeServerUrl = true;
			options.SiteResolving = true;
			return LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID), options);
		}
	}
}