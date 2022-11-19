using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Extensions
{
    public static class ItemExtensions {
        public static bool HasRendering(this Item item, ID id) {
            var layout = new Sitecore.Data.Fields.LayoutField(item);
            var renderings = layout.GetReferences(GetDeviceItem("default"));
            return renderings.Any(x => x.RenderingID == id);
        }
        private static DeviceItem GetDeviceItem(string deviceName) {
            return ID.IsID(deviceName) 
                ? Context.Database.Resources.Devices.GetAll().First(d => d.ID.Guid == new Guid(deviceName)) 
                : Context.Database.Resources.Devices.GetAll().First(d => string.Equals(d.Name, deviceName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}