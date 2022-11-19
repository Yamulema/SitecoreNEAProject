using System;
using Neambc.Neamb.Feature.Banner.Repositories.Enums;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Banner.Models
{
    public class TopSiteBannerDto : IRenderingModel
    {
        public bool IsHidden { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            var siteSettingItem = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);
            var topBannerItemReferenced = siteSettingItem[Templates.TopSiteBannerSetting.Fields.Banner];
            if (string.IsNullOrEmpty(topBannerItemReferenced)) {
                IsHidden = true;
            } else {
                IsHidden = false;
                Item = Sitecore.Context.Database.GetItem(new ID(topBannerItemReferenced));
            }
            
            
            PageItem = PageContext.Current.Item;
        }
    }
}