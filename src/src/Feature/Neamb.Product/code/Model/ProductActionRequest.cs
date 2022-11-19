using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class ProductActionRequest
	{
		public string ActionConstant { get; set; }
		public string ProductCode { get; set; }
		public bool HasCheckEligibility { get; set; }
		public Item ContextItem { get; set; }
		public bool IsSpecialOffer { get; set; }

		public string ComponentId { get; set; }

		public ID CtaLinkItemId { get; set; }
        public string PostData { get; set; }
        public ID EligibilityItemId { get; set; }
		public string ActionType { get; set; }
		public string ActionString { get; set; }
        public bool IsOmni { get; set; }
        public bool RequiresOnlyLogin { get; set; }
    }
}