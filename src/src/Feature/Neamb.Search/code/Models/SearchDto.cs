using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Search.Models
{
    public class SearchDto : IRenderingModel
    {
        public string Placeholder { get; set; }
        public int ResultCount { get; set; }
        public List<SearchResultCard> SearchResultCards { get; set; }
        public string MoreCta { get; set; }

        public string RedirectUrl { get; set; }
        public string KeywordParmName { get; set; }
        public string InitialSearchValue { get; set; }
        public int Take { get; set; }

        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
        }
    }
}