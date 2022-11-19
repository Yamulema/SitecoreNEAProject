using Neambc.Seiumb.Foundation.Indexing.Enums;
using System;
using System.Collections.Generic;

namespace Neambc.Seiumb.Foundation.Indexing.Models
{
    public class StoreSearchRequest
    {
        public List<Guid> Categories { get; set; }
        public string Content { get; set; }
        public StoreSortBy Sort { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}