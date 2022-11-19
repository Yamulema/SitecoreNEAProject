using System;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	[Service(typeof(IBaseUrlComingSoon))]
	public class BaseUrlComingSoon: IBaseUrlComingSoon
	{
		public string GetBaseUrlPartner(string urlLink) {
			return !string.IsNullOrEmpty(urlLink)? urlLink.Split(new[] { '?' })[0]:urlLink;
		}
	}
}