using System;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using RestSharp;

namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    [Service(typeof(IStoreProcessChecker))]
    public class StoreProcessChecker: IStoreProcessChecker
    {
        private readonly ICacheManager _cacheManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly string _cacheKeyGroup = "RakutenStore:Etag";
        public StoreProcessChecker(ICacheManager cacheManager, IGlobalConfigurationManager globalConfigurationManager) {
            _cacheManager = cacheManager;
            _globalConfigurationManager = globalConfigurationManager;
        }

        public string GetEtag()
        {

            return _cacheManager.RetrieveFromCache<string>(_cacheKeyGroup);            
        }

        /// <summary>
        /// Verify if the etag changed compared with the Redis values so in that case return true
        /// </summary>
        /// <param name="etagFromApi">Rest api header parameter</param>
        /// <returns></returns>
        public bool CanContinueImportProcess(Parameter etagFromApi) {
            
            var etagfromCache= _cacheManager.RetrieveFromCache<string>(_cacheKeyGroup);
            return (string.IsNullOrEmpty(etagfromCache) || etagFromApi == null || !etagfromCache.Equals(etagFromApi.Value));
        }

        /// <summary>
        /// Save in Redis the etag value
        /// </summary>
        /// <param name="etagFromApi">Rest api header parameter</param>
        public void SaveEtagCache(Parameter etagFromApi) {
            if (etagFromApi != null) {
                var expirationEtag = _globalConfigurationManager.ExpirationRedisEtag;
                _cacheManager.StoreInCache(_cacheKeyGroup, etagFromApi.Value, DateTime.Now.AddHours(expirationEtag));
            }
        }
    }
}