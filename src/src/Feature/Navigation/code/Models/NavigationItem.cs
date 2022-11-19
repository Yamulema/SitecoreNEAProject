using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Navigation.Models
{
    public class NavigationItem
    {
        /// <summary>
        /// Name to be displayed in the menu
        /// </summary>
        public Item Item { get; set; }
        /// <summary>
        /// Url of the menu item
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Flag to indicate if the menu correspond to the page that is opened in this moment in the browser
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Children of the items
        /// </summary>
        public NavigationItems Children { get; set; }

        /// <summary>
        /// String with GTM Data Layer content
        /// </summary>
        public string OnClickEventContent { get; set; }
    }
}