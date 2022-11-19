using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;

namespace Neambc.Neamb.Foundation.MBCData.Repositories.Rest
{
    public interface ISecurityRestBaseRepository
    {
        string ExecuteEncryption(TokenModel token,object securityRequest, string url);
    }
}