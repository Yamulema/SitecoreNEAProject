using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Search.Models
{
    public abstract class PageCard
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cta { get; set; }
    }
}