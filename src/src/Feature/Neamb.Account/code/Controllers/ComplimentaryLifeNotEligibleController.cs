using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers
{
	public class ComplimentaryLifeNotEligibleController : SitecoreController
	{
		private readonly IEligibilityCompIntroLife _eligibilityCompIntroLife;
		private readonly ISessionAuthenticationManager _sessionManager;

		public ComplimentaryLifeNotEligibleController(
			IEligibilityCompIntroLife eligibilityCompIntroLife, ISessionAuthenticationManager sessionManager
			)
		{
			_eligibilityCompIntroLife = eligibilityCompIntroLife;
			_sessionManager = sessionManager;
		}

		public ActionResult ComplimentaryLifeEligibility()
		{
			var accountMembership = _sessionManager.GetAccountMembership();
			var model = new ComplimentaryLifeDTO();
			model.Initialize(RenderingContext.Current.Rendering);
			if (accountMembership.Status == StatusEnum.Hot)
			{
				//Get the eligibility result
				model.EligibilityResult = _eligibilityCompIntroLife.IsMemberEligible(accountMembership.Mdsid);
				model.MembershipType = accountMembership.Profile.MembershipType;
				model.AccountStatus = accountMembership.Status;
			}

			return View("/Views/Neamb.Account/ComplimentaryLife/ComplimentaryLifeNotEligible.cshtml", model);
		}
	}
}