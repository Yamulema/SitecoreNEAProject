using Sitecore.Data.Items;

namespace Neambc.Seiumb.Foundation.Sitecore.Utility {
	public static class LanguageHelper {
		public static bool HasAContextLanguage(this Item item) {
			var ret = false;
			if ((item != null) && (item.Versions != null) && (item.Versions.Count > 0)) {
				var latestLanguageVersion = item.Versions.GetLatestVersion();
				ret = (latestLanguageVersion != null) &&
					(!latestLanguageVersion.IsFallback) &&
					(latestLanguageVersion.Versions.Count > 0);
			}
			return ret;
		}
	}
}