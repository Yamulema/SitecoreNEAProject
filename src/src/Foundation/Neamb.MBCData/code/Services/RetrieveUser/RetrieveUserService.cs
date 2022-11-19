using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.RetrieveUser;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.RetrieveUser
{
    [Service(typeof(IRetrieveUserService))]
    public class RetrieveUserService : IRetrieveUserService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public RetrieveUserService(IAccessTokenService accessTokenService, IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService,
            ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public RetrieveUserModel RetrieveUserData(int mdsid, int unionId)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken)) throw new ArgumentException("token invalid", "token");
            if (mdsid < 0 || unionId < 0) throw new ArgumentException($"Parameters for RetrieveUserService - invalid unionId (< 0)");

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlRetrieveUser,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new RetrieveUserRequest
                {
                    MdsId = mdsid,
                    UnionId = unionId
                },
                IsBasicAuthentication = false
            };

            _logService.Info("Starting calling RetrieveUserService - RetrieveUserData with params:", this);
            _logService.Info($"MdsId: {mdsid}", this);
            _logService.Info($"UnionId: {unionId}", this);

            var response = _mbcRestfulService.Post<RetrieveUserResponse>(restRequestDto);
            if (response.Success) {
                if (response.Result?.Data != null && response.Result.Success)
                    return response.Result.Data;

                _responseHandler.LogErrorResponse(response.Result?.Error, "RetrieveUser", _logService);

            } else {
                _logService.Error($"Post Error to RetrieveUser: {response.StatusCode}, {response.ExceptionDetail}", this);
            }

            return response.Result?.Data;
        }
    }
}