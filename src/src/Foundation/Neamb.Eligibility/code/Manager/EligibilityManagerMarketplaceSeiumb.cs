using System;
using System.Collections.Generic;
using log4net;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility;
using ILog = Neambc.Seiumb.Foundation.Sitecore.ILog;

namespace Neambc.Neamb.Foundation.Eligibility.Manager
{
    [Service(typeof(IEligibilityManagerMarketplaceSeiumb))]
    public class EligibilityManagerMarketplaceSeiumb : IEligibilityManagerMarketplaceSeiumb

    {
        private readonly ICacheManagerSeiumb _cacheManager;
        private readonly string _cacheKeyGroup = "MarketplaceEligibility";
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly ILog _log;
        private readonly IProductRestManagerSeiumb _productRestManagerSeiumb;

        public EligibilityManagerMarketplaceSeiumb(ICacheManagerSeiumb cacheManager, IGlobalConfigurationManager globalConfigurationManager, ILog log, IProductRestManagerSeiumb productRestManagerSeiumb) {
            _cacheManager = cacheManager;
            _globalConfigurationManager = globalConfigurationManager;
            _log = log;
            _productRestManagerSeiumb = productRestManagerSeiumb;
        }
        public bool IsMemberEligible(int mdsid) {
            if (mdsid==0)
            {
                return false;
            }
            string key = $"{_cacheKeyGroup}:{mdsid}";
            var expiredAt = DateTime.Now.AddMinutes(_globalConfigurationManager.ExpirationMinutesCacheEligibilityMarketplace);
            var valueCache = _cacheManager.RetrieveFromCache<bool?>(key);
            if (valueCache != null)
            {
                _log.Debug($"Eligibility Marketplace retrieved from cache Seiumb {valueCache.Value}");
                return valueCache.Value;
            }
            else
            {
                bool resultEligibility= _productRestManagerSeiumb.GetEligibility(mdsid);
                _log.Debug($"Eligibility Marketplace retrieved from service Seiumb {resultEligibility}");
                _cacheManager.StoreInCache<bool?>(key,resultEligibility,expiredAt);
                return resultEligibility;
            }
        }
    }
}