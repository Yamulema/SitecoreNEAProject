using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;

namespace Neambc.Neamb.Foundation.MBCData.Services.AccessToken
{
    public interface IAccessTokenService
    {
        TokenResponse GetAccessTokenFromRedis();
        TokenResponse GetAccessTokenFromServerAndSaveRedis();
    }
}
