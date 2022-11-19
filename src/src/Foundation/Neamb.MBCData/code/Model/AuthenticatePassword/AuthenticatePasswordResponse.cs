using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.AuthenticatePassword
{
    
   public class AuthenticatePasswordResponse : RestBaseResponse
    {
        public AuthenticatePasswordResponseData Data { get; set; }          
        public bool IsAuthenticated { get; set; }
        public AuthenticatePasswordResponse()
        {
            Data = null;
            IsAuthenticated = false;
        }

    }

}