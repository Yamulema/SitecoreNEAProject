using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Neambc.Seiumb.Feature.Product.Models
{
    public class ProductList
    {
        public Item ContextItem { get; set; }
        public List<Item> Items { get; set; }
        public List<string> OnClickEventContent { get; set; }
    }
}

