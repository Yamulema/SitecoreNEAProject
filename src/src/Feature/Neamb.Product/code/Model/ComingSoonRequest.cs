using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class ComingSoonRequest
	{
		public ID ReminderCtaId { get; set; }
		public Item RenderingItem { get; set; }
		public string ProductName { get; set; }
		public string ProductCode { get; set; }
		public string Mdsid { get; set; }
		public string ComponentId { get; set; }
		public string UrlActionHref { get; set; }
		public string UserName { get; set; }
		public ComponentTypeEnum ComponentType { get; set; }
        public ID EligibilityItemId { get; set; }
        public bool HasCheckEligibility { get; set; }
    }
}