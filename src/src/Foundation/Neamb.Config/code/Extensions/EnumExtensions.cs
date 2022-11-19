using System;
using System.ComponentModel;
using System.Globalization;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.Configuration.Extensions {
	public static class EnumExtensions {
		public static T FromDescription<T>(string description) {
			try {
				var fields = typeof(T).GetFields();

				foreach (var field in fields) {
					var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

					if (attributes != null && attributes.Length > 0 && attributes[0].Description == description) {
						return (T)Enum.Parse(typeof(T), field.Name);
					}
				}

			} catch (Exception e) {
				Log.Error("Error while parsing description into Enum.", e, typeof(EnumExtensions));
			}

			// Defaults to first attribute in Enum.
			var array = Enum.GetValues(typeof(T));
			return array.Length > 0 ? (T)array.GetValue(0) : throw new Exception("Empty Enum definition.");
		}

		public static string GetDescription<T>(this T e) where T : IConvertible {
			if (!(e is Enum)) {
				return null;
			}

			string result = null;
			var type = e.GetType();
			var values = Enum.GetValues(type);

			foreach (int value in values) {
				if (value != e.ToInt32(CultureInfo.InvariantCulture)) {
					continue;
				}

				var memInfo = type.GetMember(type.GetEnumName(value));
				var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (descriptionAttributes.Length > 0) {
					// We're only getting the first description we find
					// others will be ignored
					result = ((DescriptionAttribute)descriptionAttributes[0]).Description;
				}
				break;
			}

			return result;
		}
	}
}