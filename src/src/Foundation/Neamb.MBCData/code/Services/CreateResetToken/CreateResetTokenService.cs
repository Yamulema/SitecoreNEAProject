using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.CreateResetToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.CreateResetToken
{
    [Service(typeof(ICreateResetTokenService))]
    public class CreateResetTokenService : ICreateResetTokenService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public CreateResetTokenService(IAccessTokenService accessTokenService, IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService,
            ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public CreateResetTokenResponse CreateResetToken(string username, int unionId)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            
            if (string.IsNullOrEmpty(username) ||
                unionId < 0)
            {
                throw new ArgumentException("Parameters for CreateResetToken - CreateResetToken are incorrect");
            }
            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlCreateResetToken,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new CreateResetTokenRequest
                {
                    Username = username,
                    UnionId = unionId
                },
                IsBasicAuthentication = false
            };
            var response = _mbcRestfulService.Post<CreateResetTokenResponse>(restRequestDto);
            if (response != null) {
                if (response.Success)
                {
                    if (response.Result != null && !response.Result.Success && response.Result.Error != null)
                    {
                        _responseHandler.LogErrorResponse(response.Result.Error, "CreateResetTokenService", _logService);
                    } 
                }
                else
                {
                    _logService.Error($"Post Error in Create Reset Token Service: {response.StatusCode}, {response.ExceptionDetail}", this);
                }

                return response.Result;
            } else {
                _logService.Error("Response is null in Create Reset Token Service", this);
                return null;
            }

            
        }
    }
}