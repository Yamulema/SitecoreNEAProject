using Neambc.Seiumb.Foundation.Indexing.Models;
using System;
using System.Collections.Generic;

namespace Neambc.Seiumb.Foundation.Indexing.Interfaces
{
    public interface ISeiumbStoreSearchManager
    {
        (IEnumerable<StoreSearchResult> results, bool hasOneMorePage) GetStores(bool popularOffers, bool favoritesOnly,
            string[] categories, List<Guid> favoriteStores, string storeName, int sort, int take, int skip);
    }
}