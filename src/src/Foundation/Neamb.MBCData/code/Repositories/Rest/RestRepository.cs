using System;
using System.Net;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Seiumb.Foundation.Sitecore;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Neambc.Neamb.Foundation.MBCData.Repositories.Rest
{
    [Service(typeof(IRestRepository))]
    public class RestRepository : IRestRepository
    {
        private readonly ILog _log;

        public RestRepository(ILog log)
        {
            _log = log;
        }

        public APIResponse<T> Get<T>(RestRequestDto restRequestDto)
        {
            try
            {
                var request = new RestRequest(restRequestDto.Action, Method.GET);

                if (!string.IsNullOrEmpty(restRequestDto.Token))
                {
                    request.AddHeader("Authorization", $"Bearer {restRequestDto.Token}");
                }

                return BaseCall<T>(restRequestDto,request);
            }
            catch (Exception ex)
            {
                return ExceptionHandling<T>("GET", ex);
            }
        }

        public APIResponse<T> Post<T>(RestRequestDto restRequestDto)
        {
            try
            {
                var request = new RestRequest(restRequestDto.Action, Method.POST) { RequestFormat = DataFormat.Json };
                request.AddJsonBody(restRequestDto.Body);

                if (!string.IsNullOrEmpty(restRequestDto.Token))
                {
                    request.AddHeader("Authorization", $"Bearer {restRequestDto.Token}");
                }

                return BaseCall<T>(restRequestDto,request);
            }
            catch (Exception ex)
            {
                return ExceptionHandling<T>("POST", ex);
            }
        }

        public APIResult Post(RestRequestDto restRequestDto)
        {
            throw new NotImplementedException();
        }

        public IRestResponse RawPost(RestRequestDto restRequestDto)
        {
            throw new NotImplementedException();
        }

        public APIResult ExceptionHandling(string operation, Exception ex)
        {
            throw new NotImplementedException();
        }

        #region Private Methods

        private APIResponse<T> BaseCall<T>(RestRequestDto restRequestDto, RestRequest request)
        {
            var client = new RestClient(restRequestDto.Server);
            if (restRequestDto.IsBasicAuthentication)
            {
                var authProfile = (TokenRequest)restRequestDto.Body;
                client.Authenticator = new HttpBasicAuthenticator(authProfile.Username, authProfile.Password);
            }
            if (restRequestDto.Parameters != null) {
                foreach (var parameterItem in restRequestDto.Parameters) {
                    request.AddParameter(parameterItem.Key, parameterItem.Value);
                }
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var response = client.Execute(request);
            watch.Stop();

            _log.Debug($"{request.Method.ToString()} " +
                $"Operation server: {restRequestDto.Server}, " +
                $"Action: {request.Resource}, " +
                $"Status code : {response.StatusCode}, " +
                $"Time (ms) : {watch.Elapsed.Milliseconds}");

            var success = response.StatusCode.Equals(HttpStatusCode.OK);
            if (!success)
            {
                _log.Error($"RestRepository Response: {JsonConvert.SerializeObject(response)}", this);
            }
            return new APIResponse<T>
            { 
                Success = success,
                StatusCode = response.StatusCode,
                Headers = response.Headers,
                Result = restRequestDto.ParseJson ? JsonConvert.DeserializeObject<T>(response.Content,
                    new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    }) : default(T)
            };
        }

        private APIResponse<T> ExceptionHandling<T>(string operation, Exception ex)
        {
            _log.Error($"RestRepository {operation} operation failed: {ex.Message}", ex, this);

            return new APIResponse<T>
            {
                Success = false,
                ExceptionDetail = $"EXCEPTION: {ex.Message}",
                Result = default(T)
            };
        }

        #endregion
    }
}