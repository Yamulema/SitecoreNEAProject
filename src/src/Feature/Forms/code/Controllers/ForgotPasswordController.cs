using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Feature.Forms.Repositories;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Forms.Controllers {
	public class ForgotPasswordController : BaseController {
		private readonly IFormsRepository _formsRepository;
		private readonly ILockedAccountService _lockedAccountService;

		/// <summary>
		/// Constructor
		/// </summary>
		public ForgotPasswordController(IFormsRepository formsRepository, ILockedAccountService lockedAccountService) {
			_formsRepository = formsRepository;
			_lockedAccountService = lockedAccountService;
		}


		public ActionResult ForgotPassword() {
			var username = Request.Params["username"];
			var retrievePasswordModel = new RetrievePasswordModel();
			retrievePasswordModel.Initialize(RenderingContext.Current.Rendering);
			if (string.IsNullOrEmpty(username)) {
				return View("/Views/Forms/ForgotPassword.cshtml", retrievePasswordModel);
			} else {
				retrievePasswordModel.UserName = username;
				_lockedAccountService.HandleLockedAccount(username,
					RenderingContext.Current.Rendering.Item.Fields[Templates.RetrievePassword.Fields.CancelLink],
					RenderingContext.Current.Rendering.Item.Fields[Templates.RetrievePassword.Fields.ResetLink],
					out var isUsernameValid);
				retrievePasswordModel.IsUsernameValid = isUsernameValid;
				retrievePasswordModel.Submitted = true;
				return View("/Views/Forms/ForgotPassword.cshtml", retrievePasswordModel);
			}

		}

        [HttpPost]
        [ValidateFormHandler]

        public ActionResult ForgotPassword(RetrievePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _lockedAccountService.HandleLockedAccount(model.UserName,
                RenderingContext.Current.Rendering.Item.Fields[Templates.RetrievePassword.Fields.CancelLink],
                RenderingContext.Current.Rendering.Item.Fields[Templates.RetrievePassword.Fields.ResetLink],
                out var isUsernameValid);
                
				model.IsUsernameValid = isUsernameValid;
                model.Initialize(RenderingContext.Current.Rendering);
                model.Submitted = true;
				if (response == LoginErrors.ACCOUNT_LOCKED_NOT_SENT_MAIL)
				{
					model.SendEmail = false;
				}
				else
				{
					model.SendEmail = true;
				}
                return View("/Views/Forms/ForgotPassword.cshtml", model);
            }
            else
            {
                model.Initialize(RenderingContext.Current.Rendering);
                var modelStateVal = ViewData.ModelState[nameof(model.UserName)];
                model.HasErrorEmail = modelStateVal.Errors.Count > 0;
                return View("/Views/Forms/ForgotPassword.cshtml", model);
            }
        }

        [HttpPost]
		[ValidateFormHandler]
		
		public ActionResult ForgotPasswordRedirect(RetrievePasswordModel model) {
			var itemForgot = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(model.ItemId));

			var redirectUrl = LinkManager.GetItemUrl(itemForgot);

			return Redirect(redirectUrl + "?username=" + model.UserName);
		}
	}
}