using System;
using System.Collections.Generic;
using Neambc.Neamb.Foundation.Indexing.Models;

namespace Neambc.Neamb.Foundation.Indexing.Interfaces
{
    public interface IStoreSearchManager
    {
        (IEnumerable<StoreSearchResult> results, bool hasOneMorePage) GetStores(bool popularOffers, bool favoritesOnly, 
            string[] categories, List<Guid> favoriteStores, string storeName, int sort, int take, int skip);
    }
}