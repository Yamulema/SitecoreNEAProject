using System;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Data.Items;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.Analytics.Managers {
    [Service(typeof(IAnalyticsManager))]
    public class AnalyticsManager : IAnalyticsManager {
        private readonly ILog _logService;

        public AnalyticsManager(ILog logService) {
            _logService = logService;
        }
        public void TrackSiteSearch(string query) {
            //var pageEventItem = Guid.Empty;

            //Assert.ArgumentNotNull(pageEventItem, nameof(pageEventItem));
            //Assert.IsNotNull(pageEventItem, $"Cannot find page event: {pageEventItem}");

            if (Tracker.IsActive) {
                var searchPageEventGuid = Sitecore.Context.Database.GetItem("{0C179613-2073-41AB-992E-027D03D523BF}").ID.Guid;
                var pageEventData = new PageEventData("Search", searchPageEventGuid) {
                    ItemId = Guid.Empty,
                    Data = query,
                    DataKey = query,
                    Text = query
                };
                var interaction = Tracker.Current.Session.Interaction;
                if (interaction != null) {
                    interaction.CurrentPage.Register(pageEventData);
                }
            }
        }

        /// <summary>
        /// Set the goal programatically the one related to cta action
        /// </summary>
        /// <param name="goalItem">Goal item</param>
        /// <returns>True in case of success; otherwise false</returns>
        public bool SetGoal(Item goalItem) {
            if (goalItem == null) {
                _logService.Error("Goal item is null",this);
                throw new ArgumentException("Goal item is null", "goalItem");
            }
            _logService.Debug("Setting Goal starting", this);
            var flag = false;
            try {
                if (!Tracker.Enabled) {
                    return false;
                }
                //Check if tracker is active or not
                if (!Tracker.IsActive) {
                    Tracker.StartTracking();
                }

                if (Tracker.IsActive && Tracker.Current.CurrentPage != null) {
                    var goalTrigger = Tracker.MarketingDefinitions.Goals[goalItem.ID.ToGuid()];

                    var goalEventData = Tracker.Current.CurrentPage.RegisterGoal(goalTrigger);
                    goalEventData.Data = goalItem["Name"];
                    goalEventData.ItemId = goalItem.ID.ToGuid();
                    goalEventData.DataKey = goalItem.Paths.Path;
                    goalEventData.Text = goalItem["Description"];
                    _logService.Debug($"Setting Goal data {goalEventData.Data},{goalEventData.ItemId},{goalEventData.DataKey},{goalEventData.Text}  ", this);
                    var pageData = new Sitecore.Analytics.Data.PageEventData(goalTrigger.Alias, goalTrigger.Id);
                    Sitecore.Analytics.Tracker.Current.CurrentPage.Register(pageData);
                    Tracker.Current.Interaction.AcceptModifications();
                }
                flag = true;
            } catch (Exception ex) {
                _logService.Error($"Error tracking goal {goalItem.ID}", ex, this);
            }
            _logService.Debug("Setting Goal ending", this);
            return flag;
        }

    }
}