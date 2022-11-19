using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Description;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;

namespace Neambc.Neamb.Feature.GeneralContent.Services
{
    [Service(typeof(IBulkService))]
    public class BulkService : IBulkService
    {
        public int RemoveOnClickEvent(string eventName, bool performPublish) {
            var result = 0;
            if (Configuration.RemoveGaEvents) {
                foreach (var removeGaItem in Configuration.RemoveGaItems) {
                    var id = new ID(removeGaItem);
                    var database = Sitecore.Configuration.Factory.GetDatabase("master");
                    var item = database.GetItem(id);
                    if (item != null)
                    {
                        Log.Warn($"RemoveOnClickEvent: Item {removeGaItem} found in master", this);
                        using (new SecurityDisabler())
                        {
                            item.Editing.BeginEdit();
                            foreach (Field field in item.Fields)
                            {
                                field.Value = RemoveEvent(field.Value, eventName);
                            }
                            item.Editing.EndEdit();
                            if (performPublish)
                            {
                                PublishItem(item);
                            }
                            result++;
                        }
                    } else {
                        Log.Warn($"RemoveOnClickEvent: Item {removeGaItem} not found in master", this);
                    }
                }
            }
            return result;
        }
        private string RemoveEvent(string fieldValue, string eventName) {
            var pattern = $@"({eventName})([(])([^)]+)(\);?)";
            var match = Regex.Match(fieldValue, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                fieldValue = Regex.Replace(fieldValue, pattern, string.Empty);
            }
            fieldValue = fieldValue.Replace(@"onclick=""""", string.Empty);
            return fieldValue;
        }

        private void PublishItem(Item item)
        {
            using (new SecurityDisabler())
            {
                // The publishOptions determine the source and target database,
                // the publish mode and language, and the publish date
                var publishOptions =
                    new Sitecore.Publishing.PublishOptions(item.Database,
                        Database.GetDatabase("web"),
                        Sitecore.Publishing.PublishMode.SingleItem,
                        item.Language,
                        DateTime.Now);  // Create a publisher with the publish options

                publishOptions.UserName = $"sitecore\\{Configuration.PublishingUser}";
                var publisher = new Sitecore.Publishing.Publisher(publishOptions);

                // Choose where to publish from
                publisher.Options.RootItem = item;

                // Publish children as well?
                publisher.Options.Deep = false;

                // Do the publish!
                Log.Warn($"RemoveOnClickEvent: Publishing Item {item.ID} from master to web", this);
                publisher.PublishAsync();

                item.Publishing.ClearPublishingCache();
            }
        }
    }
}