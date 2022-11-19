using System;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Rakuten.Manager;

namespace Neambc.Neamb.Feature.Rakuten.Repositories
{
    [Service(typeof(IStoreImportRepository))]
    public class StoreImportRepository: IStoreImportRepository
    {
        private readonly IStoreImportManager _storeImportManager;
        private readonly ICustomLocker _customLocker;
        private readonly IRakutenLog _rakutenLog;
        public StoreImportRepository(IStoreImportManager storeImportManager, ICustomLocker customLocker, IRakutenLog rakutenLog) {
            _storeImportManager = storeImportManager;
            _customLocker = customLocker;
            _rakutenLog = rakutenLog;
        }

        public void ImportStores() {
            try {
                using (_customLocker.Lock())
                {
                    _storeImportManager.ExecuteImportStoreSitecore();
                }
            } catch (Exception e) {
                _rakutenLog.Error("Error Import Store Process",e);
            }
            
        }
    }
}