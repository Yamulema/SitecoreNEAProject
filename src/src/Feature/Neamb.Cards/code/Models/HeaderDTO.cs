using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public abstract class HeaderDTO : IRenderingModel
    {
        public Rendering Rendering { get; protected set; }
        public Item Item { get; protected set; }
        public bool HasSubheadline { get; protected set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
        }
        protected bool HasValueField(ID field)
        {
            var value = Item.Fields[field].Value;
			var hasField = !string.IsNullOrEmpty(value);
            return hasField;
        }
    }
}