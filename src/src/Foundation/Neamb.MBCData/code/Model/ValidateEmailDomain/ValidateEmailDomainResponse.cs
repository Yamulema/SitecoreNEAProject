using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.ValidateEmailDomain
{
    
   public class ValidateEmailDomainResponse : RestBaseResponse
    {
        public ValidateEmailDomainResponseData Data { get; set; }
        public bool IsAuthenticated { get; set; }
        public ValidateEmailDomainResponse()
        {
            Data = null;
            IsAuthenticated = false;
        }

    }

}