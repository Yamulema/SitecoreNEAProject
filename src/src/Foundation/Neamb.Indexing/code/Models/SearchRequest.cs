using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Indexing.Enums;
using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Indexing.Models
{
    public class SearchRequest {
        public IEnumerable<SearchFilter> Filters { get; set; }
        public IEnumerable<ID> ExcludeIds { get; set; } = new List<ID>();
        public SortBy SortBy { get; set; } = SortBy.None;
        public int Take { get; set; } = 1000;
    }
}