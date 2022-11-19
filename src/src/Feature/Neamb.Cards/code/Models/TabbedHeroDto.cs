using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class TabbedHeroDto : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public List<TabbedHeroItem> TabbedItems { get; set; }
        /// <summary>
        /// Transition time between cards in milliseconds.
        /// </summary>
        public int Timer { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            TabbedItems = new List<TabbedHeroItem>();
        }
    }
}