using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.CloudFront;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.Aws
{
    [Service(typeof(ICloudFrontFactory))]
    public class CloudFrontFactory : ICloudFrontFactory {
        private readonly ILog _log;
        public CloudFrontFactory(ILog log) {
            _log = log;
        }

        public ICloudFrontProxy GetClient() {
            return new CloudFrontProxy(new AmazonCloudFrontClient(), _log);
        }
    }
}