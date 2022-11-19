using System;
using Neambc.Neamb.Foundation.Indexing.Enums;
using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.Indexing.Models
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