using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.UpdateUser
{
    public class UpdateUserResponse : RestBaseResponse
    {
        public UpdateUserResponseData Data { get; set; }
        public bool IsAuthenticated { get; set; }
        public UpdateUserErrorCodeEnum ErrorCodeResponse { get; set; }
        public UpdateUserResponse()
        {
            Data = null;
            IsAuthenticated = false;
        }

    }
}



