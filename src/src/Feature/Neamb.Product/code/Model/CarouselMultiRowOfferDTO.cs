using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Sitecore.Data.Fields;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class CarouselMultiRowOfferDTO
    {
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public Item PageItem { get; set; }
        public List<MultiRowOfferItemDto> ItemsCarousel { get; set; }
        public bool HasImage { get; set; }
        public Field Cards { get; set; }
        public string CardClass { get; set; }

        public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
            ItemsCarousel = new List<MultiRowOfferItemDto>();
            HasImage = false;
            Cards = Item.Fields[Templates.CarouselOfferCards.Fields.Cards];
        }
	}
}