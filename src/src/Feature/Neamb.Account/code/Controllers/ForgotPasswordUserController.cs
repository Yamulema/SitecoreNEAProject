using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Model.RestResponse;
using Neambc.Neamb.Foundation.MBCData.Services.CreateResetToken;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers {
	public class ForgotPasswordUserController : BaseController {

		private const string FORGOT_PASSWORD_VIEW = "/Views/Neamb.Account/ForgotPassword.cshtml";
		private readonly IAuthenticationAccountManager _authenticationAccountManager;
        private readonly ICreateResetTokenService _createResetTokenService;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;

		public ForgotPasswordUserController(IAuthenticationAccountManager authenticationAccountManager, ICreateResetTokenService createResetTokenService, IGlobalConfigurationManager globalConfigurationManager, ISessionAuthenticationManager sessionAuthenticationManager) {
			_authenticationAccountManager = authenticationAccountManager;
            _createResetTokenService = createResetTokenService;
			_globalConfigurationManager = globalConfigurationManager;
			_sessionAuthenticationManager = sessionAuthenticationManager;
		}

		/// <summary>
		/// Form Forgot Password
		/// </summary>
		/// <returns>View</returns>
		public ActionResult ForgotPasswordUser() {
			var forgotPasswordDto = new ForgotPasswordDTO();
			forgotPasswordDto.Initialize(RenderingContext.Current.Rendering);
			var contextItem = RenderingContext.Current.Rendering.Item;
			forgotPasswordDto.HasTooltipEmail = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.EmailTooltip]);
			var usernamePar = Request.QueryString[ConstantsNeamb.UsernamePar];
			if (!string.IsNullOrEmpty(usernamePar)) {
				forgotPasswordDto.Email = usernamePar;
				var serviceResponse = _createResetTokenService.CreateResetToken(usernamePar, Convert.ToInt32(_globalConfigurationManager.Unionid));
				if (serviceResponse.Success && serviceResponse.Data!= null && !string.IsNullOrEmpty(serviceResponse.Data.ResetToken)) {
					ProcessForgotPasswordProcess(forgotPasswordDto, contextItem, serviceResponse.Data.ResetToken, serviceResponse.Data.FirstName, serviceResponse.Data.ExpiresAt);
				}
			}

			return View(FORGOT_PASSWORD_VIEW, forgotPasswordDto);
		}

		/// <summary>
		/// Post Forgot password form
		/// </summary>
		/// <param name="model">data</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateFormHandler]
		public ActionResult ForgotPasswordUser(ForgotPasswordDTO model) {
			model.ReturnUrl= _sessionAuthenticationManager.GetRequestedPageLogin();
			
			var contextItem = RenderingContext.Current.Rendering.Item;
			model.Initialize(RenderingContext.Current.Rendering);
			if (string.IsNullOrEmpty(model.Emailconfirmation)) {
				if (ModelState.IsValid) {
					var serviceResponse = _createResetTokenService.CreateResetToken(model.Email, Convert.ToInt32(_globalConfigurationManager.Unionid));
                    if (serviceResponse.Success && serviceResponse.Data != null && !string.IsNullOrEmpty(serviceResponse.Data.ResetToken)) {
						if (serviceResponse.Data.NewToken) {
							ProcessForgotPasswordProcess(model, contextItem, serviceResponse.Data.ResetToken, serviceResponse.Data.FirstName, serviceResponse.Data.ExpiresAt);
						} else {
							ProcessForgotPasswordProcess(model, contextItem, serviceResponse.Data.ResetToken, serviceResponse.Data.FirstName, serviceResponse.Data.ExpiresAt, false);
						}
					}
					else if (serviceResponse.Error != null && serviceResponse.Error.Code ==(int)RestRestResponseErrorEnum.UsernameNoFound) {
						model.HasErrorEmailNotFound = true;
					}					
				} else {
					model.ErrorsEmail =
						ValidationFieldHelper.SetErrorsField(ViewData.ModelState[nameof(model.Email)], true, true, true);
				}
			} else {
				model.ProcessedSucessfully = true;
			}
			return View(FORGOT_PASSWORD_VIEW, model);
		}

		private void ProcessForgotPasswordProcess(ForgotPasswordDTO model, Item contextItem, string token, string firstName, string expiresAt, bool newToken=true) {
			var urlOptions = new ItemUrlBuilderOptions
			{
				AlwaysIncludeServerUrl = true,

			};
			Sitecore.Data.Fields.LinkField resetLink = contextItem.Fields[Templates.ForgotPassword.Fields.PasswordResetPage];
			var pathReset = resetLink != null && resetLink.TargetItem != null
				? LinkManager.GetItemUrl(resetLink.TargetItem, urlOptions)
				: string.Empty;
			Sitecore.Data.Fields.LinkField cancelLink = contextItem.Fields[Templates.ForgotPassword.Fields.PasswordDisavow];
			var pathCancel = cancelLink != null && cancelLink.TargetItem != null
				? LinkManager.GetItemUrl(cancelLink.TargetItem, urlOptions)
				: string.Empty;
			var result =
				_authenticationAccountManager.SendExactTargetResetEmail(
					new ExactTargetResetEmail
                    {
						UserName = model.Email,
						FirstName = firstName,
						Token = token,
						ResetPath = pathReset,
						CancelPath = pathCancel,
						ExpiresAt = expiresAt,
						NewToken = newToken,
						ResetPasswordEnum = ResetPasswordEnum.RequestedUser
					});
			if (result.ResultExactTarget) {
				model.ProcessedSucessfully = true;
			} else {
				model.HasGeneralError = true;
			}
		}
	}
}