using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.AuthenticatePassword;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.WebServices;
using Sitecore.Diagnostics;
using System;
using Neambc.Seiumb.Foundation.Authentication.Models;

namespace Neambc.Seiumb.Foundation.Authentication.Managers
{
    [Service(typeof(IAuthenticationManager))]
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IWebServicesConfiguration _webServicesConfiguration;
        private readonly IRetrieveUserManager _profileManager;
        private readonly IBase64Service _base64Service;
        private readonly IAuthenticatePasswordService _authenticatePasswordService;
        private readonly IUserRepository _userRepository;

        public AuthenticationManager(IAuthenticationRepository authenticationRepository, IWebServicesConfiguration webServicesConfiguration,
            IRetrieveUserManager profileManager, IBase64Service base64Service, IAuthenticatePasswordService authenticatePasswordService,
            IUserRepository userRepository)
        {
            _authenticationRepository = authenticationRepository;
            _webServicesConfiguration = webServicesConfiguration;
            _profileManager = profileManager;
            _base64Service = base64Service;
            _authenticatePasswordService = authenticatePasswordService;
            _userRepository = userRepository;
        }

        public LoginResponse AuthenticateUser(SeiuProfile seiuProfile, string username, string password, string cellcode)
        {
            try
            {
                if(string.IsNullOrEmpty(password))
                {
                    Log.Error($"method: AuthenticateUser, model password is empty, username: {username}", this);
                }                
                var response = _authenticationRepository.ValidateUsernameAndPassword(username, password, cellcode, _webServicesConfiguration.MatchRoutineIdentifierSeium);
                if (response != null && response.Success && response.Data != null && response.Data.LoggedIn)
                    _userRepository.CreateVirtualUser(username, seiuProfile, response, true);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
            }
            return new LoginResponse { Success = false };
        }

        public void FillUserData(SeiuProfile seiuProfile, string mdsid, string neaCookieMdsid, bool fromLogin = false, string registrations = null)
        {
            var userDataResponse = _profileManager.RetrieveUserSeiumb(mdsid);
            if (userDataResponse == null) return;
            var profile = _profileManager.ToProfileModel(userDataResponse);
            _userRepository.FillUserData(profile, seiuProfile, mdsid, fromLogin, registrations);
            //CREATES COOKIE USED TO WARM USER
            CookieStore.SetCookie(neaCookieMdsid, _base64Service.Encode(mdsid), TimeSpan.FromDays(Convert.ToInt32(15)));
        }

        public bool AuthenticatePassword(string username, string password)
        {
            try
            {
                var response = _authenticatePasswordService.AuthenticatePasswordStatus(username, password, Convert.ToInt32(_webServicesConfiguration.UnionId));
                if (response.Success) return response.Data.authenticated;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
            }
            return false;
        }
    }
}