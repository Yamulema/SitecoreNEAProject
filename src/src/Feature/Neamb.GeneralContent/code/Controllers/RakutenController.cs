using System.Collections.Generic;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Services.Rakuten;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class RakutenController : BaseController
    {
        private const string CREATION_MEMBER_VIEW = "/Views/Neamb.GeneralContent/Rakuten/CreationMember.cshtml";
        private const string MEMBER_PROFILE_VIEW = "/Views/Neamb.GeneralContent/Rakuten/MemberProfileInfo.cshtml";
        
        private readonly IRakutenMemberService _rakutenMemberService;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;

        public RakutenController(IRakutenMemberService rakutenMemberService, ISessionAuthenticationManager sessionAuthenticationManager, IAuthenticationAccountManager authenticationAccountManager)
        {
            _rakutenMemberService = rakutenMemberService;
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _authenticationAccountManager = authenticationAccountManager;
        }

        /// <summary>
        /// Get Creation Member view
        /// </summary>
        /// <returns></returns>
        public ActionResult CreationMember()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return View(CREATION_MEMBER_VIEW, accountMembership);
        }

        public ActionResult CreationMemberAction()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            var cellCode = _sessionAuthenticationManager.GetCellCode();
            var memberCreationResponse = _rakutenMemberService.CreateMember( true, accountMembership.Profile.Email, accountMembership.Mdsid, cellCode);
            MemberCreationResponseView model =
                new MemberCreationResponseView {
                    Data = memberCreationResponse.Result,
                    Success = memberCreationResponse.Success
                };
            var newAccountMembership = _sessionAuthenticationManager.GetAccountMembership();
            newAccountMembership.Profile.IsRakutenMember = true;
            newAccountMembership.Profile.RakutenProfile = new RakutenMemberModel
            {
                EmailAddress = model.Data.EmailAddress,
                Id = model.Data.Id,
                CreatedDate = model.Data.CreatedDate,
                EBToken = model.Data.EBtoken,
                FavoriteStores = new List<StoreInfo>()
            };
            _authenticationAccountManager.InitializeAccountMemberData(newAccountMembership);
            return View(MEMBER_PROFILE_VIEW, model);
        }
    }
}