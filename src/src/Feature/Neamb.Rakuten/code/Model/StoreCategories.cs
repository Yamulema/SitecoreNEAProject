using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Rakuten.Model
{
    public class StoreCategories
    {
        public IEnumerable<Category> Categories { get; set; }
        public bool ShowFavorites { get; set; }
    }
}