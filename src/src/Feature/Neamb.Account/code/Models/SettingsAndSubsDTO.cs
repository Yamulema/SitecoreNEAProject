using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class SettingsAndSubsDTO : IRenderingModel
    {
        public List<Newsletter> Newsletters { get; set; }
        public bool HasGeneralError { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public StatusEnum UserStatus { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            Newsletters = new List<Newsletter>();
        }
    }
}