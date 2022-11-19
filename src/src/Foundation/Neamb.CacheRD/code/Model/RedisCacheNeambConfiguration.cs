namespace Neambc.Neamb.Foundation.Cache.Model {
	public class RedisCacheNeambConfiguration : IRedisCacheNeambConfiguration {

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
		public RedisCacheNeambConfiguration() {
			EnvironmentCacheKey = "default";
		}
		#endregion

	}
}