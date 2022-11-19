using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Model
{
    public class ProductMultiOfferRadioOption
    {
        public string RadioDisplayText { get; set; }
        public string RadioIdentifier { get; set; }
        public string RadioValueText { get; set; }
        public bool IsDefaulSelectedContent { get; set; }
        public bool IsDefaulSelected { get; set; }
        public string ParameterId { get; set; }
        public string ParameterMatchValue { get; set; }
        public Item Item { get; set; }
    }
}