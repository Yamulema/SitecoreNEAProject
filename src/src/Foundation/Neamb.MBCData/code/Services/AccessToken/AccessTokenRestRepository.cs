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

namespace Neambc.Neamb.Foundation.MBCData.Services.AccessToken
{
    [Service(typeof(IAccessTokenRestRepository))]
    public class AccessTokenRestRepository : IAccessTokenRestRepository
    {
        private readonly ILog _log;
        public AccessTokenRestRepository(ILog log)
        {
            _log = log;
            
        }

        public RestResultDto<TokenResponse> Post(RestRequestDto restRequestDto)
        {
            try
            {
                if (restRequestDto.Server == null || restRequestDto.Body == null || string.IsNullOrEmpty(restRequestDto.Action))
                    throw new ArgumentException($"Parameters in AccessTokenRestRepository for Post are incorrect");

                var response = RawPost(restRequestDto);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                            return new RestResultDto<TokenResponse> {
                                Result = tokenResponse,
                                Success = true
                            };
                        }
                    case HttpStatusCode.Unauthorized:
                        {
                            _log.Error($"AccessTokenRestRepository Response: {JsonConvert.SerializeObject(response)}", this);
                            return new RestResultDto<TokenResponse>();
                        }
                    default:
                        return new RestResultDto<TokenResponse>();
                }
            }
            catch (Exception ex)
            {
                return ExceptionHandling("POST", ex);
            }
        }

        public IRestResponse RawPost(RestRequestDto restRequestDto)
        {
            var request = new RestRequest
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Json,
                Resource = restRequestDto.Action
            };
            request.AddJsonBody(restRequestDto.Body);

            var client = new RestClient(restRequestDto.Server);
            var authProfile = (TokenRequest)restRequestDto.Body;
            client.Authenticator = new HttpBasicAuthenticator(authProfile.Username, authProfile.Password);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var response = client.Execute(request);
            watch.Stop();

            _log.Debug($"{request.Method.ToString()} " +
                $"Operation server: { restRequestDto.Server }, " +
                $"Action: {request.Resource}, " +
                $"Status code : {response.StatusCode}, " +
                $"Time (ms) : {watch.Elapsed.Milliseconds}");

            return response;
        }

        public RestResultDto<TokenResponse> ExceptionHandling(string operation, Exception ex)
        {
            _log.Error($"AccessTokenRestRepository {operation} operation failed: {ex.Message}", ex, this);

            return new RestResultDto<TokenResponse>
            {
                Success = false,
                ExceptionDetail = $"EXCEPTION: {ex.Message}"
            };
        }

    }
}