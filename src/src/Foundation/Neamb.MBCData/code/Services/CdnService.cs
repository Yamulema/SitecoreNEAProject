using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Amazon.CloudFront.Model;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Neamb.Foundation.MBCData.Services.Aws;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore.Tasks;

namespace Neambc.Neamb.Foundation.MBCData.Services
{
    [Service(typeof(ICdnService))]
    public class CdnService : ICdnService {
        private readonly ICloudFrontFactory _cloudFrontFactory;
        private readonly ILog _log;
        public CdnService(ICloudFrontFactory cloudFrontFactory, ILog log) {
            _cloudFrontFactory = cloudFrontFactory;
            _log = log;
        }
        public async Task<bool> InvalidateAsync(IEnumerable<string> paths) {
            if (paths == null || !paths.Any()) {
                return true;
            }
            try {
                var client = _cloudFrontFactory.GetClient();
                var response = await client.CreateInvalidationAsync(new CreateInvalidationRequest()
                {
                    DistributionId = Configuration.DefaultCloudFrontDistributionId,
                    InvalidationBatch = new InvalidationBatch()
                    {
                        CallerReference = Guid.NewGuid().ToString(),
                        Paths = new Paths()
                        {
                            Items = paths.ToList(),
                            Quantity = paths.Count()
                        }
                    }
                });
                return !string.IsNullOrEmpty(response?.Invalidation?.Id);
            }
            catch (NeambAwsException e) {
                _log.Error("Error while reaching AWS", e, this);
                return false;
            }
            catch (Exception e) {
                _log.Error("Error in InvalidateAsync", e, this);
                throw;
            }
        }
    }
}