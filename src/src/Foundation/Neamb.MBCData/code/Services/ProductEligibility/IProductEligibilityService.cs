namespace Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility
{
    public interface IProductEligibilityService
    {
        bool GetEligibility(int mdsId, string productCode);
    }
}
