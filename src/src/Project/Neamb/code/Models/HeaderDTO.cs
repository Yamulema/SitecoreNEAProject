using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Project.Web.Models
{
    public class HeaderDTO : IRenderingModel
    {
        public string Name { get; set; }

        public StatusEnum Status { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public bool IsLoginPage { get; set; }
        public string SearchPlaceholder { get; set; }
        public List<LinkHeaderItem> LinkPages { get; set; }
        public bool HasNotificationAvatar { get; set; }
        public string StateLogo { get; set; }
        public Item SiteSettings { get; set; }
	    public string UserImageUrl { get; set; }
	    public string Mdsid { get; set; }
	    public string PersonaCode { get; set; }
        public IEnumerable<Menu> Menus { get; set; }
        public string OnLoadEvent { get; set; }
        public UserIdentifier UserIdentifier { get; set; }
		public string GtmAction { get; set; }
        
        public bool DisplaLoginPopup { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            IsLoginPage = false;
            Status = StatusEnum.Cold;
            SiteSettings = GetSiteSettings();
            SearchPlaceholder = SiteSettings!=null?SiteSettings.Fields[Templates.SiteSettings.Fields.HeaderSearchPlaceholder].Value:"";
            LinkPages = new List<LinkHeaderItem>();
            HasNotificationAvatar = false;
        }

        

        private Item GetSiteSettings()
        {
            var datasourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            return Sitecore.Context.Database.GetItem(datasourceId);
        }
    }
}