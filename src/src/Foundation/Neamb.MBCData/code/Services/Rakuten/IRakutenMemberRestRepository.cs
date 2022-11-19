using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Services.Rakuten
{
    public interface IRakutenMemberRestRepository {
        RestResultDto<MemberCreationResponse> Post(RestRequestDto restRequestDto);
    }
}
