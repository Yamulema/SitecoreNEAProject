using System;
using System.Collections.Generic;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;


namespace Neambc.Neamb.Foundation.Eligibility.Manager
{
    [Service(typeof(IEligibilityOmni))]
    public class EligibilityOmni: IEligibilityOmni
    {
        private readonly IOracleDatabase _oracleDatabase;
        private readonly ICacheManager _cacheManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly string _cacheKeyGroup = "OmniEligibility";
        public EligibilityOmni(IOracleDatabase oracleDatabase, ICacheManager cacheManager, IGlobalConfigurationManager globalConfigurationManager) {
            _oracleDatabase = oracleDatabase;
            _cacheManager = cacheManager;
            _globalConfigurationManager = globalConfigurationManager;
        }

        /// <summary>
        /// Get the query result to the Omni view in Oracle from caching or database
        /// </summary>
        /// <param name="mdsid">User identifier</param>
        /// <param name="productCode">Product code</param>
        /// <returns>Omni view results</returns>
        public IList<ViewOmni> CheckEligibility(string mdsid, string productCode) {
            if (string.IsNullOrEmpty(mdsid) || string.IsNullOrEmpty(productCode)) {
                return null;
            }
            string key = $"{_cacheKeyGroup}:{mdsid}:{productCode}";
            var expiredAt = DateTime.Now.AddHours(_globalConfigurationManager.ExpirationCacheOmniEligibility);
            var valueCache =_cacheManager.RetrieveFromCache<IList<ViewOmni>>(key);
            if (valueCache != null) {
                return valueCache;
            } else {
                var result = _oracleDatabase.ExecuteViewOmni(mdsid, productCode);
                if (result != null && result.Count>0)
                {
                    _cacheManager.StoreInCache<IList<ViewOmni>>(key,result, expiredAt);
                }
                return result;
            }
        }
    }
}