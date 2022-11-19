using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.IceDollars;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.IceDollars
{
    [Service(typeof(IIceDollarsService))]
    public class IceDollarsService : IIceDollarsService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public IceDollarsService(IAccessTokenService accessTokenService, IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler) {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public IceDollarsResponse GetBalance(int mdsId)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken)) throw new ArgumentException("token invalid", "token");
            if (mdsId < 0) throw new ArgumentException("invalid mdsid (< 0)", "mdsId");

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlICEGetBalance,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new IceDollarsRequest
                {
                    MdsId = mdsId
                },
                IsBasicAuthentication = false
            };

            var response = _mbcRestfulService.Post<IceDollarsResponse>(restRequestDto);

            if (response.Success)
            {
                if (response.Result != null && !response.Result.Success && response.Result.Error != null)
                {
                    _responseHandler.LogErrorResponse(response.Result.Error, "IceDollarsService", _logService);
                }
            }
            else
            {
                _logService.Error($"IceDollarsService - GetBalance Post Error: {response.StatusCode}, {response.ExceptionDetail}", this);
            }

            return response.Result;
        }
    }
}