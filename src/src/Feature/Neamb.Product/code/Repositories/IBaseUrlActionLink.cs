using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	public interface IBaseUrlActionLink {
		string GetBaseUrlPartner(Item renderingItem, ID ctaLinkItemId);
	}
}