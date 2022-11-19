using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.RetrieveUser
{
    public class RetrieveUserResponse : RestBaseResponse
    {
        public RetrieveUserModel Data { get; set; }
    }
}