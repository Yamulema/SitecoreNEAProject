using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.ProductEligibility;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;

namespace Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility
{
    [Service(typeof(IProductRestManagerSeiumb))]
    public class ProductRestManagerSeiumb : IProductRestManagerSeiumb
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IProductRestBaseRepository _productRestManagerBase;

        public ProductRestManagerSeiumb(IAccessTokenService accessTokenService, IGlobalConfigurationManager config, IProductRestBaseRepository productRestManagerBase) {
            _accessTokenService = accessTokenService;
            _config = config;
            _productRestManagerBase = productRestManagerBase;
        }

        /// <summary>
        /// Get eligibility for a mdsid and productCode
        /// </summary>
        /// <param name="mdsId">Mdsid user</param>
        /// <returns></returns>
        public bool GetEligibility(int mdsId) {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            if (mdsId < 0) {
                throw new ArgumentException($"Parameters for GetEligibility are incorrect");
            }
            var productEligibilityRestRequest = new ProductEligibilityBaseRequest
            {
                MdsId = mdsId
            };

            return _productRestManagerBase.GetEligibility(token.Data, productEligibilityRestRequest,_config.RestUrlProductEligibilitySeiumb);
        }

    }
}