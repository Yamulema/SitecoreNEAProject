using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Seiumb.Foundation.Sitecore;


namespace Neambc.Neamb.Foundation.MBCData.Services.Rakuten
{
    [Service(typeof(IRakutenStoreRestRepository))]
    public class RakutenStoreRestRepository : IRakutenStoreRestRepository
    {
        private readonly ILog _log;
        private readonly IRakutenRestfulService _rakutenRestfulService;
        private readonly IGlobalConfigurationManager _config;
        public RakutenStoreRestRepository(ILog log, IRakutenRestfulService rakutenRestfulService, IGlobalConfigurationManager config)
        {
            _log = log;
            _rakutenRestfulService = rakutenRestfulService;
            _config = config;
        }

        public RestResultDto<StoreResponse> GetStore(string etagStored)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            var headers = new List<KeyValuePair<string, string>>();
            //Build the parameters, url, server to set the Rest object
            parameters.Add(new KeyValuePair<string, string>("channel", _config.RakutenStoreChannel));
            if (!string.IsNullOrEmpty(etagStored))
            {
                headers.Add(new KeyValuePair<string, string>("If-Modified-Since", String.Format("{0:r}", DateTime.UtcNow)));
                headers.Add(new KeyValuePair<string, string>("If-None-Match", etagStored));
            }

            var restRequestDto = new RestRequestDto
            {
                Action = _config.RakutenStoreApiUrl,
                Parameters = parameters,
                Server = _config.RakutenServerApiUrl,
                ParseJson = true,
                Headers= headers
            };

            if (restRequestDto == null || restRequestDto.Server == null || restRequestDto.Action == null)
                throw new ArgumentException($"Parameter in GetStore is incorrect");

            RestResultDto<StoreResponse> resultStoreResponse =new RestResultDto<StoreResponse>();
            try
            {
                resultStoreResponse = _rakutenRestfulService.Get<StoreResponse>(restRequestDto);
                if (resultStoreResponse.Headers != null && resultStoreResponse.Success && resultStoreResponse.StatusCode== System.Net.HttpStatusCode.OK) {
                    var eTag = resultStoreResponse.Headers.FirstOrDefault(item => item.Name == "ETag");
                    if (eTag != null) {
                        resultStoreResponse.Result.Etag = eTag.Value as string;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Get Store Api call action failed: {ex.Message}", ex, this);
            }
            return resultStoreResponse;
        }

        public RestResultDto<StoreDetailResponseTop> GetStoreDetail(RestRequestDto restRequestDto)
        {
            if (restRequestDto == null || restRequestDto.Server == null || restRequestDto.Action == null)
                throw new ArgumentException($"Parameter in GetStoreDetail is incorrect");

            RestResultDto<StoreDetailResponseTop> resultStoreDetailResponse = new RestResultDto<StoreDetailResponseTop>();
            try
            {
                resultStoreDetailResponse = _rakutenRestfulService.Get<StoreDetailResponseTop>(restRequestDto);
            }
            catch (Exception ex)
            {
                _log.Error($"Get Store Detail Api call action failed: {ex.Message}", ex, this);
            }
            return resultStoreDetailResponse;
        }
    }
}