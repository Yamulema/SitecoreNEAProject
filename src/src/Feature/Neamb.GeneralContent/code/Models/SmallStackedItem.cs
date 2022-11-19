using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class SmallStackedItem
    {
        public Item Item { get; set; }
        public string OnClickEvent { get; set; }
    }
}