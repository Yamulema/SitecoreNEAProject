
using System;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Model
{
	[Serializable]
	public class OmniLinkModel
    {
		public string ContextItem { get; set; }
		public string EligibilityItemId { get; set; }
		public string ProductCodeLink { get; set; }
		public AccountUserBase AccountUser { get; set; }
	}
}