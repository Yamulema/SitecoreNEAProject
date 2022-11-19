using System.Collections.Generic;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;

namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    public interface IStoreImportManager {
        void ExecuteImportStoreSitecore();
        List<StoreReport> GetAllStoresInSitecore();
        List<StoreReport> EnableStoresFromList(List<string> storesToEnable);
    }
}