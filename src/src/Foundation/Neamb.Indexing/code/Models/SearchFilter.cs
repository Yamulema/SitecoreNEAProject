using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Indexing.Enums;

namespace Neambc.Neamb.Foundation.Indexing.Models
{
    public class SearchFilter
    {
        public FilterType Type { get; set; }
        public object Value { get; set; }
    }
}