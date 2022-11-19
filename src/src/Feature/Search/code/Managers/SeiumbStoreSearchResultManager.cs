using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Feature.Search.Interfaces;
using Neambc.Seiumb.Feature.Search.Models;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Indexing.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sitecore.Diagnostics;
using Neambc.Seiumb.Foundation.Indexing.Models;
using Neambc.Neamb.Foundation.MBCData.Managers;
using log4net;

namespace Neambc.Seiumb.Feature.Search.Managers
{
    [Service(typeof(ISeiumbStoreSearchResultManager))]
    public class SeiumbStoreSearchResultManager : ISeiumbStoreSearchResultManager
    {
        #region Private Readonly Variables
        private readonly ISeiumbStoreSearchManager _searchManager;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IOracleDatabase _oracleManager;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchManager"></param>
        /// <param name="sessionAuthenticationManager"></param>
        public SeiumbStoreSearchResultManager(ISeiumbStoreSearchManager searchManager, ISeiumbProfileManager seiumbProfileManager, IOracleDatabase oracleManager)
        {
            _searchManager = searchManager;
            _seiumbProfileManager = seiumbProfileManager;
            _oracleManager = oracleManager;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Return List of Stores and Member Status based on search parameters sent
        /// </summary>
        /// <param name="categories">Collection of Guids that represent categories to filter by, optional</param>
        /// <param name="favoritesOnly">If true brings favorites stores liked by the user, otherwise it brings all stores</param>
        /// <param name="popularOffers">If true brings popular offers stores, otherwise it brings all stores</param>
        /// <param name="storeName">Text string to search stores by name only, optional</param>
        /// <param name="sort">Sort options, (1) cashback desc, (2) Alpha desc (a-z)</param>
        /// <param name="take">Number of stores to take when paginating</param>
        /// <param name="skip">Number of stores to skip when paginating</param>
        /// <returns></returns>
        public StoreResult GetStores(bool popularOffers, bool favoritesOnly, string[] categories, string storeName,
            int sort, int take, int skip)
        {
            //If member is in a HOT state and is a Rakuten member then get EBToken
            var rakutenResponse = _seiumbProfileManager.GetRakutenMemberCreationResponse();
            var ebToken = _seiumbProfileManager.InHotState() && _seiumbProfileManager.IsRakutenMember() ? rakutenResponse.EBtoken : string.Empty;
            var favoriteStores = GetFavoriteStores();

            var (results, hasOneMorePage) = _searchManager.GetStores(popularOffers, favoritesOnly, categories, favoriteStores,
                storeName, sort, take, skip);

            var responseStores = results.Select(store => ToStoreResultCard(store, ebToken, favoriteStores))
                .Where(store => store != null);

            var storeResult = new StoreResult
            {
                IsRakutenMember = _oracleManager.RakutenRegExists(_seiumbProfileManager.GetProfile().Email),
                IsHotState = _seiumbProfileManager.InHotState(),
                stores = responseStores,
                hasOneMorePage = hasOneMorePage
            };
            return storeResult;
        }

        public void FavoriteStores(string store, bool behaviour)
        {
            if (!_seiumbProfileManager.InHotState() || !_seiumbProfileManager.IsRakutenMember()) {
                Log.Info($"Seiumb set Favorite Stores failed: Hot={_seiumbProfileManager.InHotState()} Rakuten={_seiumbProfileManager.IsRakutenMember()}", this);
                return;
            }
            if (!Guid.TryParse(store, out var storeGuid)) {
                Log.Info($"Seiumb set Favorite Stores failed parsing store ID={store}", this);
                return;
            }

            var rakutenResponse = _seiumbProfileManager.GetRakutenMemberCreationResponse();
            if (rakutenResponse.FavoriteStores == null)
                rakutenResponse.FavoriteStores = new List<Guid>();

            if (behaviour)
            {
                if (!rakutenResponse.FavoriteStores.Contains(storeGuid))
                    rakutenResponse.FavoriteStores.Add(storeGuid);
            }
            else
            {
                if (rakutenResponse.FavoriteStores.Contains(storeGuid))
                    rakutenResponse.FavoriteStores.Remove(storeGuid);
            }
            _seiumbProfileManager.SaveFavoriteStore(rakutenResponse);
        }
        #endregion

        #region Private Methods
        private List<Guid> GetFavoriteStores()
        {
            if (!_seiumbProfileManager.InHotState() || !_seiumbProfileManager.IsRakutenMember()) return new List<Guid>();
            return _seiumbProfileManager.GetRakutenMemberCreationResponse()?.FavoriteStores ?? new List<Guid>();
        }

        private StoreResultCard ToStoreResultCard(StoreSearchResult store, string ebToken, List<Guid> favoriteStores) {
            Log.Info($"Transform Store with GUID: {store.ItemId.Guid} started.", this);
            try
            {
                var storeList = new StoreResultCard
                {
                    IsFavorite = IsFavoriteStore(store, favoriteStores),
                    TotalReward = store.Type.Equals("Percentage") ? store.TotalReward.ToString(CultureInfo.InvariantCulture) : (int.TryParse(store.TotalReward.ToString(CultureInfo.InvariantCulture), out _) ? store.TotalReward.ToString(CultureInfo.InvariantCulture) : store.TotalReward.ToString("n2")),
                    BaseReward = store.Type.Equals("Percentage") ? store.BaseReward.ToString(CultureInfo.InvariantCulture) : (int.TryParse(store.BaseReward.ToString(CultureInfo.InvariantCulture), out _) ? store.BaseReward.ToString(CultureInfo.InvariantCulture) : store.BaseReward.ToString("n2")),
                    Categories = store.StoreCategories,
                    Tier = store.Tier,
                    Name = store.StoreName,
                    Banner = store.Banner,
                    SmallLogo = store.SmallLogo,
                    Thumbnail = store.Thumbnail,
                    Icon11230 = store.Icon11230,
                    Icon22460 = store.Icon22460,
                    Icon33690 = store.Icon33690,
                    LogoEmail = store.LogoEmail,
                    LogoMobile = store.LogoMobile,
                    LogoMobile2x = store.LogoMobile2x,
                    LogoMobile3x = store.LogoMobile3x,
                    FeedSquareLogo = store.FeedSquareLogo,
                    ShoppingUrl = $"{store.ShoppingUrl}{ebToken}",
                    StoreGuid = store.ItemId.ToString(),
                    Type = store.Type,
                    MemberOnly = store.MemberOnly
                };
                return storeList;
            }
            catch (Exception ex)
            {
                Log.Fatal($"Transform Store with GUID: {store.ItemId.Guid} failed.", ex, this);
                //Return null so the rest of the list still show up in the page.
                return null;
            }
        }

        private bool IsFavoriteStore(StoreSearchResult store, List<Guid> favoriteStores) {
            return favoriteStores.Contains(store.ItemId.Guid);
        }
        #endregion
    }
}