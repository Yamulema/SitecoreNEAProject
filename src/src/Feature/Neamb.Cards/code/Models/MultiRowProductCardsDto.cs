using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class MultiRowProductCardsDto : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public IEnumerable<MultiRowProductCard> PageCards { get; set; }
        public bool HasDatasource { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            HasDatasource = !string.IsNullOrEmpty(rendering.DataSource) && rendering.DataSource != Guid.Empty.ToString();
            PageCards = new List<MultiRowProductCard>();
        }
    }
}