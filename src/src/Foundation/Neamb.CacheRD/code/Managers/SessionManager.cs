using System;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.Cache.Managers
{
    [Service(typeof(ISessionManager))]
    public class SessionManager : ISessionManager
	{
        /// <summary>
		/// Remove from Session a key
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>True when success: otherwise false</returns>
        public bool Remove(string key)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");
            try
            {
				HttpContext.Current.Session.Remove(key);
	            return true;
            }
            catch (Exception ex)
            {
                Log.Warn($"Error removing from Session returning false, key {key}", ex, this);
                return false;
            }
        }

        /// <summary>
		/// Retrieve from the session that key
		/// </summary>
		/// <typeparam name="T">Type of the object to be saved</typeparam>
		/// <param name="key">Session key</param>
		/// <returns>Value from the session</returns>
        public T RetrieveFromSession<T>(string key)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");

            try
            {
				return (T)(HttpContext.Current.Session[key]);
			}
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
		/// Save in session the object/value
		/// </summary>
		/// <typeparam name="T">Type of the object to be saved</typeparam>
		/// <param name="key">Session key</param>
		/// <param name="element">Value/object to be saved</param>
        public void StoreInSession<T>(string key, T element)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");
            Assert.ArgumentNotNull(element, "element");

            try
            {
	            HttpContext.Current.Session[key] = element;

            }
            catch (Exception ex)
            {
                Log.Error("Error during storage in session", ex, this);
            }
        }
    }
}