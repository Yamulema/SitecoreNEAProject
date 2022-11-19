using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Models;
using System;
using System.Web;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Newtonsoft.Json;
using Neambc.Neamb.Foundation.Membership.Model;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Services.Rakuten;
using Sitecore;
using Sitecore.Data;

namespace Neambc.Seiumb.Foundation.Authentication.Managers
{
    [Service(typeof(ISeiumbProfileManager))]
    public class SeiumbProfileManager : ISeiumbProfileManager
    {
        private readonly ICacheManagerSeiumb _cacheManagerSeiumb;
        private readonly IOracleDatabase _oracleManager;
        private readonly IRakutenMemberService _rakutenMemberService;

        public SeiumbProfileManager(ICacheManagerSeiumb cacheManagerSeiumb, IOracleDatabase oracleManager, IRakutenMemberService rakutenMemberService)
        {
            _cacheManagerSeiumb = cacheManagerSeiumb;
            _oracleManager = oracleManager;
            _rakutenMemberService = rakutenMemberService;
        }

        public MemberCreationResponse GetRakutenMemberCreationResponse()
        {
            if (!IsRakutenMember()) return null;

            var profile = GetProfile();
            var rakutenProfile = _oracleManager.ViewRakutenRegs(profile.Email);

            var response = new MemberCreationResponse
            {
                EmailAddress = profile.Email,
                Id = rakutenProfile.StoreId,
                EBtoken = rakutenProfile.EBToken,
                CreatedDate = rakutenProfile.CreateDate
            };

            if (string.IsNullOrEmpty(rakutenProfile.FavoriteStore))
            {
                response.FavoriteStores = new List<Guid>();
            }
            else
            {
                var stores = JsonConvert.DeserializeObject<List<StoreInfo>>(rakutenProfile.FavoriteStore);
                response.FavoriteStores = stores?.ConvertAll<Guid>(x => x.StoreGuid);
            }
            return response;
        }

        public bool IsRakutenMember()
        {
            return _oracleManager.RakutenRegExists(GetProfile().Email);
        }

        public SeiuProfile GetProfile()
        {
            var session = HttpContext.Current.Session.SessionID;
            return _cacheManagerSeiumb.RetrieveFromCache<SeiuProfile>($"seiumb:{session}") ?? new SeiuProfile();
        }

        public void SaveProfile(SeiuProfile profile, bool verifyExistence = false)
        {
            var session = HttpContext.Current.Session.SessionID;
            var key = $"seiumb:{session}";
            if (verifyExistence && _cacheManagerSeiumb.ExistInCache(key))//dont change expiration
                _cacheManagerSeiumb.StoreInCache(key, profile);
            else //save with expiration
                _cacheManagerSeiumb.StoreInCache(key, profile, DateTime.Now.AddHours(Configuration.CachedProfileTimeOut));
        }

        public void DeleteProfile()
        {
            var session = HttpContext.Current.Session.SessionID;
            var key = $"seiumb:{session}";
            _cacheManagerSeiumb.Remove(key);
        }

        public void SaveFavoriteStore(MemberCreationResponse rakutenResponse)
        {
            var storeIDs = rakutenResponse.FavoriteStores;
            var profile = GetProfile();
            var stores = storeIDs.Where(x => Context.Database.GetItem(new ID(x)) != null)
                .Select(ToStoreInfo);
            var favoriteStores = JsonConvert.SerializeObject(stores).Replace("'", "''");

            _oracleManager.Update_Favorite_Stores(profile.MdsId, rakutenResponse.EmailAddress, favoriteStores);
        }

        public bool InHotState()
        {
            var status = GetProfile().Status;
            return !string.IsNullOrEmpty(status) && status == UserStatusCons.HOT;
        }

        private StoreInfo ToStoreInfo(Guid storeGuid)
        {
            var store = new StoreInfo
            {
                StoreGuid = storeGuid,
                StoreName = Context.Database.GetItem(new ID(storeGuid)).DisplayName
            };
            return store;
        }
    }
}