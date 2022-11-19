using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Feature.Forms.Repositories;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Forms.Controllers {
	public class ForgotUserNameController : BaseController {
		private readonly IFormsRepository _formsRepository;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="formsRepository"></param>
		public ForgotUserNameController(IFormsRepository formsRepository) {
			_formsRepository = formsRepository;
		}


		public ActionResult ForgotUserName() {
			var retrieveUserNameModel = new RetrieveUserNameModel();
			retrieveUserNameModel.Initialize(RenderingContext.Current.Rendering);
			return View("/Views/Forms/ForgotUserName.cshtml", retrieveUserNameModel);
		}

		[HttpPost]
		[ValidateInput(false)]
		[ValidateFormHandler]
		
		public ActionResult ForgotUserName(RetrieveUserNameModel model) {
			if (ModelState.IsValid) {
				_formsRepository.ValidateRetrieveUserName(model);
				model.Initialize(RenderingContext.Current.Rendering);
				model.Submitted = true;
				return View("/Views/Forms/ForgotUserName.cshtml", model);
			}
			var modelStateVal = ViewData.ModelState["FirstName"];

			model.HasErrorFirstName = modelStateVal.Errors.Count > 0;
			if (model.HasErrorFirstName) {
				model.HasErrorFirstNameInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
				model.HasErrorFirstNameLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
			}
			modelStateVal = ViewData.ModelState["LastName"];

			model.HasErrorLastName = modelStateVal.Errors.Count > 0;
			if (model.HasErrorLastName) {
				model.HasErrorLastNameInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
				model.HasErrorLastNameLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
			}

			modelStateVal = ViewData.ModelState[nameof(model.ZipCode)];
			model.HasErrorZipcode = modelStateVal.Errors.Count > 0;
			if (model.HasErrorZipcode) {
				model.HasErrorZipcodeLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
			}

			modelStateVal = ViewData.ModelState[nameof(model.DateOfBirth)];
			model.HasErrorBirthDate = modelStateVal.Errors.Count > 0;
			if (model.HasErrorBirthDate) {
				//verify the age greater than 16 years
				model.HasErrorDateOfBirthAge = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("DateRangeError")) != null;
			}

			model.Initialize(RenderingContext.Current.Rendering);
			return View("/Views/Forms/ForgotUserName.cshtml", model);
		}
	}
}