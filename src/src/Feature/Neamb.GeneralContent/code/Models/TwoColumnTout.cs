using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class TwoColumnTout : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public string BackgroundColor { get; set; }
        public bool AlignmentRight { get; set; }
        public bool HasDatasource { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            HasDatasource = !string.IsNullOrEmpty(rendering.DataSource) && rendering.DataSource != Guid.Empty.ToString();
        }
    }
}