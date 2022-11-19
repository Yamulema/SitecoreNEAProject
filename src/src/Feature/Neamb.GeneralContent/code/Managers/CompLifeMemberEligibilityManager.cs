using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore;

namespace Neambc.Neamb.Feature.GeneralContent.Managers {
	[Service(typeof(ICompLifeMemberEligibilityManager))]
	public class CompLifeMemberEligibilityManager : ICompLifeMemberEligibilityManager {

		#region Fields
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly ICompIntroLifeService _compIntroLifeService;
        #endregion

        #region Constructor
        public CompLifeMemberEligibilityManager(ISessionAuthenticationManager sessionAuthenticationManager,
            ICompIntroLifeService compIntroLifeService) {
			_sessionAuthenticationManager = sessionAuthenticationManager;
            _compIntroLifeService = compIntroLifeService;
        }
		#endregion

		#region Public Methods
		public bool GetResultEligibility(string value) {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			if (accountMembership.Status != StatusEnum.Hot && accountMembership.Status != StatusEnum.WarmHot &&
				accountMembership.Status != StatusEnum.WarmCold) {
				return false;
			}

            //Call the webservice to get the user's eligibility in comp and intro life
            var result = _compIntroLifeService.GetCompIntroEligibility(accountMembership.Mdsid);

            //Get the Sitecore item that comes from Value that is the personalization rule defined in the component
			var selectedItem = Context.Database.GetItem(value);
			if (selectedItem == null) {
				Sitecore.Diagnostics.Log.Debug("Selected item is empty", this);
				return false;
			}

			//Get the value field of the Global sitecore item
			var valueItemReferenced = selectedItem[Templates.CategoryItem.Fields.Value];

			//Personalization rule set as I (Global Item) and user eligibility result
			if (!valueItemReferenced.Equals(ConstantsNeamb.IntroLifeForEligibility) ||
				!result.IntroEligible)
                return false;

            return true;
		}
		#endregion
	}
}