using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Search.Enums;

namespace Neambc.Neamb.Feature.Search.Models
{
    public class SearchResultCard : PageCard
    {
        public string ThumbnailSrc { get; set; }
        public string Genre { get; set; }
        public PageResultStyle Style { get; set; }
    }
}