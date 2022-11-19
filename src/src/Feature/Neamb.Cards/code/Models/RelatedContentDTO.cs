using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class RelatedContentDto : IRenderingModel
    {
        public IEnumerable<RelatedContentCard> PageCards { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public string AltField { get; set; }
        public string BackgroundColor { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            PageCards = new List<RelatedContentCard>();
        }
    }
}