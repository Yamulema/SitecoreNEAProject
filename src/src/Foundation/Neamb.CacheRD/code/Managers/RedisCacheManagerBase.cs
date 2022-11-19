using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Neambc.Neamb.Foundation.Cache.Repositories;
using Neambc.Neamb.Foundation.DependencyInjection;
using ServiceStack.Redis;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.Cache.Managers
{
    [Service(typeof(IRedisCacheManagerBase))]
    public class RedisCacheManagerBase: IRedisCacheManagerBase
    {
        private readonly PooledRedisClientManager _pooledClientManager;
        
        public RedisCacheManagerBase(ICacheConnectionConfigRepo cacheConfigRepo)
        {
            _pooledClientManager = new PooledRedisClientManager(cacheConfigRepo.CacheConnectionConfiguration.PooledConnection);
            
        }

        /// <summary>
        /// Returns true when it finds the corresponding key in Redis
        /// </summary>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <returns>true if key is found, otherwise false</returns>
        public bool ExistInCache(string key, string environment)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");

            try
            {
                using (var client = _pooledClientManager.GetClient())
                {
                    var cacheKey = $"{environment}:{key}";
                    return client.ContainsKey(cacheKey);
                }
            }
            catch (TimeoutException ex)
            {
                Log.Error("Time out during exist in cache, returning false", ex, this);
                return false;
            }
        }

        /// <summary>
        /// Removes stored value from redis with the key
        /// </summary>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <returns>true if manage to remove value, otherwise false</returns>
        public bool Remove(string key, string environment)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");

            try
            {
                Log.Info($"Removinged key {key}",this);
                using (var client = _pooledClientManager.GetClient())
                {
                    var cacheKey = $"{environment}:{key}";
                    return client.Remove(cacheKey);
                }
            }
            catch (TimeoutException ex)
            {
                Log.Error("Time out during remove, returning false", ex, this);
                return false;
            }
        }

        /// <summary>
        /// Retrives typed object from cache
        /// </summary>
        /// <typeparam name="T">Object type to be retrieved</typeparam>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <returns>returns typed object if found, otherwise returns defualt typed object</returns>
        public T RetrieveFromCache<T>(string key, string environment)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");

            try
            {
                using (var client = _pooledClientManager.GetClient())
                {
                    var cacheKey = $"{environment}:{key}";
                    return client.Get<T>(cacheKey);
                }
            }
            catch (TimeoutException ex)
            {
                Log.Error("Time out during retrieval from cache, returning default object", ex, this);
                return default(T);
            }
        }

        /// <summary>
        /// Stores typed object in cache for indefinity amount of time
        /// </summary>
        /// <typeparam name="T">Object type to be stored</typeparam>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <param name="element">object to be stored</param>
        public void StoreInCache<T>(string key, T element, string environment)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");
            Assert.ArgumentNotNull(element, "element");

            try
            {
                using (var client = _pooledClientManager.GetClient())
                {
                    var cacheKey = $"{environment}:{key}";
                    client.Set<T>(cacheKey, element);
                }
            }
            catch (TimeoutException ex)
            {
                Log.Error("Time out during storage in cache", ex, this);
            }
        }

        /// <summary>
        /// Stores typed object in cache for defined amount of time
        /// </summary>
        /// <typeparam name="T">Object type to be stored</typeparam>
        /// <param name="key">unique identifier of value stored in redis</param>
        /// <param name="element">object to be stored</param>
        /// <param name="expiresAt">timespan with time of storage</param>
        public void StoreInCache<T>(string key, T element, DateTime expiresAt, string environment)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");
            Assert.ArgumentNotNull(element, "element");
            Assert.ArgumentNotNull(expiresAt, "ExpiresAt");

            try
            {
                using (var client = _pooledClientManager.GetClient())
                {
                    var cacheKey = $"{environment}:{key}";
                    client.Set<T>(cacheKey, element, expiresAt);
                }
            }
            catch (TimeoutException ex)
            {
                Log.Error("Time out during storage in cache with expiration", ex, this);
            }
        }

        public List<string> SearchPatternInCache(string pattern, string environment)
        {
            var result = new List<string>();
            try
            {
                using (var client = _pooledClientManager.GetClient())
                {
                    var fullPattern = $"{environment}:{pattern}";
                    result = client.SearchKeys(fullPattern)
                        .Select(x => x.Replace($"{environment}:", string.Empty))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Time out during storage in cache with expiration", ex, this);
            }
            return result;
        }
        public bool RemoveFolder(string path, string environment) {
            Log.Warn($"Redis remove batch start for path: {path}", this);
            var result = true;
            var keys = SearchPatternInCache(path,environment);
            Log.Warn($"Total keys: {keys.Count}", this);
            foreach (var key in keys)
            {
                var success = Remove(key,environment);
                result &= success; //Sets result to false if any key fails to be removed.
                if (success) {
                    Log.Warn($"Removed key {key} from {path}", this);
                } else {
                    Log.Warn($"Unable to remove key {key} from {path}", this);
                }
            }
            return result;
        }

        /// <summary>
        /// Acquires the lock for Redis
        /// </summary>
        /// <param name="key">unique identifier for lock</param>
        public IDisposable AcquireLock(string key, string environment)
        {
            var waitFor = TimeSpan.FromSeconds(2);
            var cacheKey = $"{environment}:Lock:{key}";
            try
            {
                using (var client = _pooledClientManager.GetClient())
                {
                    Log.Debug("Redis Cache Manager - AcquireLock: Got the lock successfully " + cacheKey, this);
                    return client.AcquireLock(cacheKey, waitFor);
                }       
            }
            catch (Exception ex)
            {
                Log.Error("Redis Cache Manager - AcquireLock: Got the lock unsuccessfully " + cacheKey, ex, this);
                return default(IDisposable);
            }
        }
    }
}