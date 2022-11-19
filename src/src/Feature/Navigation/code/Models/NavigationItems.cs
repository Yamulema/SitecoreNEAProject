using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Navigation.Models
{
    public class NavigationItems
    {
        /// <summary>
        /// Collection of the menu item
        /// </summary>
        public IList<NavigationItem> Items { get; set; }
    }
}