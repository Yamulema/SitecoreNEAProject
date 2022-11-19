using System;
using System.Globalization;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.Login
{
    [Service(typeof(ILoginUserService))]
    public class LoginUserService : ILoginUserService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public LoginUserService(IAccessTokenService accessTokenService,
            IGlobalConfigurationManager config,
            IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <param name="unionId">union</param>
        /// <param name="cellcode">Cell code</param>
        /// <param name="matchRoutineIdentifier">match routine identifier</param>
        /// <returns>User information</returns>
        public LoginResponse LoginUser(string userName, string password, int unionId, string cellcode, string matchRoutineIdentifier)
        {
            if (string.IsNullOrEmpty(password))
            {
                _logService.Error($"method: LoginUser, model password is empty, username: {userName}", this);
            }
            if (unionId < 0) throw new ArgumentException("invalid unionId (< 0)", "unionId");
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("username is empty", "username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("password is empty", "password");

            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken)) {
                throw new ArgumentException("token invalid", "token");
            }

            var restRequestDto = new RestRequestDto()
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlLogin,
                Body = new LoginRequest { Username = userName, CellCode = cellcode, MatchRoutineIdentifier = matchRoutineIdentifier,Password = password,UnionId = unionId},
                Token = token.Data.AccessToken
            };

            var response = _mbcRestfulService.Post<LoginResponse>(restRequestDto);
            response.Result.IsAuthenticated = false;
            if (response.Success) {
                var resultLogin = response.Result?.Data;
                if (resultLogin != null && response.Result.Success) {
                    resultLogin.MdsIdAsString = resultLogin.MdsId.ToString().PadLeft(9, '0');
                    foreach (var registration in resultLogin.Registrations) {
                        registration.WebUserIdAsString = registration.WebUserId.ToString().PadLeft(9, '0');
                    }
                    //Set the duplication registration old format required for Seiumb
                    string registrationDuplicateOldFormat = "";

                    foreach (var registrationDuplicatedItem in resultLogin.Registrations)
                    {
                        DateTime dateregisterParser;
                        if (DateTime.TryParseExact(registrationDuplicatedItem.RegistrationDate, "MM-dd-yyyy", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out dateregisterParser))
                        {
                            string duplicateEmails = $"{registrationDuplicatedItem.Email}|{registrationDuplicatedItem.FirstName}|{registrationDuplicatedItem.LastName}|{dateregisterParser.ToString("MMddyyyy")}|{registrationDuplicatedItem.WebUserId}";
                            if (string.IsNullOrEmpty(registrationDuplicateOldFormat))
                            {
                                registrationDuplicateOldFormat = $"{duplicateEmails}";
                            }
                            else
                            {
                                registrationDuplicateOldFormat = $"{registrationDuplicateOldFormat};{duplicateEmails}";
                            }
                        }
                    }
                    resultLogin.RegistrationDuplicateOldFormat = registrationDuplicateOldFormat;
                    response.Result.IsAuthenticated = true;

                } else {

                    response.Result.ErrorCodeResponse = (LoginUserErrorCodeEnum) response.Result.Error.Code;
                    if (response.Result != null) {
                        _responseHandler.LogErrorResponse(response.Result.Error, "LoginUserService", _logService);
                    } else {
                        _logService.Error($"Returning from LoginUser service result null", this);
                    }
                }
            } else {
                _logService.Error($"Error calling the LoginUser service {response.StatusCode}, {response.ExceptionDetail}", this);
                
            }
            return response.Result;
        }
    }
}