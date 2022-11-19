using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers {
	public class RetirementIncomeCalculatorController : BaseController {
		private readonly ISessionAuthenticationManager _sessionManager;
		public RetirementIncomeCalculatorController(ISessionAuthenticationManager sessionManager) {
			_sessionManager = sessionManager;
		}
		public ActionResult RetirementIncomeCalculator() {
			return View("/Views/Neamb.GeneralContent/Renderings/RetirementIncomeCalculator.cshtml", CreateModel());
		}
		private RetirementIncomeCalculatorDTO CreateModel() {
			var retirementIncomeCalculatorDTO = new RetirementIncomeCalculatorDTO(_sessionManager);
			retirementIncomeCalculatorDTO.Initialize(RenderingContext.Current.Rendering);
			return retirementIncomeCalculatorDTO;
		}
	}
}