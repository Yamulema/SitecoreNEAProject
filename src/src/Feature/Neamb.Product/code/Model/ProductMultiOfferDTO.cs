using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Product.Model
{
    public class ProductMultiOfferDTO
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }

        public List<ProductMultiOfferRadioOptionGroup> RadioOptionGroups { get; set; }
        public List<ProductMultiOfferMappingItem> Mapping { get; set; }
        public string UrlPartner { get; set; }
        public string ButtonName { get; set; }
        public string ButtonTarget { get; set; }
        public string CancelUrl { get; set; }
        public Dictionary<string, string> PostParams { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            UrlPartner = Item.LinkFieldUrl(Templates.ProductMultiOffer.Fields.TargetUrl);
            LinkField link = Item.Fields[Templates.ProductMultiOffer.Fields.TargetUrl];
            ButtonName = link.Text;
            if (!string.IsNullOrEmpty(link.Target))
            {
                ButtonTarget = "_blank";
            }            
        }      

    }
}