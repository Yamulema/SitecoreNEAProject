using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Feature.Forms.Repositories;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Forms.Controllers {
	public class PasswordDisavowController : BaseController {
		private readonly IFormsRepository _formsRepository;
		private readonly IBase64Service _base64Service;
		private const string PASSWORD_DISAVOW_VIEW = "/Views/Forms/PasswordDisavow.cshtml";

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="formsRepository"></param>
		/// <param name="base64Service"></param>
		public PasswordDisavowController(IFormsRepository formsRepository, IBase64Service base64Service) {
			_formsRepository = formsRepository;
			_base64Service = base64Service;
		}

		public ActionResult PasswordDisavow() {
			var username = Request.Params["id"];

			var isExperienceEditor = Sitecore.Context.PageMode.IsExperienceEditor;
			var passwordDisavowModel = new PasswordDisavowModel(RenderingContext.Current.Rendering);

			if (isExperienceEditor) {
				return View(PASSWORD_DISAVOW_VIEW, passwordDisavowModel);
			}

			username = username ?? string.Empty;
			var decodedUsername = _base64Service.Decode(username);
			_formsRepository.CancelResetToken(decodedUsername, passwordDisavowModel);
			if (passwordDisavowModel.IsCanceled) {
				return View(PASSWORD_DISAVOW_VIEW, passwordDisavowModel);
			} 

			Sitecore.Data.Fields.LinkField errorPage = passwordDisavowModel.Item.Fields[Templates.PasswordDisavow.Fields.ErrorPage];

			if (errorPage != null && errorPage.TargetItem != null) {
				var redirectUrl = LinkManager.GetItemUrl(errorPage.TargetItem);
				return Redirect(redirectUrl);
			} 
			return View(PASSWORD_DISAVOW_VIEW, passwordDisavowModel);
		}



	}
}