using Neambc.Neamb.Feature.Search.Models;

namespace Neambc.Neamb.Feature.Search.Interfaces
{
    public interface IStoreSearchResultManager
    {
        StoreResult GetStores(bool popularOffers, bool favoritesOnly, string[] categories, string storeName,
            int sort, int take, int skip);
        bool FavoriteStores(string store, bool behaviour);
    }
}