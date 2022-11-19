using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.SearchUserName
{
    [Service(typeof(ISearchUserNameService))]
    public class SearchUserNameService : ISearchUserNameService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public SearchUserNameService(IAccessTokenService accessTokenService, IGlobalConfigurationManager config, 
            IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public SearchUserNameResponse SearchUserName(string username)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken)) throw new ArgumentException("token invalid", "token");
            if (string.IsNullOrEmpty(username)) throw new ArgumentException($"Parameters for SearchUserName invalid - username is empty", "username");

            var bodyRequest = new SearchUserNameRequest
            {
               Username = username
            };

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlSearchUserName,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = bodyRequest,
                IsBasicAuthentication = false
            };

            var response = _mbcRestfulService.Post<SearchUserNameResponse>(restRequestDto);
            if (response.Success)
            {
                if (response.Result?.Data != null && response.Result.Success)
                    return response.Result;

                if (response.Result != null) {
                    response.Result.ErrorCode = (SearchUsernameErrorCodes) response.Result.Error.Code;
                    _responseHandler.LogErrorResponse(response.Result?.Error, "SearchUserNameService", _logService);
                }
            }
            else
            {
                _logService.Error($"Post Error to SearchUserNameService: {response.StatusCode}, {response.ExceptionDetail}", this);
            }
            return response.Result;
        }
    }
}
