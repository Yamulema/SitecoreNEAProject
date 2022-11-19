using Neambc.Neamb.Foundation.Eligibility.Model;

namespace Neambc.Neamb.Foundation.Eligibility.Interfaces
{
    public interface IEligibilityManagerMarketplaceSeiumb
    {
        bool IsMemberEligible(int mdsid);
    }
}