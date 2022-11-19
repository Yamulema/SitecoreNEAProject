using Neambc.Neamb.Foundation.Cache.Model;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Foundation.Cache.Repositories {
	[Service(typeof(ICacheSeiumbConfigRepo))]
	public class RedisCacheSeiumbConfigRepo : ICacheSeiumbConfigRepo
    {
        public RedisCacheSeiumbConfigRepo()
        {
            var xmlNode = Sitecore.Configuration.Factory.GetConfigNode("seiumb/cacheseiumb");
            CacheSeiumbConfiguration = Sitecore.Configuration.Factory.CreateObject(xmlNode, true) as IRedisCacheSeiumbConfiguration;
        }

        public IRedisCacheSeiumbConfiguration CacheSeiumbConfiguration { get; }
	}
}