using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.SecurityManagement.Decrypt
{
    public class AESDecryptResponse : RestBaseResponse
    {
        public AESDecryptModel Data { get; set; }
    }
}