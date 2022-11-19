using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Amazon.CloudFront;
using Amazon.CloudFront.Model;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Seiumb.Foundation.Sitecore;
using Newtonsoft.Json;
using Sitecore.Tasks;

namespace Neambc.Neamb.Foundation.MBCData.Services.Aws
{
    public class CloudFrontProxy : ICloudFrontProxy {
        private readonly ILog _log;
        private readonly IAmazonCloudFront _client;
        public CloudFrontProxy(IAmazonCloudFront client, ILog log) {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _client = client;
        }
        public async Task<CreateInvalidationResponse> CreateInvalidationAsync(CreateInvalidationRequest request) {
            try {
                _log.Info($"Calling AWS CreateInvalidationAsync request:{JsonConvert.SerializeObject(request)}", this);
                return await _client.CreateInvalidationAsync(request);
            } catch (Exception e) {
                var ex = new NeambAwsException($"Error in CreateInvalidationAsync request:{JsonConvert.SerializeObject(request)}", e);
                _log.Error(ex.Message, ex, this);
                throw ex;
            }
        }
    }
}