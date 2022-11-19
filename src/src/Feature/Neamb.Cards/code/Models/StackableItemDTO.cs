using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class StackableItemDTO : IRenderingModel
    {
        public bool HasTextPlacementLeft { get; set; }
        public bool HasImage { get; set; }
        public bool HasVideo { get; set; }
        public bool HasCallout { get; set; }
        public string VideoUrl { get; set; }
        public bool IsJWPlatformVideo { get; set; }
        public Item Item { get; set; }
        public Rendering Rendering { get; set; }
        public void Initialize(Rendering rendering)
        {
            Item = rendering.Item;
			Rendering = rendering;
            
        }
    }
}