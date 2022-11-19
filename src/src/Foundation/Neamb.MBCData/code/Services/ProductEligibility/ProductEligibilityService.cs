using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.ProductEligibility;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;

namespace Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility
{
    [Service(typeof(IProductEligibilityService))]
    public class ProductEligibilityService : IProductEligibilityService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IProductRestBaseRepository _productRestBaseRepository;

        public ProductEligibilityService(IAccessTokenService accessTokenService, IGlobalConfigurationManager config, IProductRestBaseRepository productRestBaseRepository) {
            _accessTokenService = accessTokenService;
            _config = config;
            _productRestBaseRepository = productRestBaseRepository;
        }

        /// <summary>
        /// Get eligibility for a mdsid and productCode
        /// </summary>
        /// <param name="mdsId">Mdsid user</param>
        /// <param name="productCode">Product code</param>
        /// <returns></returns>
        public bool GetEligibility(int mdsId, string productCode) {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            if (mdsId < 0 || string.IsNullOrEmpty(productCode))
            {
                throw new ArgumentException($"Parameters for ProductEligibilityService - GetEligibility are incorrect");
            }
            var request = new ProductEligibilityRequest
            {
                MdsId = mdsId,
                ProductCode = productCode
            };

            return _productRestBaseRepository.GetEligibility(token.Data, request, _config.RestUrlProductEligibility);
        }
    }
}