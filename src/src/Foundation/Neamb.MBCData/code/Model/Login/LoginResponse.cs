using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.Login
{
    public class LoginResponse : RestBaseResponse
    {
        public LoginRestModel Data { get; set; }
        public bool IsAuthenticated { get; set; }
        public LoginUserErrorCodeEnum ErrorCodeResponse { get; set; }
        public LoginResponse() {
            Data = null;
            IsAuthenticated = false;
        }
    }
}