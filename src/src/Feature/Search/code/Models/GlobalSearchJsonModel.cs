using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Search.Models
{
    public class GlobalSearchJsonModel
    {
        public string title { get; set; }
        public string content { get; set; }
        public string url { get; set; }
        public string onClickEventTitle { get; set; }
        public string onClickEventUrl { get; set; }
    }
}