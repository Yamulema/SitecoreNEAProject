using System.Xml;
using Neambc.Neamb.Foundation.Cache.Model;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore;

namespace Neambc.Neamb.Foundation.Cache.Repositories {
	[Service(typeof(ICacheConnectionConfigRepo))]
	public class RedisCacheConnectionConfigRepo : ICacheConnectionConfigRepo
    {
        public RedisCacheConnectionConfigRepo()
        {
            var xmlNode = Sitecore.Configuration.Factory.GetConfigNode("neamb/cacheconfig");
            CacheConnectionConfiguration = Sitecore.Configuration.Factory.CreateObject(xmlNode, true) as IRedisCacheConnectionConfiguration;
        }

        public IRedisCacheConnectionConfiguration CacheConnectionConfiguration { get; }
	}
}