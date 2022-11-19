using System.Web.Mvc;
using Neambc.Neamb.Feature.Contest.Interfaces;
using Neambc.Neamb.Feature.Contest.Model;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;

namespace Neambc.Neamb.Feature.Contest.Controllers {
	public class ContestSubmissionController : BaseController {

		#region Fields
		private const string CONTEST_SUBMISSION_VIEW = "/Views/Neamb.Contest/ContestSubmission.cshtml";
		private readonly IContestSubmissionService _contestSubmissionService;
		#endregion

		#region Constructors
		public ContestSubmissionController(IContestSubmissionService contestSubmissionService) {
			_contestSubmissionService = contestSubmissionService;
		}
		#endregion

		#region Public Methods
		public ActionResult ContestSubmission() {
			var model = new ContestSubmissionDto();
			_contestSubmissionService.FillModelContestSubmission(model);
			return View(CONTEST_SUBMISSION_VIEW, model);
		}

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult ContestSubmission(ContestSubmissionDto model) {
			_contestSubmissionService.ExecuteSubmission(model, ViewData, ModelState);
			return View(CONTEST_SUBMISSION_VIEW, model);
		}
		#endregion
	}
}