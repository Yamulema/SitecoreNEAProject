namespace Neambc.Neamb.Foundation.Cache.Model {
	public class RedisCacheSeiumbConfiguration : IRedisCacheSeiumbConfiguration {

		#region Fields
		//default values

		#endregion

		#region Properties

		public string EnvironmentCacheKey { get; set; }

		#endregion

		#region Constructor
		/// <summary>
		/// constructor with connectio and environment set
		/// </summary>
		public RedisCacheSeiumbConfiguration() {
			EnvironmentCacheKey = "default";
		}
		#endregion

	}
}