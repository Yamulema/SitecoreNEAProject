using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Project.Web.Models
{
    public class Menu
    {
        public string MenuClass { get; set; }
        public string UrlItem { get; set; }
        public string ClassAnchor { get; set; }
        public string Toggle { get; set; }
        public bool HasSubMenu { get; set; }
        public string ClickAction { get; set; }
        public Item Item { get; set; }
    }
}