using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Feature.Account.Models;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neambc.Seiumb.Feature.Account.Manager
{
    [Service(typeof(ILocalDivisionManager))]
    public class LocalDivisionManager: ILocalDivisionManager
    {
        private readonly string _cacheKeyGroup = "LocalDivision";
        private readonly ICacheManager _cacheManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly ISeiumbProfileManager _seiumbProfileManager;

        public LocalDivisionManager(ICacheManager cacheManager, IGlobalConfigurationManager globalConfigurationManager, ISeiumbProfileManager seiumbProfileManager)
        {
            _cacheManager = cacheManager;
            _globalConfigurationManager = globalConfigurationManager;
            _seiumbProfileManager = seiumbProfileManager;
        }

        /// <summary>
        /// Execute query in the database to return the local code items
        /// </summary>
        /// <returns>Item list</returns>
        private List<LocalCodeDto> GetLocalCodesGlobalFromDatabase() {
            List<LocalCodeDto> localCodeFromDatabase = new List<LocalCodeDto>();
            Item[] allItemsGlobal = Sitecore.Context.Database.SelectItems("fast://sitecore/content/NEAMBC/Global/Locals/SEIUMB Local Code Folder//*");
            allItemsGlobal.ToList().ForEach(item=> localCodeFromDatabase.Add(new LocalCodeDto{ Code = item[Templates.LocalCode.Fields.Id], Division = item[Templates.LocalCode.Fields.LocalDivision]}));
            return localCodeFromDatabase;
        }

        /// <summary>
        /// Get from the redis or the database the local code items
        /// </summary>
        /// <returns></returns>
        public IList<LocalCodeDto> GetLocalCodesGlobal() {
            var key = $"{_cacheKeyGroup}";
            var expiredAt = DateTime.Now.AddHours(_globalConfigurationManager.ExpirationCacheLocalCodesHours);
            var valueCache = _cacheManager.RetrieveFromCache<IList<LocalCodeDto>>(key);
            if (valueCache != null) return valueCache;
            var result = GetLocalCodesGlobalFromDatabase();
            if (result != null && result.Count > 0) _cacheManager.StoreInCache<IList<LocalCodeDto>>(key, result, expiredAt);
            return result;
        }

        /// <summary>
        /// Verify the existence of the localCode and division from the local code list retrieved from the database
        /// </summary>
        /// <param name="localCodeFromSitecore">Local code items</param>
        /// <param name="divisionToCompare">Division</param>
        /// <returns></returns>
        public bool ExistLocalCodeUser(IList<LocalCodeDto> localCodeFromSitecore,string divisionToCompare) {
            var profile = _seiumbProfileManager.GetProfile();
            var localCodeUser = localCodeFromSitecore.FirstOrDefault(item =>
                item.Code == profile.SeiuLocalNumber && item.Division == divisionToCompare);
            return localCodeUser != null;
        }
    }
}