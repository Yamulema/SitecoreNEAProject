using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.ValidateResetToken;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.ValidateResetToken
{
    [Service(typeof(IValidateResetTokenService))]
    public class ValidateResetTokenService : IValidateResetTokenService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public ValidateResetTokenService(IAccessTokenService accessTokenService,
            IGlobalConfigurationManager config,
            IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public bool ValidateResetToken(string username, string resetToken, int unionId) {
            if (unionId < 0) throw new ArgumentException("Parameters for ValidateResetTokenService - ValidateResetToken invalid - invalid unionId (< 0)", "unionId");
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Parameters for ValidateResetTokenService - ValidateResetToken invalid - username is empty", "username");
            if (string.IsNullOrEmpty(resetToken)) throw new ArgumentException("Parameters for ValidateResetTokenService -ValidateResetToken invalid - resetToken is empty", "resetToken");

            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken)) throw new ArgumentException("token invalid", "token");

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlValidateResetToken,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new ValidateResetTokenRequest
                {
                    Username = username,
                    UnionId = unionId,
                    ResetToken = resetToken
                },
                IsBasicAuthentication = false
            };

            
            var response = _mbcRestfulService.Post<ValidateResetTokenResponse>(restRequestDto);
            if (response.Success)
            {
                if (response.Result?.Data != null && response.Result.Data.Valid)
                    return true;

                _responseHandler.LogErrorResponse(response.Result?.Error, "ValidateResetToken", _logService);
            }
            else
            {
                _logService.Error($"Post Error to ValidateResetTokenService: {response.StatusCode}, {response.ExceptionDetail}", this);
            }
            return false;
        }
    }
}