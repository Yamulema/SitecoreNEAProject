using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.SecurityManagement.Encrypt
{
    public class AESEncryptResponse : RestBaseResponse
    {
        public AESEncryptModel Data { get; set; }
    }
}