using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;

namespace Neambc.Seiumb.Foundation.Sitecore.Utility {
	public static class RenderingHelper {

		#region Public Methods
		/// <summary>
		/// Return the first rendering to be rendered in a specific placeholder on the "default" device
		/// </summary>
		public static RenderingItem GetRenderingsByPlaceholder(string placeholderKey, Item item) {
			RenderingItem ret = null;
			var renderings = GetRenderingReferences(item, "default");
			if (renderings != null) {
				foreach (var rendering in renderings) {
					if (rendering.Placeholder == placeholderKey) {
						ret = rendering.RenderingItem;
						break;
					}
				}
			}
			return ret;
		}

		/// <summary>
		/// Return all renderings from an item defined on a device
		/// </summary>
		public static RenderingReference[] GetRenderingReferences(Item item, string deviceName) {
			LayoutField layoutField = item.Fields["__renderings"];
			var renderings = layoutField.GetReferences(GetDeviceItem(item.Database, deviceName));
			return renderings;
		}

		/// <summary>
		/// Get the device item from a device name
		/// </summary>
		public static DeviceItem GetDeviceItem(Database db, string deviceName) {
			return db.Resources.Devices.GetAll().First(d => d.Name.ToLower() == deviceName.ToLower());
		}
		#endregion
	}
}