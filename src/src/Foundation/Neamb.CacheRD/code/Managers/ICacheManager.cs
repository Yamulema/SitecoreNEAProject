using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.Cache.Managers
{
    public interface ICacheManager
    {
        bool ExistInCache(string key);
        bool Remove(string key);
        T RetrieveFromCache<T>(string key);
        void StoreInCache<T>(string key, T element);
        void StoreInCache<T>(string key, T element, DateTime expiresAt);
        List<string> SearchPatternInCache(string pattern);
        bool RemoveFolder(string path);
        IDisposable AcquireLock(string key);
    }
}