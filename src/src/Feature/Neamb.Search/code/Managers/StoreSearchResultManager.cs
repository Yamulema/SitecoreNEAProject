using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Feature.Search.Interfaces;
using Neambc.Neamb.Feature.Search.Models;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Indexing.Models;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Neambc.Neamb.Feature.Search.Managers
{
    [Service(typeof(IStoreSearchResultManager))]
    public class StoreSearchResultManager : IStoreSearchResultManager {
        #region Private Readonly Variables
        private readonly IStoreSearchManager _searchManager;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IAccountRepository _account;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchManager"></param>
        /// <param name="sessionAuthenticationManager"></param>
        public StoreSearchResultManager(IStoreSearchManager searchManager, IAccountRepository account,
            ISessionAuthenticationManager sessionAuthenticationManager) {
            _searchManager = searchManager;
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _account = account;

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Return List of Stores and Member Status based on search parameters sent
        /// </summary>
        /// <param name="categories">Collection of Guids that represent categories to filter by, optional</param>
        /// <param name="memberOnly">If true brings member only stores, otherwise it brings all stores member or not </param>
        /// <param name="content">Text string to search stores by name only, optional</param>
        /// <param name="sort">Sort options, (1) cashback desc, (2) Alpha desc (a-z)</param>
        /// <param name="page">Page to retrieve, page size is fixed to 20 items</param>
        /// <returns></returns>
        public StoreResult GetStores(bool popularOffers, bool favoritesOnly, string[] categories, string storeName,
            int sort, int take, int skip) {
            var _member = _sessionAuthenticationManager.GetAccountMembership();
            //If member is in a HOT state and is a Rakuten member then get EBToken
            _member.Profile = _account.RetrieveRakutenProfile(_member);
            var ebToken = _member.Status == StatusEnum.Hot && _member.Profile.IsRakutenMember ? _member.Profile.RakutenProfile.EBToken : string.Empty;
            var favoriteStores = GetFavoriteStores();

            var (results, hasOneMorePage) = _searchManager.GetStores(popularOffers, favoritesOnly, categories, favoriteStores,
                storeName, sort, take, skip);

            var responseStores = results.Select(store => ToStoreResultCard(store, ebToken, favoriteStores))
                .Where(store => store != null);

            var storeResult = new StoreResult {
                IsRakutenMember = _member.Profile.IsRakutenMember,
                IsHotState = _member.Status == StatusEnum.Hot,
                stores = responseStores,
                hasOneMorePage = hasOneMorePage
            };
            return storeResult;
        }

        public bool FavoriteStores(string store, bool behaviour) {
            var _member = _sessionAuthenticationManager.GetAccountMembership();
            if (_member.Status != StatusEnum.Hot || !_member.Profile.IsRakutenMember) return false;
            if (!Guid.TryParse(store, out var storeGuid)) return false;

            if (_member.Profile.RakutenProfile.FavoriteStores == null)
                _member.Profile.RakutenProfile.FavoriteStores = new List<StoreInfo>();

            if (behaviour) {
                if (!_member.Profile.RakutenProfile.FavoriteStores.Select<StoreInfo, Guid>(x => x.StoreGuid).ToList().Contains(storeGuid))
                {
                    var storeItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(storeGuid));

                    var newStore = new StoreInfo
                    {
                        StoreGuid = storeGuid,
                        StoreName = storeItem.DisplayName
                    };
                    _member.Profile.RakutenProfile.FavoriteStores.Add(newStore);
                }
            } else {
                if (_member.Profile.RakutenProfile.FavoriteStores.Select<StoreInfo, Guid>(x => x.StoreGuid).ToList().Contains(storeGuid))
                    _member.Profile.RakutenProfile.FavoriteStores.RemoveAll(x=>x.StoreGuid == storeGuid);
            }
            return _account.SaveFavoriteStore(_member.Mdsid, _member.Username, _member.Profile.RakutenProfile.FavoriteStores);
        }
        #endregion

        #region Private Methods
        private List<Guid> GetFavoriteStores()
        {
            var _member = _sessionAuthenticationManager.GetAccountMembership();
            if (_member.Status != StatusEnum.Hot || !_member.Profile.IsRakutenMember
                || _member.Profile.RakutenProfile.FavoriteStores == null) return new List<Guid>();
            var results = _member.Profile.RakutenProfile.FavoriteStores.Select<StoreInfo, Guid>(x => x.StoreGuid).ToList();
            return results;
        }

        private StoreResultCard ToStoreResultCard(StoreSearchResult store, string ebToken, List<Guid> favoriteStores)
        {
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

        private bool IsFavoriteStore(StoreSearchResult store, List<Guid> favoriteStores)
        {
            return favoriteStores.Contains(store.ItemId.Guid);
        }
        #endregion
    }
}