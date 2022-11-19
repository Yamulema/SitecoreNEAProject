using System.Web.Mvc;
using Neambc.Neamb.Feature.ClickAndSave.Model;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Pipelines;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.ClickAndSave.Controllers {
	public class LoginClickAndSaveController : BaseController {

		#region Fields
		private const string LoginClickSaveView = "/Views/Neamb.ClickAndSave/LoginClickAndSave.cshtml";
		private readonly IAuthenticationAccountManager _authenticationAccountManager;
		private readonly IEligibilityManager _eligibilityManager;
		private readonly IPipelineService _pipelineService;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly IProductManager _productManager;
		#endregion

		#region Constructor
		public LoginClickAndSaveController(
			IAuthenticationAccountManager authenticationAccountManager, IEligibilityManager eligibilityManager, IPipelineService pipelineService, IGlobalConfigurationManager globalConfigurationManager,
			IProductManager productManager
		) {
			_authenticationAccountManager = authenticationAccountManager;
			_eligibilityManager = eligibilityManager;
			_pipelineService = pipelineService;
			_globalConfigurationManager = globalConfigurationManager;
			_productManager = productManager;
		}
		#endregion

		#region Public Methods
		public ActionResult LoginClickAndSave() {
			var model = new LoginClickAndSaveDto();
			model.Initialize(RenderingContext.Current.Rendering);
			var mdsidParameter = Request.QueryString[ConstantsNeamb.Mdsid];
			var redirectUrlParameter = Request.QueryString[ConstantsNeamb.RedirectUrl];
			LinkField destinationLinkField = model.Item.Fields[Templates.LoginFormClickAndSave.Fields.DefaultClickSaveUrl];
			var urlForRedirection = destinationLinkField.TargetItem != null ? LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(destinationLinkField.TargetItem.ID)) : LinkManager.GetItemUrl(model.Item);

			if (!string.IsNullOrEmpty(mdsidParameter) && !string.IsNullOrEmpty(redirectUrlParameter)) {
				model.Mdsid = mdsidParameter;
				model.RedirectUrl = redirectUrlParameter;
				//Verify the mdsid exists and it is registered
				var account = new AccountMembership();
				_authenticationAccountManager.RetrieveAccount(account, mdsidParameter);
				if (account.Status == StatusEnum.Cold || account.Status == StatusEnum.Unknown || account.Status == StatusEnum.WarmCold) {
					//User is not registered
					return Redirect(urlForRedirection);
				} else {
					model.UserName = account.Username;
				}
			} else {
				//Error in query parameters in url
				return Redirect(urlForRedirection);
			}
			return View(LoginClickSaveView, model);
		}

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult LoginClickAndSave(LoginClickAndSaveDto model) {
			model.Initialize(RenderingContext.Current.Rendering);
			model.HasErrorPassword = ViewData.ModelState["Password"].Errors.Count > 0;
			var redirectUrlParameter = Request.QueryString[ConstantsNeamb.RedirectUrl];
			if (ModelState.IsValid) {
				//authenticate username and password
				var account = new AccountMembership();
				var responseLogin =
					_authenticationAccountManager.AuthenticateAccount(model.UserName, model.Password, account, string.Empty);
				if (account.Status == StatusEnum.Hot || account.Status == StatusEnum.WarmCold) {
					model.IsValid = true;
					//Check product eligibility
					var resultEligibility = _eligibilityManager.IsMemberEligible(account.Mdsid, "486 01");
					if (resultEligibility != EligibilityResultEnum.Eligible) {
						model.HasErrorEligible = true;
					} else {
						var accountUser = _productManager.GetAccountUser(account);
						var resultPipeline = _pipelineService.RunProcessPipelines(_globalConfigurationManager.ClickAndSaveProductCode, accountUser, ConstantsNeamb.PipelineNameDatapass, redirectUrlParameter);
						var resultRedirect = resultPipeline.ActionPrimary;
						return Redirect(resultRedirect);
					}
				} else {
					LinkField resetLink =
						model.Item.Fields[Templates.LoginFormClickAndSave.Fields.PasswordReset];
					var options = new ItemUrlBuilderOptions
					{
						AlwaysIncludeServerUrl = true,  
						
					};
					var pathReset = resetLink != null && resetLink.TargetItem != null
						? LinkManager.GetItemUrl(resetLink.TargetItem, options)
						: string.Empty;
                    _authenticationAccountManager.ProcessErrorAuthentication(responseLogin, account, model.UserName, pathReset);

					//Set the error according the user account status
					switch (account.Status) {
						case StatusEnum.Unknown:
						case StatusEnum.Cold: {
							model.HasErrorInvalidCredentials = true;
							break;
						}
						case StatusEnum.LockedNewToken: {
							model.HasLockedError = true;
							break;
						}
						case StatusEnum.LockedOldToken: {
							model.HasAlreadyLockedTokenValidError = true;
							break;
						}
						default: {
							Sitecore.Diagnostics.Log.Info("Default condition " + account.Status, this);
							model.HasErrorTimeout = true;
							break;
						}
					}
				}
			}
			return View(LoginClickSaveView, model);
		}
		#endregion
	}
}