using System;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;
using Neambc.Neamb.Foundation.MBCData.Services.RegisterUser;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
    [Service(typeof(IRegistrationManager))]
    public class RegistrationManager : IRegistrationManager
    {
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly ISessionManager _sessionManager;
		private readonly IGtmService _gtmService;
        private readonly IRegisterUserService _registerUserService;

        public RegistrationManager(IGlobalConfigurationManager globalConfigurationManager, ISessionManager sessionManager, IGtmService gtmService, IRegisterUserService registerUserService)
        {
            _globalConfigurationManager = globalConfigurationManager;
			_sessionManager = sessionManager;
			_gtmService = gtmService;
            _registerUserService = registerUserService;
        }

        public RegisterUserResponse RegisterAccount(AccountMembership account, string password, string cellcode,
            string campaingcode)
        {
            var resultService = _registerUserService.RegisterUserData(account.Profile.FirstName, account.Profile.LastName,
                account.Profile.StreetAddress, account.Profile.City, account.Profile.StateCode, account.Profile.ZipCode,
                account.Profile.DateOfBirth, account.Profile.Phone, account.Username.ToLower(), password,
                account.Profile.EmailPermissionIndicator, campaingcode, cellcode, Convert.ToInt32(_globalConfigurationManager.Unionid), _globalConfigurationManager.Webusersource);
            if (resultService.Data !=null && resultService.Data.Registered)
            {
                account.Status = StatusEnum.Registered;
            }

            return resultService;
        }

		/// <summary>
		/// See if there is session data to process the redirection after registration
		/// </summary>
		/// <param name="itemId">Page item id that is being currently processed</param>
		/// <returns>Gtm action to be included in the view</returns>
		public string ExecuteGtmActionRegistrationRedirection(string itemId) {
			var registrationRedirectionSucessRule = _sessionManager.RetrieveFromSession<string>(ConstantsNeamb.RegistrationRedirectionSuccessRule);
			string gtmAction = "";
			var registrationRedirectionItemId = _sessionManager.RetrieveFromSession<string>(ConstantsNeamb.RegistrationRedirectionItemId);
			if (!string.IsNullOrEmpty(registrationRedirectionSucessRule) && itemId.Equals(registrationRedirectionItemId))
			{
				_sessionManager.Remove(ConstantsNeamb.RegistrationRedirectionSuccessRule);
				_sessionManager.Remove(ConstantsNeamb.RegistrationRedirectionItemId);
				gtmAction = GetGtmActionRegistration(RegistrationEventResultEnum.Success);
			}
			return gtmAction;
		}

		/// <summary>
		/// Get the gtm action to be included in the view
		/// </summary>
		/// <param name="registrationEventResultEnum">Sucess or fail</param>
		/// <returns></returns>
		public string GetGtmActionRegistration(RegistrationEventResultEnum registrationEventResultEnum)
		{
			RegistrationEvent registrationEvent = new RegistrationEvent
			{
				Event = "registration",
				RegistrationAction = "submit",
				RegistrationResult = registrationEventResultEnum.GetDescription()
			};
			return _gtmService.GetGtmEvent(registrationEvent);
		}
	}
}