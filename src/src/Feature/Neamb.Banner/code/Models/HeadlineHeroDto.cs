using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Banner.Repositories.Enums;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Banner.Models
{
    public class HeadlineHeroDto : IRenderingModel
    {
        public bool HasDatasource { get; set; }
        /// <summary>
        /// Item1 corresponds to the ID of the template field; Item2 corresponds to the Sitecore Item.
        /// </summary>
        public Tuple<ID, Item> PageTitle { get; set; }
        /// <summary>
        /// Item1 corresponds to the ID of the template field; Item2 corresponds to the Sitecore Item.
        /// </summary>
        public Tuple<ID, Item> Subheadline { get; set; }
        /// <summary>
        /// Item1 corresponds to the ID of the template field; Item2 corresponds to the Sitecore Item.
        /// </summary>
        public Tuple<ID, Item> HeroImage { get; set; }
        public Tuple<ID, Item> QuoteWidget { get; set; }
        public string HeroImageSrc { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public HeadlineType Type { get; set; }
        public bool HasImage { get; set; }
        public bool CenteredText { get; set; }
        
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            HasDatasource = !string.IsNullOrEmpty(rendering.DataSource) && rendering.DataSource != Guid.Empty.ToString();
        }
    }
}