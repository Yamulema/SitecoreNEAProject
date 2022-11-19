using System;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Services.AuthenticatePassword;
using Neambc.Neamb.Foundation.MBCData.Services.Login;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.WebServices;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Sitecore.Diagnostics;

namespace Neambc.Seiumb.Foundation.Authentication.Repositories
{
    [Service(typeof(IAuthenticationRepository))]
    public class AuthenticationRepository : IAuthenticationRepository
    {
        
        private readonly IWebServicesConfiguration _webServicesConfiguration;
        private readonly ISearchUserNameService _searchUserNameService;
        private readonly ILoginUserService _loginUserService;
        private readonly IAuthenticatePasswordService _authenticatePasswordService;

        public AuthenticationRepository(IWebServicesConfiguration webServicesConfiguration, ISearchUserNameService searchUserNameService, ILoginUserService loginUserService, IAuthenticatePasswordService authenticatePasswordService)
        {
            _authenticatePasswordService = authenticatePasswordService;
            _webServicesConfiguration = webServicesConfiguration;
            _searchUserNameService = searchUserNameService;
            _loginUserService = loginUserService;
        }

        // return:  1  if username is valid and not registered
        //          0  if username is valid but registered
        //         -1  if username is invalid
        public int IsUsernameAvailable(string username)   
        {
            var response =  _searchUserNameService.SearchUserName(username);
            if (!response.Success) return -1;
            return response.Data.Registered ? 0 : 1;
        }

        public LoginResponse ValidateUsernameAndPassword(string username, string password, string cellcode,string matchRoutine) {
            if (string.IsNullOrEmpty(password))
            {
                Log.Error($"method: ValidateUsernameAndPassword, model password is empty. Username {username}", this);
            }

            return _loginUserService.LoginUser(username, password, Convert.ToInt32(_webServicesConfiguration.UnionId), cellcode, matchRoutine);
        }

        public bool AuthenticatePassword(string username, string password) {
		    var serviceResponse = _authenticatePasswordService.AuthenticatePasswordStatus(username, password, Convert.ToInt32(_webServicesConfiguration.UnionId));
            return serviceResponse.Data.authenticated;
        }
    }
}