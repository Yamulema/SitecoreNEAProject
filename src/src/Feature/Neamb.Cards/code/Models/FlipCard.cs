using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class FlipCard 
    {
        public Item FlipItem { get; set; }
        public string BackgroundColor { get; set; }
        public void Initialize(Rendering rendering)
        {
            FlipItem = rendering.Item;            
        }
    }
}