using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Model;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Foundation.Configuration.Utility
{
	public static class ProcessPagesHelper
	{
		/// <summary>
		/// Verify if the current page has been visited
		/// </summary>
		/// <param name="currentItem">Current page</param>
		/// <param name="profileComponent">Profile component id</param>
		/// <param name="suscriptionComponent">Setting and subscriptions id</param>
		/// <param name="compLifeComponent">Comp life component id</param>
		/// <param name="cacheManager">Cache manager</param>
		/// <param name="username">user name</param>
		public static void ExecuteProcessPageVisited(Item currentItem, ID profileComponent, ID suscriptionComponent,
			ID compLifeComponent, ICacheManager cacheManager, string username)
		{
			if (currentItem.IsDerived(profileComponent))
			{
				ExecuteProcessPageVisitedInner(cacheManager, username, "1");
			}
			else if (currentItem.IsDerived(suscriptionComponent))
			{
				ExecuteProcessPageVisitedInner(cacheManager, username, "2");
			}
			else if (currentItem.IsDerived(compLifeComponent))
			{
				ExecuteProcessPageVisitedInner(cacheManager, username, "3");
			}
		}

		/// <summary>
		/// Save in redis that the page has been visited
		/// </summary>
		/// <param name="cacheManager">Redis classes</param>
		/// <param name="username">User name logged</param>
		/// <param name="pagetype">Id that identify the page</param>
		private static void ExecuteProcessPageVisitedInner(ICacheManager cacheManager, string username, string pagetype)
		{
			var uniqueName =
				string.Format("{0}{1}", ConstantsNeamb.RedisKeyVisitedPages, username);

			if (cacheManager.ExistInCache(uniqueName))
			{
				var cacheValue = cacheManager.RetrieveFromCache<string>(uniqueName);
				if (!cacheValue.Contains(pagetype))
				{
					cacheValue = string.Format("{0}|{1}", cacheValue, pagetype);
					cacheManager.StoreInCache(uniqueName, cacheValue);
				}
			}
			else
			{
				cacheManager.StoreInCache(uniqueName, pagetype);
			}
		}

		/// <summary>
		/// Verify if the page has been visited by the user
		/// </summary>
		/// <param name="cacheManager">Cache manager class</param>
		/// <param name="currentItem">Item that referenced to the page</param>
		/// <param name="profileComponent">Profile component id</param>
		/// <param name="suscriptionComponent">Setting and subscription component id</param>
		/// <param name="compLifeComponent">Comp life component id</param>
		/// <param name="username">User name</param>
		/// <returns></returns>
		public static PageVisitedResult VerifyPageVisited(ICacheManager cacheManager, Item currentItem, ID profileComponent,
			ID suscriptionComponent, ID compLifeComponent, string username)
		{
			var uniqueName =
				string.Format("{0}{1}", ConstantsNeamb.RedisKeyVisitedPages, username);
			var valueCache = cacheManager.RetrieveFromCache<string>(uniqueName);
			var result = new PageVisitedResult();
			if (valueCache != null)
			{
				if (currentItem.IsDerived(profileComponent))
				{
					result.NeedCheck = true;
					result.ResultCheck = valueCache.Contains("1");
				}
				else if (currentItem.IsDerived(suscriptionComponent))
				{
					result.NeedCheck = true;
					result.ResultCheck = valueCache.Contains("2");
				}
				else if (currentItem.IsDerived(compLifeComponent))
				{
					result.NeedCheck = true;
					result.ResultCheck = valueCache.Contains("3");
				}
				else
				{
					result.NeedCheck = false;
					result.ResultCheck = false;
				}
			}
			else
			{
				if (currentItem.IsDerived(profileComponent) || currentItem.IsDerived(suscriptionComponent) ||
				    currentItem.IsDerived(compLifeComponent))
				{
					result.NeedCheck = true;
					result.ResultCheck = false;
				}
				else
				{
					result.NeedCheck = false;
					result.ResultCheck = false;
				}
			}

			return result;
		}

		public static void DeletePageVisited(ICacheManager cacheManager, string username)
		{
			var uniqueName =
				string.Format("{0}{1}", ConstantsNeamb.RedisKeyVisitedPages, username);
			cacheManager.Remove(uniqueName);
		}
	}
}