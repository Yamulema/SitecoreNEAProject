using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CloudFront.Model;

namespace Neambc.Neamb.Foundation.MBCData.Services.Aws {
    public interface ICloudFrontProxy {
        Task<CreateInvalidationResponse> CreateInvalidationAsync(CreateInvalidationRequest request);
    }
}