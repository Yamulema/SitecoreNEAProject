using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class TwoColumnStackableDTO : HeaderDTO, IRenderingModel
    {
        public bool HasHeadline { get; private set; }
        public new void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            HasHeadline = HasValueField(Templates.TwoColumnStackable.Fields.Header);
            HasSubheadline = HasValueField(Templates.TwoColumnStackable.Fields.Subhead);
        }
    }
}