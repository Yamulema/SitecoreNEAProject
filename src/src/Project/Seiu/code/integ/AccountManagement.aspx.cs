using System;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Seiumb.Foundation.WebServices;
using Neambc.Seiumb.Foundation.WebServices.Managers;

namespace Neambc.Seiumb.Project.Seiu.integ
{
    public partial class AccountManagement : System.Web.UI.Page
    {
        private readonly IAccountServiceProxy _neambServiceManager;
		private readonly IWebServicesConfiguration _webServicesConfiguration;
        private readonly ISearchUserNameService _searchUserNameService;


        public AccountManagement(IAccountServiceProxy neambServiceManager, IWebServicesConfiguration webServicesConfiguration, ISearchUserNameService searchUserNameService)
        {
            _neambServiceManager = neambServiceManager;
			_webServicesConfiguration = webServicesConfiguration;
            _searchUserNameService = searchUserNameService;

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void TestIsUsernameAvailable(object sender, EventArgs e)
        {
            //var response = _neambServiceManager.ValidateAccountUserName(tbUsername.Value);
            //IsUsernameAvailableResponse.Value = (response != null && response.available.Equals("Y")).ToString();
            var response = _searchUserNameService.SearchUserNameStatus(tbUsername.Value);
            //IsUsernameAvailableResponse.Value = response.data.registered;

        }

        protected void TestValidateUsernameAndPassword(object sender, EventArgs e)
        {
            var response = _neambServiceManager.ValidateUsernameAndPasswordTemp(tbUsernameVUP.Value, tbPasswordVUP.Value, tbCellCodeVUP.Value,_webServicesConfiguration.UnionId,_webServicesConfiguration.MatchRoutineIdentifierSeium);
            //validateUsernameAndPasswordResponse.Value = JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        protected void TestRegisterUser(object sender, EventArgs e)
        {
            var response = _neambServiceManager.RegisterAccount(tbfirstNameRG.Value, tblastNameRG.Value, tbstreetAddressRG.Value, tbcityRG.Value,
                tbstateCodeRG.Value, tbzipCodeRG.Value, tbdobRG.Value, tbphoneRG.Value, tbUsernameRG.Value, tbPasswordRG.Value, tbpermissionIndicatorRG.Value,
                tbcampcodeRG.Value, tbcellCodeRG.Value, _webServicesConfiguration.UnionId,_webServicesConfiguration.Webusersource);

            //validateRegisterUserResponse.Value = JsonConvert.SerializeObject(response, Formatting.Indented);
        }
    }
}