using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.CancelResetToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.CancelResetToken
{
    [Service(typeof(ICancelResetTokenService))]
    public class CancelResetTokenService : ICancelResetTokenService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public CancelResetTokenService(IAccessTokenService accessTokenService, IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService,
            ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }


        /// <summary>
        /// Cancel reset token
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="unionId">Seiumb/Neamb</param>
        /// <returns></returns>
        /// <returns>Response of the AWS WebService</returns>
        public bool CancelResetToken(string username, int unionId)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            
            if (string.IsNullOrEmpty(username) ||
                unionId < 0)
            {
                throw new ArgumentException($"Parameters for CancelResetToken - CancelResetToken are incorrect");
            }
            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlCancelResetToken,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new CancelResetTokenRequest
                {
                    Username = username,
                    UnionId = unionId
                },
                IsBasicAuthentication = false
            };
            var response = _mbcRestfulService.Post<CancelResetTokenResponse>(restRequestDto);
            if (response != null) {
                if (response.Success)
                {
                    if (response.Result != null && !response.Result.Success && response.Result.Error != null)
                    {
                        _responseHandler.LogErrorResponse(response.Result.Error, "CancelResetTokenService", _logService);
                    } 
                }
                else
                {
                    _logService.Error($"Post Error in Cancel Reset Token Service: {response.StatusCode}, {response.ExceptionDetail}", this);
                }

                
            } else {
                _logService.Error($"Response is null in Cancel Reset Token Service", this);
                
            }
            return (response != null && response.Success && response.Result!=null && response.Result.Success && response.Result.Data!=null && response.Result.Data.Canceled);
        }
    }
}