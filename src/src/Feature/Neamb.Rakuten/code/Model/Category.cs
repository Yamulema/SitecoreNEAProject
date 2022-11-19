using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Rakuten.Model
{
    public class Category
    {
        public Guid Guid { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool NeambEnabled { get; set; }
        public bool SeiumbEnabled { get; set; }
        public List<Category> Subcategories { get; set; }
    }
}