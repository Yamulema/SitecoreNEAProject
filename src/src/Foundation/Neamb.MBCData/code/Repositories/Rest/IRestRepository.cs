using System;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using RestSharp;

namespace Neambc.Neamb.Foundation.MBCData.Repositories.Rest
{
    public interface IRestRepository
    {
        APIResponse<T> Get<T>(RestRequestDto restRequestDto);
        APIResponse<T> Post<T>(RestRequestDto restRequestDto);
        APIResult Post(RestRequestDto restRequestDto);
        IRestResponse RawPost(RestRequestDto restRequestDto);
        APIResult ExceptionHandling(string operation, Exception ex);
    }
}
