using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.Configuration.Services.ActionReminder
{
    [Service(typeof(IActionReminderService))]
    public class ActionReminderService : IActionReminderService {
        private readonly ICacheManager _cacheManager;
        public ActionReminderService(ICacheManager cacheManager) {
            _cacheManager = cacheManager;
        }
        public void SetVisited(PageType pageType, string userName) {
            var key = GetKey(pageType,userName);
            if (string.IsNullOrEmpty(key)){
                return;
            }
            SetKey(key);
        }
        public bool WasVisited(PageType pageType, string userName) {
            var key = GetKey(pageType, userName);
            return !string.IsNullOrEmpty(key) && HasKey(key);
        }

        public void RemoveVisited(PageType pageType, string userName) {
            var key = GetKey(pageType, userName);
            if (string.IsNullOrEmpty(key)) {
                return;
            }
            RemoveKey(key);
        }
        public void SetAll(string username) {
            foreach (var pageType in (PageType[])Enum.GetValues(typeof(PageType))) {
                SetVisited(pageType, username);
            }
        }
        public void RemoveAll(string username) {
            foreach (var pageType in (PageType[])Enum.GetValues(typeof(PageType))) {
                RemoveVisited(pageType, username);
            }
        }
       
        private string GetKey(PageType pageType, string userName) {
            switch (pageType)
            {
                case PageType.Profile:
                    return $"{Configuration.ReminderKey}:{Configuration.ProfileKey}:{userName}";
                case PageType.Subscription:
                    return $"{Configuration.ReminderKey}:{Configuration.SubscriptionKey}:{userName}";
                case PageType.Complife:
                    return $"{Configuration.ReminderKey}:{Configuration.ComplifeKey}:{userName}";
                default:
                    return string.Empty;
            }
        }
        private void SetKey(string key) {
            if (HasKey(key)) {
                return;
            }
            //Add business logic here if you need to store a value for a given key.
            _cacheManager.StoreInCache(key, true);
        }
        private bool HasKey(string key) {
            //Add business logic here if you need to validate the value a key.
            return _cacheManager.ExistInCache(key);
        }
        private void RemoveKey(string key)
        {
            if (HasKey(key)){
                //Add business logic here if you need to flag a value for a given key.
                _cacheManager.Remove(key);
            }
        }

        public int Migrate() {
            if (Configuration.MigrateVisitedPages) {
                var records = GetRecords();
                SetVisited(records);
                return records.Count();
            }
            return 0;
        }

        private IEnumerable<Tuple<string, IEnumerable<PageType>>> GetRecords() {
            var records = new List<Tuple<string, IEnumerable<PageType>>>();
            var keys = _cacheManager.SearchPatternInCache("Neambc:VisitedPages:*");
            foreach (var key in keys){
                var value = _cacheManager.RetrieveFromCache<string>(key);
                var identifier = GetIdentifier(key);
                if (string.IsNullOrEmpty(identifier)){
                    continue;
                }
                var pageTypes = GetPageTypes(value);
                if (!pageTypes.Any()){
                    continue;
                }
                records.Add(new Tuple<string, IEnumerable<PageType>>(identifier, pageTypes));
            }
            return records;
        }

        private string GetIdentifier(string key) {
            var regex = new Regex(@"(.+\:)*(.+)\:(.*)", RegexOptions.IgnoreCase);
            var match = regex.Match(key);
            return match.Success ? match?.Groups[3]?.Value : null;
        }

        private IEnumerable<PageType> GetPageTypes(string value) {
            var result = new List<PageType>();
            if (value.Contains("1")) {
                result.Add(PageType.Profile);
            }
            if (value.Contains("2"))
            {
                result.Add(PageType.Subscription);
            }
            if (value.Contains("3"))
            {
                result.Add(PageType.Complife);
            }
            return result;
        }
        private void SetVisited(IEnumerable<Tuple<string, IEnumerable<PageType>>> records) {
            foreach (var record in records) {
                foreach (var pageType in record.Item2) {
                    Log.Warn($"Setting page reminder {pageType} for {record.Item1}", this);
                    SetVisited(pageType, record.Item1);
                }
            }
        }
        private string GetPath(PageType pageType) {
            switch (pageType)
            {
                case PageType.Profile:
                    return $"{Configuration.ReminderKey}:{Configuration.ProfileKey}:*";
                case PageType.Subscription:
                    return $"{Configuration.ReminderKey}:{Configuration.SubscriptionKey}:*";
                case PageType.Complife:
                    return $"{Configuration.ReminderKey}:{Configuration.ComplifeKey}:*";
                default:
                    return string.Empty;
            }
        }

        public void RemoveVisited(PageType pageType) {
            var path = GetPath(pageType);
            _cacheManager.RemoveFolder(path);
        }
     }
}