using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.ProductEligibility;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;

namespace Neambc.Neamb.Foundation.MBCData.Repositories.Rest
{
    [Service(typeof(ISecurityRestBaseRepository))]
    public class SecurityRestBaseRepository : ISecurityRestBaseRepository
    {
        private readonly IRestRepository _restRepository;
        private readonly IGlobalConfigurationManager _config;
        private readonly IAccessTokenService _accessTokenService;

        public SecurityRestBaseRepository(IRestRepository restRepository, IGlobalConfigurationManager config, IAccessTokenService accessTokenService)
        {
            _restRepository = restRepository;
            _config = config;
            _accessTokenService = accessTokenService;
        }

        /// <summary>
        /// Execute encryption
        /// </summary>
        /// <param name="token">Token for authentication in rest services</param>
        /// <param name="securityRequest">Data to be sent to the rest request</param>
        /// <param name="url">Rest url</param>
        /// <returns>Data encrypted</returns>
        public string ExecuteEncryption(TokenModel token,object securityRequest, string url)
        {
            if (securityRequest == null || string.IsNullOrEmpty(url))
                throw new ArgumentException($"Parameters for SecurityRestBaseRepository - ExecuteEncryption are incorrect");

            var restRequestDto = new RestRequestDto()
            {
                Server = _config.RestUrl,
                Action = url,
                ParseJson = true,
                Token = token.AccessToken,
                Body = securityRequest,
                IsBasicAuthentication = false
            };

            var response = _restRepository.Post<ApiSecurityResponse>(restRequestDto);
            if (response != null && response.Success && response.Result?.Data != null)
            {
                return response.Result.Data.EncryptedText;
            }
            //Verify if there is an authentication error retry to get the token
            if (response?.Result?.Error != null && response.Result.Error.Code == 401)
            {
                var accessTokenRetry = _accessTokenService.GetAccessTokenFromServer();
                if (!accessTokenRetry.Success) throw new Exception("GetAccessTokenFromServer retry in ProductRestBaseRepository has errors");

                var newToken = (TokenResponse)accessTokenRetry;
                restRequestDto.Token = newToken.Data.AccessToken;
                response = _restRepository.Post<ApiSecurityResponse>(restRequestDto);

                if (response != null && response.Success && response.Result?.Data != null)
                    return response.Result.Data.EncryptedText;
                else {
                    throw new ApplicationException($"Error with encryption process");
                }
            }
            else {
                throw new ApplicationException($"Error with encryption process");
            }
        }
    }
}