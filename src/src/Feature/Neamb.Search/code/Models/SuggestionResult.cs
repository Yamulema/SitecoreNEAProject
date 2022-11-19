using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Search.Models
{
    public class SuggestionResult
    {
        //public Guid Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}