using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Model.CompIntroLife;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
	public interface IEligibilityCompIntroLife
	{
		EligibilityResultEnum IsMemberEligible(string mdsid);
        EligibilityResultEnum IsCurrentSessionEligible();
        CompIntroLifeEligibilityModel GetResultCompIntroLifeEligibility(string mdsid);
        EligibilityResultEnum GetEligibilityResult(CompIntroLifeEligibilityModel model);
    }
}