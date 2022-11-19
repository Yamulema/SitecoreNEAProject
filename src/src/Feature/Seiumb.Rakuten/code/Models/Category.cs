using System;
using System.Collections.Generic;

namespace Neambc.Seiumb.Feature.Rakuten.Models
{
    public class Category
    {
        public Guid Guid { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool NeambEnabled { get; set; }
        public bool SeiumbEnabled { get; set; }
        public IEnumerable<Category> Subcategories { get; set; }
    }
}