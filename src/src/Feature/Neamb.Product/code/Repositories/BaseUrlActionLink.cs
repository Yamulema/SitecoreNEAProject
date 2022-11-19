using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	[Service(typeof(IBaseUrlActionLink))]
	public class BaseUrlActionLink : IBaseUrlActionLink
	{
		public string GetBaseUrlPartner(Item renderingItem, ID ctaLinkItemId)
		{
			var urlLink = renderingItem.LinkFieldUrl(ctaLinkItemId).Trim();
			urlLink = urlLink.Split(new[] { '?' })[0];
			return urlLink;
		}
	}
}