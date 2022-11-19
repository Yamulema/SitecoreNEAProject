using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services.Rakuten;

using Neambc.Seiumb.Foundation.Authentication.Interfaces;

namespace Neambc.Seiumb.Foundation.Rakuten.Manager
{
    [Service(typeof(IRakutenRegistrationSeiumbManager))]
    public class RakutenRegistrationSeiumbManager: IRakutenRegistrationSeiumbManager
    {
        private readonly IRakutenMemberService _rakutenMemberService;
        private readonly ISeiumbProfileManager _seiumbProfileManager;

        public RakutenRegistrationSeiumbManager(IRakutenMemberService rakutenMemberService, 
            ISeiumbProfileManager seiumbProfileManager) {
            _rakutenMemberService = rakutenMemberService;
            _seiumbProfileManager = seiumbProfileManager;
        }
        public bool CheckSignUpRakutenUser(string cellCode) {
            var profile = _seiumbProfileManager.GetProfile();
            var memberCreationResponse = _rakutenMemberService.CreateMember(false, profile.Email, profile.MdsId, cellCode);
            var model = memberCreationResponse.Result;
            return memberCreationResponse.Success;
        }
    }
}