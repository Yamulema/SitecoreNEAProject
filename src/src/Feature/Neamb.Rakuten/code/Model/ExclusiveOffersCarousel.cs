using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Rakuten.Model
{
    public class ExclusiveOffersCarousel : IRenderingModel
    {
        public string Headline { get; set; }
        public List<ExclusiveOfferCard> Cards { get; set; }
        public Rendering Rendering { get; set; }
        public Item PageItem { get; set; }
        public Item Item { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
        }
    }
}