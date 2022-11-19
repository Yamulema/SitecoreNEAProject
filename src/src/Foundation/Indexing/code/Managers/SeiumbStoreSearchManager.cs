using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.Indexing.Enums;
using Neambc.Seiumb.Foundation.Indexing.Interfaces;
using Neambc.Seiumb.Foundation.Indexing.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neambc.Seiumb.Foundation.Indexing.Managers
{
    [Service(typeof(ISeiumbStoreSearchManager))]

    public class SeiumbStoreSearchManager : ISeiumbStoreSearchManager
    {
        private IGlobalConfigurationManager _globalConfigurationManager;

        public SeiumbStoreSearchManager(IGlobalConfigurationManager globalConfigurationManager)
        {
            _globalConfigurationManager = globalConfigurationManager;            
        }

        private StoreSearchRequest CreateSearchRequest(string[] categories, string content, int sort, int take, int skip)
        {
            var categoriesList = new List<Guid>();
            if (categories != null)
            {
                foreach (var category in categories.ToList())
                {
                    if (Guid.TryParse(category, out var categoryGuid))
                    {
                        categoriesList.Add(categoryGuid);
                    }
                }
            }
            return new StoreSearchRequest
            {
                Sort = Enum.TryParse<StoreSortBy>(sort.ToString(), out var sortBy) ? sortBy : StoreSortBy.CashBack,
                Categories = categoriesList,
                Content = content,
                Take = take,
                Skip = skip,
            };
        }

        public (IEnumerable<StoreSearchResult> results, bool hasOneMorePage) GetStores(bool popularOffers, bool favoritesOnly,
            string[] categories, List<Guid> favoriteStores, string storeName, int sort, int take, int skip)
        {
            var request = CreateSearchRequest(categories, storeName, sort, take, skip);
            var index = ContentSearchManager.GetIndex(_globalConfigurationManager.StoreIndex);
            using (var context = index.CreateSearchContext())
            {
                try
                {
                    var query = PredicateBuilder.True<StoreSearchResult>();

                    #region Popular Offers
                    var popularOffersPredicate = PredicateBuilder.True<StoreSearchResult>();
                    popularOffersPredicate = popularOffersPredicate.Or(x => x.PopularOffer);
                    #endregion

                    #region Favorites Only
                    var favoritesOnlyPredicate = PredicateBuilder.True<StoreSearchResult>();
                    foreach (var store in favoriteStores)
                        favoritesOnlyPredicate = favoritesOnlyPredicate.Or(x => x.ItemId == ID.Parse(store));
                    #endregion

                    #region Filters

                    if (request.Categories.Any() || !string.IsNullOrEmpty(request.Content))
                    {
                        #region Categories
                        if (request.Categories.Any())
                        {
                            var categoriesPredicate = PredicateBuilder.True<StoreSearchResult>();
                            foreach (var category in request.Categories)
                                categoriesPredicate = categoriesPredicate.Or(x => x.StoreCategories.Contains(category));

                            query = query.Or(categoriesPredicate.Boost(600f));

                            if (popularOffers) query = query.And(popularOffersPredicate.Boost(600f));
                            if (favoritesOnly)
                                if (favoriteStores.Any()) query = query.And(favoritesOnlyPredicate.Boost(1000f));
                        }
                        #endregion

                        #region Content
                        if (!string.IsNullOrWhiteSpace(request.Content))
                        {
                            var contentPredicate = PredicateBuilder.True<StoreSearchResult>();
                            contentPredicate = contentPredicate.Or(x => x.StoreName.Contains(request.Content));
                            query = query.And(contentPredicate.Boost(600f));
                        }
                        #endregion
                    } else {
                        if (popularOffers) query = query.Or(popularOffersPredicate.Boost(800f));

                        if (favoritesOnly)
                        {
                            if (!favoriteStores.Any()) return (new List<StoreSearchResult>(), false);
                            query = query.Or(favoritesOnlyPredicate.Boost(1000f));
                        }
                    }

                    #endregion

                    //Exclude Stores
                    var excludeStoresPredicate = PredicateBuilder.True<StoreSearchResult>();
                    excludeStoresPredicate = excludeStoresPredicate.And(x => x.SEIUMBEnable == true); //disabled for SEIUMB
                    excludeStoresPredicate = excludeStoresPredicate.And(x => x.Type != "Coupons"); //exclude Coupons
                    query = query.And(excludeStoresPredicate);

                    //Max number of records for the search results
                    var queryable = context.GetQueryable<StoreSearchResult>()
                        .Where(query)
                        .Skip(request.Skip)
                        .Take(request.Take);

                    #region Sorting
                    switch (request.Sort)
                    {
                        case StoreSortBy.CashBack:
                            queryable = queryable.OrderByDescending(x => x.TotalReward);
                            break;
                        case StoreSortBy.Alphabetically:
                            queryable = queryable.OrderBy(x => x.StoreNameIndex);
                            break;
                    }
                    #endregion

                    //Hits search engine.
                    var result = queryable.GetResults();
                    var hasOneMorePage = (result.TotalSearchResults - request.Skip - request.Take) > 0;
                    var data = result.Hits.Any() ? result.Hits.Select(x => x.Document).ToList() : new List<StoreSearchResult>();
                    return (data, hasOneMorePage);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Exception on Store Search", ex, this);
                    return (new List<StoreSearchResult>(), false);
                }
            }
        }
    }
}