using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.MBCData.Services.Rakuten;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using System.Collections.Generic;
using System.Linq;

namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    [Service(typeof(IRakutenMemberManager))]
    public class RakutenMemberManager: IRakutenMemberManager
    {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IRakutenMemberService _rakutenMemberService;
        private readonly IStoreSearchManager _storeSearchManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;
        public RakutenMemberManager(ISessionAuthenticationManager sessionAuthenticationManager,
            IRakutenMemberService rakutenMemberService, IStoreSearchManager storeSearchManager, IAuthenticationAccountManager authenticationAccountManager) {
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _rakutenMemberService = rakutenMemberService;
            _storeSearchManager = storeSearchManager;
            _authenticationAccountManager = authenticationAccountManager;
        }
        public bool CheckSignUpRakutenUser() {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            var memberCreationResponse = _rakutenMemberService.CreateMember(true, accountMembership.Profile.Email, accountMembership.Mdsid,
                _sessionAuthenticationManager.GetCellCode());
            var model = memberCreationResponse.Result;
            if (memberCreationResponse.Success) {
               accountMembership.Profile.IsRakutenMember = true;

                var stores = model.FavoriteStores?.ConvertAll<StoreInfo>(x => new StoreInfo
                {
                    StoreGuid = x,
                    StoreName = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(x)).DisplayName
                });
                accountMembership.Profile.RakutenProfile = new RakutenMemberModel {
                    EmailAddress = model.EmailAddress,
                    Id = model.Id,
                    CreatedDate = model.CreatedDate,
                    EBToken = model.EBtoken,
                    FavoriteStores = stores == null ? new List<StoreInfo>() : stores
                };
                _authenticationAccountManager.InitializeAccountMemberData(accountMembership);
                return true;
            } else {
                return false;
            }
        }
    }
}