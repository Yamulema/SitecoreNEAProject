using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	public interface ICtaActionRepository {
		ActionButtonResult GetActionResult(StatusEnum statusUser, ActionRequest actionRequest);
		ActionButton GetActionData(StatusEnum statusUser, ActionRequest actionRequest);
        Anonymous SetAnonymousData(ID anonymousItemId, Item renderingItem, string productName);
    }
}