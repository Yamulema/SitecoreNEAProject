using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Models;
using Neambc.Seiumb.Foundation.WebServices;

namespace Neambc.Seiumb.Foundation.Authentication.Managers
{
    [Service(typeof(IPassthroughService))]
	public class PassthroughService : IPassthroughService {
		private readonly IAuthenticationRepository _authenticationRepository;
		private readonly IPartnerFactory _partnerFactory;
		private readonly ILockedAccountService _lockedAccountService;
		private readonly IWebServicesConfiguration _webServicesConfiguration;
        private readonly IProductRestManagerSeiumb _productRestManagerSeiumb;

        public PassthroughService(IAuthenticationRepository authenticationRepository, IPartnerFactory partnerFactory, ILockedAccountService lockedAccountService, IWebServicesConfiguration webServicesConfiguration, IProductRestManagerSeiumb productRestManagerSeiumb) {
			_authenticationRepository = authenticationRepository;
			_partnerFactory = partnerFactory;
			_lockedAccountService = lockedAccountService;
			_webServicesConfiguration = webServicesConfiguration;
            _productRestManagerSeiumb = productRestManagerSeiumb;
        }
		public AuthenticationResponse Authenticate(PassthroughRequest passthroughRequest) {
			var isValid = ValidateRequest(passthroughRequest);
			var result = new AuthenticationResponse() {
				Status = LoginUserErrorCodeEnum.None,
				ReturnUrl = string.Empty,
				IsEligible = false
			};
			if (!isValid) return result;

			var response = _authenticationRepository.ValidateUsernameAndPassword(passthroughRequest.Username, passthroughRequest.Password, 
                string.Empty,_webServicesConfiguration.MatchRoutineIdentifierSeium);

            if (response != null && response.Success) {
                result.Status = response.ErrorCodeResponse;
                if (response.Data == null || !response.Data.LoggedIn) return result;
                result.IsEligible = IsElegible(response.Data.MdsIdAsString);
                result.ReturnUrl = GetReturnUrl(passthroughRequest);
                result.LoggedIn = true;

            } else {
                if (response == null) return result;
                if (response.ErrorCodeResponse == LoginUserErrorCodeEnum.AccountLocked) result.LoginErrors = 
                    _lockedAccountService.HandleLockedAccount(passthroughRequest.Username, out var isUsernameValid, true);
                result.Status = response.ErrorCodeResponse;
            }
			return result;
		}

		private bool ValidateRequest(PassthroughRequest passthroughRequest) {
			if (string.IsNullOrEmpty(passthroughRequest.Password)) return false;
            if (string.IsNullOrEmpty(passthroughRequest.Username)) return false;
            return !string.IsNullOrEmpty(passthroughRequest.ProductCode);
        }

		private string GetReturnUrl(PassthroughRequest passthroughRequest) {
			var partner = _partnerFactory.GetPartner(passthroughRequest.ProductCode);
			return partner != null ? partner.GetActionPrimary(passthroughRequest.QueryStringParameters) : string.Empty;
		}

		private bool IsElegible(string mdsId) {
            int.TryParse(mdsId, out var mdsidInt);
            return _productRestManagerSeiumb.GetEligibility(mdsidInt);
		}
	}
}