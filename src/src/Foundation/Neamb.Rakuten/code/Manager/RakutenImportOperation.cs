using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using System;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Publishing;

namespace Neambc.Neamb.Foundation.Rakuten.Manager {
    [Service(typeof(IRakutenImportOperation))]
    public class RakutenImportOperation : IRakutenImportOperation {
        private readonly IRakutenLog _rakutenLog;

        public RakutenImportOperation(IRakutenLog rakutenLog) {
            _rakutenLog = rakutenLog;
        }
        public void DeleteItemSitecore(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            if (Settings.RecycleBinActive)
            {
                item.Recycle();
            }
            else
            {
                item.Delete();
            }
        }

        public void PublishItem(Item rootItem, bool withChildren = true)
        {
            _rakutenLog.Debug("Starting publish process");
            //TODO: Improve exception handling for this method
            if (rootItem == null) {
                _rakutenLog.Debug($"Import Operation Publication Error - Root Item Null");
                return;
            }

            try {
                using (new SecurityDisabler()) {
                    _rakutenLog.Debug($"Starting publish process {rootItem.ID.ToString()}");
                    PublishOptions publishOptions = new PublishOptions(rootItem.Database,
                        Database.GetDatabase("web"),
                        Sitecore.Publishing.PublishMode.SingleItem,
                        rootItem.Language,
                        DateTime.Now);
                    publishOptions.RootItem = rootItem;
                    publishOptions.Deep = withChildren;


                    var handle = PublishManager.Publish(new PublishOptions[] { publishOptions });
                    PublishManager.WaitFor(handle);
                    Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                    Database[] databases = new Database[1] { db };

                    Sitecore.Handle publishHandle = Sitecore.Publishing.PublishManager.PublishItem(rootItem, databases, db.Languages, withChildren, false);
                    PublishManager.WaitFor(publishHandle);


                    //var handle = PublishManager.Publish(new PublishOptions[] { publishOptions });
                    //PublishManager.WaitFor(handle);
                    //Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                    //Database[] databases = new Database[1] { db };

                    //Sitecore.Handle publishHandle = Sitecore.Publishing.PublishManager.PublishItem(rootItem, databases, db.Languages, true, false);
                    //PublishManager.WaitFor(publishHandle);

                    //var publishOptions = new Sitecore.Publishing.PublishOptions(rootItem.Database,
                    //    Database.GetDatabase("web"),
                    //    Sitecore.Publishing.PublishMode.Full,
                    //    rootItem.Language,
                    //    System.DateTime.Now);

                    //// Create a publisher with the publish options
                    //var publisher = new Sitecore.Publishing.Publisher(publishOptions);
                    //// Choose where to publish from
                    //publisher.Options.RootItem = rootItem;
                    //// Publish children as well?
                    //publisher.Options.Deep = withChildren;
                    //// Do the publish!
                    //publisher.Publish();
                    _rakutenLog.Debug($"Ending publish process {rootItem.ID.ToString()}");
                }
            }
            catch (Exception ex)
            {
                _rakutenLog.Error("Import Operation Publication Error", ex);
            }
        }

        public void PublishItem(ID parentId)
        {
            var master = Sitecore.Data.Database.GetDatabase("master");
            var rootItem = master.GetItem(parentId);
            PublishItem(rootItem);
        }
    }
}