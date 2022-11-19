using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Model.CompIntroLife;
using Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
	[Service(typeof(IEligibilityCompIntroLife))]
	public class EligibilityCompIntroLife: IEligibilityCompIntroLife
	{
        private readonly ISessionAuthenticationManager _sessionManager;
        private readonly ICompIntroLifeService _compIntroLifeService;

        public EligibilityCompIntroLife(ICompIntroLifeService compIntroLifeService, ISessionAuthenticationManager sessionManager) {
            _compIntroLifeService = compIntroLifeService;
            _sessionManager = sessionManager;
        }

		/// <summary>
		/// Get the Eligible for Comp Life page
		/// </summary>
		/// <param name="mdsid">Mdsid</param>
		/// <returns></returns>
		public EligibilityResultEnum IsMemberEligible(string mdsid) {
            var compModel = _compIntroLifeService.GetCompIntroEligibility(mdsid);
            return GetEligibilityResult(compModel);
        }
        public EligibilityResultEnum GetEligibilityResult(CompIntroLifeEligibilityModel compModel) {
            return compModel.CompEligible ? EligibilityResultEnum.Eligible : EligibilityResultEnum.NotEligible;
        }

        public CompIntroLifeEligibilityModel GetResultCompIntroLifeEligibility(string mdsid) {
            return _compIntroLifeService.GetCompIntroEligibility(mdsid);
        }

        public EligibilityResultEnum IsCurrentSessionEligible()
        {
            if (Sitecore.Context.PageMode.IsExperienceEditor) {
                return EligibilityResultEnum.Eligible;
            }
            var accountMembership = _sessionManager.GetAccountMembership();
            return IsMemberEligible(accountMembership?.Mdsid);
        }
    }
}