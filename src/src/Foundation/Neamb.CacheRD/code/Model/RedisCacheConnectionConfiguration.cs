namespace Neambc.Neamb.Foundation.Cache.Model {
	public class RedisCacheConnectionConfiguration : IRedisCacheConnectionConfiguration
    {

		#region Fields
		//default values

		#endregion

		#region Properties

		public string PooledConnection { get; set; }
		
		#endregion

		#region Constructor
		/// <summary>
		/// constructor with connection and environment set
		/// </summary>
		public RedisCacheConnectionConfiguration() {
			PooledConnection = "127.0.0.1";
		}
		#endregion

	}
}