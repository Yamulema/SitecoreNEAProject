using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.ProductEligibility;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;

namespace Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility
{
    [Service(typeof(IProductRestBaseRepository))]
    public class ProductRestBaseRepository : IProductRestBaseRepository
    {
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly IGlobalConfigurationManager _config;
        
        public ProductRestBaseRepository(IMBCRestfulService mbcRestfulService, IGlobalConfigurationManager config) {
            _mbcRestfulService = mbcRestfulService;
            _config = config;
        }

        /// <summary>
        /// Get eligibility for a mdsid and productCode
        /// </summary>
        /// <param name="token">Access token</param>
        /// <param name="productRestRequest"></param>
        /// <param name="url">Url to execute the action</param>
        /// <returns></returns>
        public bool GetEligibility(TokenModel token, object productRestRequest, string url)
        {
            if (token == null || string.IsNullOrEmpty(token.AccessToken) || productRestRequest == null || string.IsNullOrEmpty(url))
                throw new ArgumentException($"Parameters for ProductRestBaseRepository - GetEligibility are incorrect");

            var restRequestDto = new RestRequestDto {
                Server = _config.RestUrl,
                Action = url,
                ParseJson = true,
                Token = token.AccessToken,
                Body = productRestRequest,
                IsBasicAuthentication = false
            };

            var response = _mbcRestfulService.Post<ProductEligibilityResponse>(restRequestDto);
            return response.Success && response.Result?.Data != null && response.Result.Data.Eligible;
        }
    }
}