using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class ProductOfferLinksDTO
	{
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public Item PageItem { get; set; }
		public StatusEnum UserStatus { get; set; }

		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			UserStatus = StatusEnum.Unknown;
		}
	}
}