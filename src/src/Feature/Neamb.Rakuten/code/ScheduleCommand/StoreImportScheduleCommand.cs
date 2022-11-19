using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Rakuten.Repositories;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Tasks;

namespace Neambc.Neamb.Feature.Rakuten.ScheduleCommand
{
    public class StoreImportScheduleCommand
    {
        public void Execute(Item[] item, CommandItem commandItem, ScheduleItem scheduleItem) {
            var storeImportRepository =
                (IStoreImportRepository)ServiceLocator.ServiceProvider.GetService(typeof(IStoreImportRepository));
            storeImportRepository.ImportStores();
        }
    }
}