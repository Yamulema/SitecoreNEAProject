using System.Xml;
using Neambc.Neamb.Foundation.Cache.Model;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore;

namespace Neambc.Neamb.Foundation.Cache.Repositories {
	[Service(typeof(ICacheNeambConfigRepo))]
	public class RedisCacheNeambConfigRepo : ICacheNeambConfigRepo
    {
        public RedisCacheNeambConfigRepo()
        {
            var xmlNode = Sitecore.Configuration.Factory.GetConfigNode("neamb/cacheneamb");
            CacheNeambConfiguration = Sitecore.Configuration.Factory.CreateObject(xmlNode, true) as IRedisCacheNeambConfiguration;
        }

        public IRedisCacheNeambConfiguration CacheNeambConfiguration { get; }
	}
}