using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.AccessToken
{
    public class TokenResponse : RestBaseResponse
    {
        public TokenModel Data { get; set; }
    }
}