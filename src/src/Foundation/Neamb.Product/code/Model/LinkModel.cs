
using System;
using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Model
{
	[Serializable]
	public class LinkModel
	{
		public string ContextItem { get; set; }
		public string EligibilityItemId { get; set; }
		public string ProductCodeLink { get; set; }
		public string CtaLinkItemId { get; set; }
		public AccountUserBase AccountUser { get; set; }
        public Dictionary<string, object> PostData { get; set; }
        public PassthroughModel PassthrougData { get; set; }
    }
}