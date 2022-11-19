using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class Plan
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public List<Quote> Quotes { get; set; }
    }
}