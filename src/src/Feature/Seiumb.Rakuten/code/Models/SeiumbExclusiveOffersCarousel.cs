using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Neambc.Seiumb.Feature.Rakuten.Models
{
    public class SeiumbExclusiveOffersCarousel : IRenderingModel
    {
        public string Headline { get; set; }
        public List<SeiumbExclusiveOfferCard> Cards { get; set; }
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