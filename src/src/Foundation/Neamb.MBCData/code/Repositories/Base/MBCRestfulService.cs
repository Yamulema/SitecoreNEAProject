using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Text;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.Sitecore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Neambc.Neamb.Foundation.MBCData.Repositories.Base {
    // ReSharper disable once InconsistentNaming
    [Service(typeof(IMBCRestfulService))]
    public class MBCRestfulService : IMBCRestfulService {
        private readonly ILog _log;
        private readonly IAccessTokenService _accessTokenService;

        public MBCRestfulService(ILog log, IAccessTokenService accessTokenService)
        {
            _log = log;
            _accessTokenService = accessTokenService;
        }

        public RestResultDto<T> Post<T>(RestRequestDto restRequestDto)
        {
            try
            {
                if (restRequestDto.Server == null || restRequestDto.Body == null || string.IsNullOrEmpty(restRequestDto.Token) || string.IsNullOrEmpty(restRequestDto.Action))
                {
                    throw new ArgumentException($"Parameters in MBCRestfulService for Post are incorrect");
                }

                var response = LowLevelPost(restRequestDto);
                var restResultDto = HandleResponse<T>(response);

                if (!restResultDto.Success) {
                    restRequestDto.Token = UpdateToken();
                    response = LowLevelPost(restRequestDto);
                    restResultDto = HandleResponse<T>(response);
                }
                return restResultDto;
            }
            catch (Exception ex)
            {
                _log.Error($"MBCRestfulService Post operation failed", ex);

                return new RestResultDto<T>
                {
                    Success = false,
                    ExceptionDetail = $"Exception: {ex.Message}"
                };
            }
        }
        private RestResultDto<T> HandleResponse<T>(IRestResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    return HandleOkResponse<T>(response);
                }
                case HttpStatusCode.Unauthorized:
                {
                    var unAuthMessage = $"Unauthorized MBCRestfulService Response: {JsonConvert.SerializeObject(response)}";
                    _log.Error(unAuthMessage,this);
                    return new RestResultDto<T>
                    {
                        Success = false,
                        ExceptionDetail = unAuthMessage
                    };
                }
                default:
                    var message = $"Unexpected response: {response?.StatusCode.ToString()} on service MBCRestfulService";
                    _log.Warn(message,this);
                    return new RestResultDto<T>
                    {
                        Success = false,
                        ExceptionDetail = message
                    };
            }
        }

        private string UpdateToken()
        {
            var accessTokenRetry = _accessTokenService.GetAccessTokenFromServerAndSaveRedis();
            return accessTokenRetry.Data.AccessToken;
        }

        private RestResultDto<T> HandleOkResponse<T>(IRestResponse response)
        {
            return new RestResultDto<T>
            {
                Success = true,
                StatusCode = response.StatusCode,
                Result = JsonConvert.DeserializeObject<T>(response.Content,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })
            };
        }

        private IRestResponse LowLevelPost(RestRequestDto restRequestDto)
        {
            var request = new RestRequest
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Json,
                Resource = restRequestDto.Action,
            };
            request.AddHeader("Authorization", $"Bearer {restRequestDto.Token}");
            var camelCaseSerializer = JsonSerializer.Create(
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            var objectJson = JObject.FromObject(restRequestDto.Body, camelCaseSerializer);
            request.AddParameter("application/json", objectJson, ParameterType.RequestBody);

            var client = new RestClient(restRequestDto.Server);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var response = client.Execute(request);
            watch.Stop();

            _log.Debug($"{request.Method.ToString()} "
                + $"Operation server: {restRequestDto.Server}, "
                + $"Action: {request.Resource}, "
                + $"Status code : {response.StatusCode}, "
                + $"Time (ms) : {watch.Elapsed.Milliseconds}");

            return response;
        }
    }
}