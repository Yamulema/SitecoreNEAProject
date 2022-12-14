using System;
using System.Net;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.CompIntroLife;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Model.UpdateUserName;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Seiumb.Foundation.Sitecore;
using Newtonsoft.Json;
using RestSharp;


namespace Neambc.Neamb.Foundation.MBCData.Repositories.Rest
{
    [Service(typeof(IUpdateUserNameRestRepository))]
    public class UpdateUserNameRestRepository : IUpdateUserNameRestRepository
    {
        private readonly ILog _log;
        private readonly IAccessTokenService _accessTokenService;

        public UpdateUserNameRestRepository(ILog log, IAccessTokenService accessTokenService)
        {
            _log = log;
            _accessTokenService = accessTokenService;
        }

        public APIResult Post(RestRequestDto restRequestDto)
        {
            try
            {
                if (restRequestDto.Server == null || restRequestDto.Body == null ||
                    string.IsNullOrEmpty(restRequestDto.Token) || string.IsNullOrEmpty(restRequestDto.Action))
                    throw new ArgumentException($"Parameters in UpdateUserNameRestRepository for Post are incorrect");

                var response = RawPost(restRequestDto);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                           var UpdateUserNameResult = JsonConvert.DeserializeObject<UpdateUserNameResponse>(response.Content, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                            return UpdateUserNameResult;
                          
                        }
                    case HttpStatusCode.Unauthorized:
                        {
                            _log.Error($"UpdateUserNameRestRepository Response: {JsonConvert.SerializeObject(response)}", this);
                            var accessTokenRetry = _accessTokenService.GetAccessTokenFromServer();

                            if (!accessTokenRetry.Success) throw new Exception("GetAccessTokenFromServer retry in UpdateUserNameRestRepository has errors");

                            var newToken = (TokenResponse)accessTokenRetry;
                            restRequestDto.Token = newToken.Data.AccessToken;

                            var responseRetry = RawPost(restRequestDto);
                            var UpdateUserNameResult = JsonConvert.DeserializeObject<UpdateUserNameResponse>(responseRetry.Content,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });
                            return UpdateUserNameResult;
                        }
                    default:
                        return new APIResult();
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
            request.AddHeader("Authorization", $"Bearer {restRequestDto.Token}");
            request.AddJsonBody(restRequestDto.Body);

            var client = new RestClient(restRequestDto.Server);

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

        public APIResult ExceptionHandling(string operation, Exception ex)
        {
            _log.Error($"UpdateUserNameRestRepository {operation} operation failed: {ex.Message}", ex, this);

            return new APIResult
            {
                Success = false,
                ExceptionDetail = $"EXCEPTION: {ex.Message}"
            };
        }

        public APIResponse<T> Get<T>(RestRequestDto restRequestDto)
        {
            throw new NotImplementedException();
        }

        public APIResponse<T> Post<T>(RestRequestDto restRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}