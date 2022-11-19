using Neambc.Neamb.Foundation.Eligibility.Model;

namespace Neambc.Neamb.Foundation.Eligibility.Interfaces
{
    public interface IEligibilityManagerMarketplace
    {
        EligibilityResultEnum IsMemberEligible(string mdsid, string productcode);
    }
}