using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(ISeminarRepository))]
    public class SeminarRepository: ISeminarRepository
    {
        private readonly IOracleDatabase _oracleManager;
        private readonly ICacheManager _cacheManager;
        private readonly ISessionManager _sessionManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        public SeminarRepository(IOracleDatabase oracleManager, ICacheManager cacheManager, IGlobalConfigurationManager globalConfigurationManager, ISessionManager sessionManager) {
            _oracleManager = oracleManager;
            _cacheManager = cacheManager;
            _globalConfigurationManager = globalConfigurationManager;
            _sessionManager = sessionManager;
        }
        public IReadOnlyList<ViewSeminar> GetSeminaries() {
            IReadOnlyList<ViewSeminar> response = null;
            string keySeminaries = ConstantsNeamb.RedisKeySeminaries;
            if (_cacheManager.ExistInCache(keySeminaries))
            {
                response = _cacheManager.RetrieveFromCache<IReadOnlyList<ViewSeminar>>(keySeminaries);
            } else {
                response = _oracleManager.ViewAllSeminar();
                if (response != null) {
                    _cacheManager.StoreInCache<IReadOnlyList<ViewSeminar>>(keySeminaries,
                        response,
                        DateTime.Now.AddHours(_globalConfigurationManager.ExpirationHoursSeminaries));
                }
            }

            return response;
        }
        public ViewSeminar GetSeminary(Item renderingItem) {
            ViewSeminar seminarFound = null;
            var seminaries = GetSeminaries();
            if (seminaries != null) {
                var seminaryId = GetSeminarId(renderingItem);
                seminarFound= seminaries.FirstOrDefault(item => item.SeminarId == seminaryId);
            }
            return seminarFound;
        }

        public string GetSeminarId(Item renderingItem)
        {
            //Try to get from session that was saved from the url
            var seminarId = _sessionManager.RetrieveFromSession<string>(ConstantsNeamb.SessionSeminaryId);
            if (string.IsNullOrEmpty(seminarId))
            {
                seminarId = renderingItem[Templates.RetirementSeminarCta.Fields.Seminar];
                Sitecore.Diagnostics.Log.Info($"Seminar id item Sitecore {seminarId}", this);
            }
            return seminarId;
        }

        public string GetLeaCode(Item renderingItem) {
            var seminary = GetSeminary(renderingItem);
            return seminary != null ? seminary.LeaCode : "";
        }

        public bool IsValidSeminaryId(Item renderingItem)
        {
            var seminar = GetSeminary(renderingItem);
            return seminar != null;
        }
    }
}