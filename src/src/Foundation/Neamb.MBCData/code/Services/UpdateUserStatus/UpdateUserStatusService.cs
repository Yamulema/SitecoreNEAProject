using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.UpdateUserStatus;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.UpdateUserStatus
{
    [Service(typeof(IUpdateUserStatusService))]
    public class UpdateUserStatusService : IUpdateUserStatusService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public UpdateUserStatusService(IAccessTokenService accessTokenService,
            IGlobalConfigurationManager config,
            IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler) {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public bool UpdateUserStatus(string username, string statusCode, int unionId) {
            if (unionId < 0) throw new ArgumentException("Parameters for UpdateUserStatusService - UpdateUserStatus invalid - invalid unionId (< 0)", "unionId");
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Parameters for UpdateUserStatusService - UpdateUserStatus invalid - username is empty", "username");
            if (string.IsNullOrEmpty(statusCode)) throw new ArgumentException("Parameters for UpdateUserStatusService -UpdateUserStatus invalid - statusCode is empty", "statusCode");

            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken)) throw new ArgumentException("token invalid", "token");

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlUpdateUserStatus,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new UpdateUserStatusRequest
                {
                    Username = username,
                    UnionId = unionId,
                    StatusCode = statusCode
                },
                IsBasicAuthentication = false
            };

            var response = _mbcRestfulService.Post<UpdateUserStatusResponse>(restRequestDto);
            if (response.Success) {
                if (response.Result?.Data != null && response.Result.Data.Updated)
                    return true;
                _responseHandler.LogErrorResponse(response.Result?.Error, "UpdateUserStatus", _logService);
            }
            else
            {
                _logService.Error($"Post Error to UpdateUserStatusService: {response.StatusCode}, {response.ExceptionDetail}", this);
            }
            return false;
        }
    }
}