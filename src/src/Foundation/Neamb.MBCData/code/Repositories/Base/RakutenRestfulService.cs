using System;
using System.Net;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Seiumb.Foundation.Sitecore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace Neambc.Neamb.Foundation.MBCData.Repositories.Base
{
    [Service(typeof(IRakutenRestfulService))]
    public class RakutenRestfulService: IRakutenRestfulService
    {
        private readonly ILog _log;
        
        public RakutenRestfulService(ILog log)
        {
            _log = log;
        }

        public RestResultDto<T> Get<T>(RestRequestDto restRequestDto)
        {
            try
            {
                var request = new RestRequest(restRequestDto.Action, Method.GET);
                return BaseCall<T>(restRequestDto, request);
            }
            catch (Exception ex)
            {
                return ExceptionHandling<T>("GET", ex);
            }
        }
        private RestResultDto<T> BaseCall<T>(RestRequestDto restRequestDto, RestRequest request)
        {
            var client = new RestClient(restRequestDto.Server);
            if (restRequestDto.Parameters != null)
            {
                foreach (var parameterItem in restRequestDto.Parameters)
                {
                    request.AddParameter(parameterItem.Key, parameterItem.Value);
                }
            }
            if (restRequestDto.Headers != null)
            {
                foreach (var headerItem in restRequestDto.Headers)
                {
                    request.AddHeader(headerItem.Key, headerItem.Value);
                }
            }
            if (restRequestDto.Body != null) {
                var camelCaseSerializer = JsonSerializer.Create(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                var objectJson = JObject.FromObject(restRequestDto.Body, camelCaseSerializer);
                request.AddParameter("application/json", objectJson, ParameterType.RequestBody);
             }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var response = client.Execute(request);
            watch.Stop();

            _log.Debug($"{request.Method.ToString()} " +
                $"Operation server: {restRequestDto.Server}, " +
                $"Action: {request.Resource}, " +
                $"Status code : {response.StatusCode}, " +
                $"Time (ms) : {watch.Elapsed.Milliseconds}");

            var success = response.StatusCode.Equals(HttpStatusCode.OK) || response.StatusCode.Equals(HttpStatusCode.Created);

            return new RestResultDto<T>
            {
                Success = success,
                StatusCode = response.StatusCode,
                Headers = response.Headers,
                Result = restRequestDto.ParseJson ? JsonConvert.DeserializeObject<T>(response.Content,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }) : default(T)
            };
        }

        private RestResultDto<T> ExceptionHandling<T>(string operation, Exception ex)
        {
            _log.Error($"RestRepository {operation} operation failed: {ex.Message}", ex, this);

            return new RestResultDto<T>
            {
                Success = false,
                ExceptionDetail = $"EXCEPTION: {ex.Message}",
                Result = default(T)
            };
        }

        public RestResultDto<T> Post<T>(RestRequestDto restRequestDto)
        {
            var request = new RestRequest
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Json,
                Resource = restRequestDto.Action
            };
            return BaseCall<T>(restRequestDto, request);

        }
    }
}