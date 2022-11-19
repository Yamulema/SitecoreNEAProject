using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.Neamb.Foundation.MBCData.Model.ValidateEmailDomain;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;

namespace Neambc.Neamb.Foundation.MBCData.Services.ValidateEmailDomain
{
    [Service(typeof(IValidateEmailDomain))]
    public class ValidateEmailDomainService : IValidateEmailDomain
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public ValidateEmailDomainService(IAccessTokenService accessTokenService,IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService=mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public ValidateEmailDomainResponse ValidateEmailDomainStatus(string Email)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            if (string.IsNullOrEmpty(Email))
            {
                throw new ArgumentException($"email is not found");
            }

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlValidateEmailDomain,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new ValidateEmailDomainRequest
                {
                    username = Email
                },
                IsBasicAuthentication = false
            };

            var response = _mbcRestfulService.Post<ValidateEmailDomainResponse>(restRequestDto);
          
            if (response.Success)
            {
                if (response.Result != null && !response.Result.Success && response.Result.Error != null)
                {
                    _responseHandler.LogErrorResponse(response.Result?.Error, "ValidateEmailDomain", _logService);
                }
            }
            else
            {
                _logService.Error($"ValidateEmailDomain Post Error: {response.StatusCode}, {response.ExceptionDetail}", this);
            }

            return response.Result;
        }
    }
}
