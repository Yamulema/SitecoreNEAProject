using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Seiumb.Feature.Navigation.Attributes;

namespace Neambc.Seiumb.Feature.Navigation.Models
{
    /// <summary>
    /// Position of the menu
    /// </summary>
    public enum NavigationPosition
    {
        [StringValue("")] None,
        [StringValue("left nav")]  Left,
        [StringValue("footer")]  Bottom,
        [StringValue("top nav")]  Top
    }
}