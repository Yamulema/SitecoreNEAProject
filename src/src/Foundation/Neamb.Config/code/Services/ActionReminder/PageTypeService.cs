using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Foundation.Configuration.Services.ActionReminder
{
    [Service(typeof(IPageTypeService))]
    public class PageTypeService : IPageTypeService
    {
        public PageType GetPageType(Item pageItem) {
            if (pageItem.IsDerived(Configuration.ProfilePasswordId)) {
                return PageType.Profile;
            }
            if (pageItem.IsDerived(Configuration.SettingSubscriptionId))
            {
                return PageType.Subscription;
            }
            if (pageItem.IsDerived(Configuration.CompLifeId))
            {
                return PageType.Complife;
            }
            return PageType.NotHandled;
        }
    }
}