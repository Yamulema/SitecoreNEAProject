using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Search.Models
{
    public class ContentFilterResult
    {
        public int Total { get; set; }
        public List<string> AllCategories { get; set; }
        public List<ContentPageCard> PageCards { get; set; }
    }
}