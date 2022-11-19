using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.ApiSecurity;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Sitecore.Shell.Applications.PageScreenshots;

namespace Neambc.Neamb.Foundation.MBCData.Services.SecurityManagement
{
    [Service(typeof(ISecurityService))]
    public class SecurityService : ISecurityService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;

        public SecurityService(IAccessTokenService accessTokenService,
            IGlobalConfigurationManager config,
            IMBCRestfulService mbcRestfulService)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
        }

        public string AesEncrypt(string mdsId, Token tokenPartner)
        {
            if (string.IsNullOrEmpty(mdsId)) throw new ArgumentNullException("Invalid mdsid is empty","mdsId");

            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
                throw new ArgumentException("token invalid", "token");

            var apiSecurityRequest = new ApiSecurityRequest {
                KeySize = _config.AESKeySize,
                PasswordIterations = _config.AESPasswordInteractions,
                PlainText = mdsId.TrimStart(new Char[] {
                    '0'
                })
            };

            switch (tokenPartner)
            {
                case Token.Afinium:
                    apiSecurityRequest.Password = _config.AfiniumAESPassword;
                    apiSecurityRequest.Salt = _config.AfiniumAESSalt;
                    break;
                case Token.Mercer:
                    apiSecurityRequest.Password = _config.MercerAESPassword;
                    apiSecurityRequest.Salt = _config.MercerAESSalt;
                    break;
                default:
                    throw new ApplicationException($"Error with AES encryption");
            }

            var restRequestDto = new RestRequestDto()
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlAESEncrypt,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = apiSecurityRequest,
                IsBasicAuthentication = false
            };
            
            var response = _mbcRestfulService.Post<ApiSecurityResponse>(restRequestDto);

            return response.Success ?
                response.Result.Data.EncryptedText :
                string.Empty;
        }
    }
}