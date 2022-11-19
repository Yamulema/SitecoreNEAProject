using System;
using System.Collections.Generic;
using log4net;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using ILog = Neambc.Seiumb.Foundation.Sitecore.ILog;

namespace Neambc.Neamb.Foundation.Eligibility.Manager
{
    [Service(typeof(IEligibilityManagerMarketplace))]
    public class EligibilityManagerMarketplace: IEligibilityManagerMarketplace
    {
        private readonly IEligibilityManager _eligibilityManager;
        private readonly ICacheManager _cacheManager;
        private readonly string _cacheKeyGroup = "MarketplaceEligibility";
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly ILog _log;

        public EligibilityManagerMarketplace(IEligibilityManager eligibilityManager, ICacheManager cacheManager, IGlobalConfigurationManager globalConfigurationManager, ILog log) {
            _eligibilityManager = eligibilityManager;
            _cacheManager = cacheManager;
            _globalConfigurationManager = globalConfigurationManager;
            _log = log;
        }
        public EligibilityResultEnum IsMemberEligible(string mdsid, string productcode) {
            if (string.IsNullOrEmpty(mdsid) || string.IsNullOrEmpty(productcode))
            {
                return EligibilityResultEnum.NotEligible;
            }
            string key = $"{_cacheKeyGroup}:{mdsid}:{productcode}";
            var expiredAt = DateTime.Now.AddMinutes(_globalConfigurationManager.ExpirationMinutesCacheEligibilityMarketplace);
            var valueCache = _cacheManager.RetrieveFromCache<EligibilityResultEnum?>(key);
            if (valueCache != null)
            {
                _log.Debug($"Eligibility Marketplace retrieved from cache {valueCache.Value}");
                return valueCache.Value;
            }
            else
            {
                var resultEligibility= _eligibilityManager.IsMemberEligible(mdsid, productcode);
                _log.Debug($"Eligibility Marketplace retrieved from service {resultEligibility}");
                _cacheManager.StoreInCache<EligibilityResultEnum?>(key,resultEligibility,expiredAt);
                return resultEligibility;
            }
        }
    }
}