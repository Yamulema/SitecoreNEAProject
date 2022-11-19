using Neambc.Neamb.Foundation.MBCData.Model.CompIntroLife;

namespace Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife
{
    public interface ICompIntroLifeService
    {
        /// <summary>
        /// Get eligibility for Comp/Intro Products
        /// </summary>
        /// <param name="mdsId">Mdsid user</param>
        /// <returns></returns>
        CompIntroLifeEligibilityModel GetCompIntroEligibility(string mdsId);
    }
}
