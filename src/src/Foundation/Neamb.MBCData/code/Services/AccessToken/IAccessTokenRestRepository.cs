using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Services.AccessToken
{
    public interface IAccessTokenRestRepository
    {
        RestResultDto<TokenResponse> Post(RestRequestDto restRequestDto);
    }
}
