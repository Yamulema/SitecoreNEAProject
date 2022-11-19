using System;
using System.Collections.Generic;

using Neambc.Neamb.Foundation.Cache.Repositories;
using Neambc.Neamb.Foundation.DependencyInjection;


namespace Neambc.Neamb.Foundation.Cache.Managers
{
    [Service(typeof(ICacheManagerSeiumb))]
    public class RedisCacheSeiumbManager : ICacheManagerSeiumb
    {
        private readonly string _environment;
        private readonly IRedisCacheManagerBase _redisCacheManagerBase;

        public RedisCacheSeiumbManager(ICacheSeiumbConfigRepo cacheConfigRepo, IRedisCacheManagerBase redisCacheManagerBase)
        {
            _environment = cacheConfigRepo.CacheSeiumbConfiguration.EnvironmentCacheKey;
            _redisCacheManagerBase = redisCacheManagerBase;
        }

        /// <summary>
        /// Returns true when it finds the corresponding key in Redis
        /// </summary>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <returns>true if key is found, otherwise false</returns>
        public bool ExistInCache(string key) {
            return _redisCacheManagerBase.ExistInCache(key, _environment);
        }

        /// <summary>
        /// Removes stored value from redis with the key
        /// </summary>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <returns>true if manage to remove value, otherwise false</returns>
        public bool Remove(string key) {
            return _redisCacheManagerBase.Remove(key, _environment);
        }

        /// <summary>
        /// Retrives typed object from cache
        /// </summary>
        /// <typeparam name="T">Object type to be retrieved</typeparam>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <returns>returns typed object if found, otherwise returns defualt typed object</returns>
        public T RetrieveFromCache<T>(string key) {
            return _redisCacheManagerBase.RetrieveFromCache<T>(key, _environment);
        }

        /// <summary>
        /// Stores typed object in cache for indefinity amount of time
        /// </summary>
        /// <typeparam name="T">Object type to be stored</typeparam>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <param name="element">object to be stored</param>
        public void StoreInCache<T>(string key, T element)
        {
            _redisCacheManagerBase.StoreInCache(key, element, _environment);
        }

        /// <summary>
        /// Stores typed object in cache for defined amount of time
        /// </summary>
        /// <typeparam name="T">Object type to be stored</typeparam>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <param name="element">object to be stored</param>
        /// <param name="expiresAt">timespan with time of storage</param>
        public void StoreInCache<T>(string key, T element, DateTime expiresAt)
        {
            _redisCacheManagerBase.StoreInCache(key,element,expiresAt,_environment);
        }

        public List<string> SearchPatternInCache(string pattern) {
            return _redisCacheManagerBase.SearchPatternInCache(pattern, _environment);
        }
        public bool RemoveFolder(string path) {
            return _redisCacheManagerBase.RemoveFolder(path, _environment);
        }

        /// <summary>
        /// Acquires the lock for Redis
        /// </summary>
        /// <param name="key">unique identifier for lock</param>
        public IDisposable AcquireLock(string key) {
            return _redisCacheManagerBase.AcquireLock(key, _environment);
        }
    }
}