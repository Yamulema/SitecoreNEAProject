using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.AuthenticatePassword;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.AuthenticatePassword
{
    [Service(typeof(IAuthenticatePasswordService))]
    public class AuthenticatePasswordService : IAuthenticatePasswordService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public AuthenticatePasswordService(IAccessTokenService accessTokenService,IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService=mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public AuthenticatePasswordResponse AuthenticatePasswordStatus(string username, string password, int unionId)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException($"Username is not found");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"Empty Password for user {username}");
            }
            if (unionId < 0) throw new ArgumentException($"invalid unionId (< 0)");

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlAuthenticatePassword,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new AuthenticatePasswordRequest
                {
                    username = username,
                    password = password,
                    unionId = unionId
                },
                IsBasicAuthentication = false
            };

            var response = _mbcRestfulService.Post<AuthenticatePasswordResponse>(restRequestDto);
          
            if (response.Success)
            {
                if (response.Result != null && !response.Result.Success && response.Result.Error != null) {
                    _responseHandler.LogErrorResponse(response.Result.Error, "AuthenticatePassword",_logService);
                }
            }
            else
            {
                _logService.Error($"AuthenticatePassword Post Error: {response.StatusCode}, {response.ExceptionDetail}", this);
            }

            return response.Result;
        }
    }
}
