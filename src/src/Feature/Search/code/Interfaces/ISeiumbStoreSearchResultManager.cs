using Neambc.Seiumb.Feature.Search.Models;

namespace Neambc.Seiumb.Feature.Search.Interfaces
{
    public interface ISeiumbStoreSearchResultManager {
        StoreResult GetStores(bool popularOffers, bool favoritesOnly, string[] categories, string storeName, int sort, int take, int skip);
        void FavoriteStores(string store, bool behaviour);
    }
}