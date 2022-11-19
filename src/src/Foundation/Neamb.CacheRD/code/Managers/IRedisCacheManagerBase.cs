using System;
using System.Collections.Generic;


namespace Neambc.Neamb.Foundation.Cache.Managers
{
    public interface IRedisCacheManagerBase
    {
        bool ExistInCache(string key, string environment);
        bool Remove(string key, string environment);
        T RetrieveFromCache<T>(string key, string environment);
        void StoreInCache<T>(string key, T element, string environment);
        void StoreInCache<T>(string key, T element, DateTime expiresAt, string environment);
        List<string> SearchPatternInCache(string pattern, string environment);
        bool RemoveFolder(string path, string environment);
        IDisposable AcquireLock(string key, string environment);
    }
}