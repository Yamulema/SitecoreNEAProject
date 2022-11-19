using System;

namespace Neambc.Neamb.Foundation.MBCData.Model.AccessToken
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public long ExpiresAt { get; set; }
        public DateTime ExpiresAtInRedis { get; set; }
        public string TokenType { get; set; }
    }
}