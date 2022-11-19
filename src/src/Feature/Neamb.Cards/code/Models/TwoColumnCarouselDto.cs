using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class TwoColumnCarouselDto : IRenderingModel
    {
        public IEnumerable<CarouselPageCard> PageCards { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public bool HasDatasource { get; private set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            HasDatasource = !string.IsNullOrEmpty(rendering.DataSource) && rendering.DataSource != Guid.Empty.ToString();
            PageCards = new List<CarouselPageCard>();
        }
    }
}