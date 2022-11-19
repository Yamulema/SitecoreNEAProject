using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Model
{
    public class ProductMultiOfferRadioOptionGroup
    {
        public Item Item { get; set; }
        public string RadioGroupDescription { get; set; }
        public string RadioGroupParameter { get; set; }
        public bool IsMandatory { get; set; }
        public List<ProductMultiOfferRadioOption> RadioOptions { get; set; }
        public string GroupId { get; set; }
    }
}