using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Feature.Forms.Repositories;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Forms.Controllers {
	public class ResetPasswordController : BaseController {
		private readonly IFormsRepository _formsRepository;
		private readonly IBase64Service _base64Service;
		private const string VIEW_RESET_PASSWORD = "/Views/Forms/ResetPassword.cshtml";

		public ResetPasswordController(IFormsRepository formsRepository, IBase64Service base64Service) {
			_formsRepository = formsRepository;
			_base64Service = base64Service;
		}


		public ActionResult ResetPassword(string id, string s) {
			var decodedId = _base64Service.Decode(id);
			var username = HttpUtility.UrlDecode(HttpUtility.UrlEncode(decodedId));
			var token = HttpUtility.UrlDecode(HttpUtility.UrlEncode(s));
			var resetPasswordModel = new ResetPasswordModel();
			resetPasswordModel.Initialize(RenderingContext.Current.Rendering);
			_formsRepository.ValidateResetToken(username, token, resetPasswordModel);
			resetPasswordModel.UserName = username;
			return View(VIEW_RESET_PASSWORD, resetPasswordModel);
		}

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult ResetPassword(ResetPasswordModel model) {
			if (ModelState.IsValid) {
				_formsRepository.ResetPassword(model.UserName, model.NewPassword, model.ConfirmPassword, model);
				model.Initialize(RenderingContext.Current.Rendering);
				model.Submitted = true;
				if (model.IsPasswordReset) {
					Sitecore.Data.Fields.LinkField thankYouPage = model.Item.Fields[Templates.ResetPassword.Fields.ThankYouPage];
					if (thankYouPage?.TargetItem != null) {
						return Redirect(Sitecore.Links.LinkManager.GetItemUrl(thankYouPage.TargetItem));
					}
				}
			} else {
				model.Initialize(RenderingContext.Current.Rendering);
			}
			return View(VIEW_RESET_PASSWORD, model);
		}
	}
}