using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Services.Rakuten
{
    public interface IRakutenMemberService
    {
        RestResultDto<MemberCreationResponse> CreateMember(bool isNEA, string email, string mdsId, string cellCode);
    }
} 
