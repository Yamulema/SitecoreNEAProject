using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Links;
using Sitecore.Sites;
using sit=Sitecore;

namespace Neambc.Seiumb.Foundation.Sitecore.Utility
{
	public static class RedirectionHelper
	{
		public static bool CanRedirectPreviousPage(string currentPath)
		{
			var pagesExclude = sit.Configuration.Settings.GetSetting("ExcludePagesThankyou");
			var pagesExcludeList = pagesExclude.Split('|');
			foreach (var pagesExcludeItem in pagesExcludeList)
			{
				var item = sit.Context.Database.GetItem(new sit.Data.ID(pagesExcludeItem));
				if (item != null)
				{
					if (LinkManager.GetItemUrl(item).Equals(currentPath))
					{
						return false;
					}
				}
			}
			return true;
		}

        /// <summary>
        /// Get the Sitecore path given the Uri url.
        /// </summary>
        /// <param name="url">Uri url</param>
        /// <returns>Sitecore path</returns>
        public static string GetFullPath(Uri url)
        {
            // Obtain a SiteContext for the host and virtual path
            var siteContext = SiteContextFactory.GetSiteContext(url.Host, url.PathAndQuery);

            // Get the path to the Home item
            var homePath = siteContext.StartPath;
            if (!homePath.EndsWith("/"))
                homePath += "/";

            // Get the path to the item, removing virtual path if any
            var itemPath = sit.MainUtil.DecodeName(url.AbsolutePath);
            itemPath = HttpUtility.UrlDecode(itemPath);
            if (itemPath != null && itemPath.StartsWith(siteContext.VirtualFolder))
                itemPath = itemPath.Remove(0, siteContext.VirtualFolder.Length);

            // Obtain the item
            var fullPath = homePath + itemPath;
            return fullPath;
        }

    }
}