using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;


namespace Neambc.Neamb.Feature.GeneralContent.Services

{
    [Service(typeof(ISmartPublish))]


    public class SmartPublish : ISmartPublish
    {
        private readonly ISmartPublishLog _smartpublishLog;

        public SmartPublish(ISmartPublishLog smartpublishLog)
        {
            _smartpublishLog = smartpublishLog;
        }


        public void PublishItem(Item rootItem, bool withChildren = true)
        {
            _smartpublishLog.Debug("Starting publish process");
            //TODO: Improve exception handling for this method
            if (rootItem == null)
            {
                _smartpublishLog.Debug("Auto Publication Error - Root Item Null");
                return;
            }

            try
            {
                using (new SecurityDisabler())
                {
                    _smartpublishLog.Debug($"Starting publish process {rootItem.ID.ToString()}");
                    var publishOptions = new Sitecore.Publishing.PublishOptions(rootItem.Database,
                        Database.GetDatabase("web"),
                        Sitecore.Publishing.PublishMode.Smart,
                        rootItem.Language,
                        System.DateTime.Now);

                    // Create a publisher with the publish options
                    var publisher = new Sitecore.Publishing.Publisher(publishOptions);
                    // Choose where to publish from
                    publisher.Options.RootItem = rootItem;
                    // Publish children as well?
                    publisher.Options.Deep = withChildren;
                    // Do the publish!
                    publisher.Publish();
                    _smartpublishLog.Debug($"Ending publish process {rootItem.ID.ToString()}");

                }
            }
            catch (Exception ex)
            {
                _smartpublishLog.Error($"Publish Error in {rootItem.ID.ToString()}", ex);
            }
        }

        public void PublishItem(ID parentId)
        {
            var master = Sitecore.Data.Database.GetDatabase("master");
            if (master == null)
            {
                _smartpublishLog.Debug("master DB is not available.");
                return;
            }
            else
            {
                var rootItem = master.GetItem(parentId);
                if (rootItem == null)
                {
                    _smartpublishLog.Debug("Parent ID is null.");
                    return;
                }
                else
                {
                    PublishItem(rootItem);
                }

            }

        }
    }
}