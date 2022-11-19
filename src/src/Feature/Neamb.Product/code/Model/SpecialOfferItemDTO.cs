using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class SpecialOfferItemDTO : ProductDetailDTO
	{
		public override void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
		}
	}
}