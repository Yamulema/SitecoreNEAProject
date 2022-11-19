using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;

namespace Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility
{
    public interface IProductRestBaseRepository 
    {
        bool GetEligibility(TokenModel accessToken, object productRestRequest, string url);
    }
}