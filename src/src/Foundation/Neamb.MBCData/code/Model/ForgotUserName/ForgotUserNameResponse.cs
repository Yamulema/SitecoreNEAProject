using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.ForgotUserName
{
    
   public class ForgotUserNameResponse : RestBaseResponse
    {
        public ForgotUserNameResponseData Data { get; set; }
        public bool IsAuthenticated { get; set; }
        public ForgotUserNameErrorCodeEnum ErrorCodeResponse { get; set; }
        public ForgotUserNameResponse()
        {
            Data = null;
            IsAuthenticated = false;
        }

    }

}