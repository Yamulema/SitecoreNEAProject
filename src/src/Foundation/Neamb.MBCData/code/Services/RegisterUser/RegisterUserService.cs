using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.RetrieveUser;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Neamb.Foundation.MBCData.Services.RetrieveUser;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.RegisterUser
{
    [Service(typeof(IRegisterUserService))]
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public RegisterUserService(IAccessTokenService accessTokenService, IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService,
            ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }


        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="streetAddress">Address</param>
        /// <param name="city">City</param>
        /// <param name="stateCode">State</param>
        /// <param name="zipCode">Zip</param>
        /// <param name="dob">Birth Date</param>
        /// <param name="phone">Phone</param>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="permissionIndicator">Indicater for alerts</param>
        /// <param name="campcode">Campaing code</param>
        /// <param name="cellCode">Cell code</param>
        /// <param name="unionId">Seiumb/Neamb</param>
        /// <param name="webusersource">Webusersource Seiumb/Neamb</param>
        /// <returns></returns>
        /// <returns>Response of the AWS WebService</returns>
        public RegisterUserResponse RegisterUserData(string firstName,
            string lastName,
            string streetAddress,
            string city,
            string stateCode,
            string zipCode,
            string dob,
            string phone,
            string username,
            string password,
            string permissionIndicator,
            string campcode,
            string cellCode, int unionId, string webusersource)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            _logService.Debug($@"firstName:{firstName}");
            _logService.Debug($@"lastName:{lastName}");
            _logService.Debug($@"streetAddress:{streetAddress}");
            _logService.Debug($@"stateCode:{stateCode}");
            _logService.Debug($@"zipCode:{zipCode}");
            _logService.Debug($@"dob:{dob}");
            _logService.Debug($@"username:{username}");
            _logService.Debug($@"password:{password}");

            if (string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(streetAddress) ||
                //string.IsNullOrEmpty(city) ||
                string.IsNullOrEmpty(stateCode) ||
                string.IsNullOrEmpty(zipCode) ||
                string.IsNullOrEmpty(dob) ||
                //string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) ||
                unionId < 0)
            {
                
                throw new ArgumentException($"Parameters for RegisterUserData - RegisterUserData are incorrect");
            }
            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlRegisterUser,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new RegisterUserRequest
                {
                    Username = username,
                    CampCode = campcode,
                    CellCode= cellCode,
                    City = city,
                    Dob = dob,
                    FirstName = firstName,
                    LastName = lastName,
                    Password = password,
                    PermissionIndicator = permissionIndicator,
                    Phone = phone,
                    UnionId = unionId,
                    StateCode = stateCode,
                    StreetAddress = streetAddress,
                    WebUserSource = webusersource,
                    ZipCode = zipCode
                },
                IsBasicAuthentication = false
            };
            var response = _mbcRestfulService.Post<RegisterUserResponse>(restRequestDto);
            if (response.Success) {
                if (response.Result != null && !response.Result.Success && response.Result.Error!=null) {
                    _responseHandler.LogErrorResponse(response.Result.Error, "RetrieveUserService", _logService);
                }
            } else {
                _logService.Error($"Post Error to RetrieveUser: {response.StatusCode}, {response.ExceptionDetail}", this);
            }

            return response.Result;
        }
    }
}