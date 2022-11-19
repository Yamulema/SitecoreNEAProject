using System;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Model
{
    [Serializable]
    public class MultiRowOfferItemDto : ProductDetailDTO
	{
        public bool HasImage { get; set; }
        public string ButtonClass { get; set; }
        public String ViewDetailsText { get; set; }
        public String ViewDetailsUrl { get; set; }
        public String ViewDetailsTarget { get; set; }
        public String ViewDetailsClickAction { get; set; }
        public new void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
            HasImage = false;
        }
	}
}