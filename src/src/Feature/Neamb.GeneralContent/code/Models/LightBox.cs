using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class LightBox
    {
        public Item Item { get; set; }
        public string RedirectUrl { get; set; }
    }
}