using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.SearchUserName
{
    public class SearchUserNameResponse : RestBaseResponse
    {
        public SearchUserNameModel Data;
        public SearchUsernameErrorCodes ErrorCode { get; set; }
    }
}