using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.ValidateResetToken;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers
{
    public class ResetPasswordController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountManager _accountManager;
        private readonly IBase64Service _base64Service;
        private readonly IValidateResetTokenService _validateResetTokenService;

        public ResetPasswordController(IAccountRepository accountRepository,
            IAccountManager accountManager, IBase64Service base64Service, IValidateResetTokenService validateResetTokenService)
        {
            _accountRepository = accountRepository;
            _accountManager = accountManager;
            _base64Service = base64Service;
            _validateResetTokenService = validateResetTokenService;
        }

        /// <summary>
        /// Form Reset Password
        /// </summary>
        /// <returns>View</returns>
        public ActionResult ResetPassword()
        {
            var resetPasswordDto = new ResetPasswordDTO();
            resetPasswordDto.Initialize(RenderingContext.Current.Rendering);
            resetPasswordDto.HasTooltipPassword = !string.IsNullOrEmpty(RenderingContext.Current.Rendering.Item[Templates.Password.Fields.Tooltip]);
            var paramId = Request.QueryString[ConstantsNeamb.ParamId];
            var paramToken = Request.QueryString[ConstantsNeamb.ParamS];
            if (!string.IsNullOrEmpty(paramId) && !string.IsNullOrEmpty(paramToken))
            {
                var decodedParamId = _base64Service.Decode(paramId);
                var isTokenValid = _validateResetTokenService.ValidateResetToken(decodedParamId, paramToken, (int)Union.NEA);
                if (isTokenValid)
                {
                    resetPasswordDto.HasTokenValid = true;
                    resetPasswordDto.Username = decodedParamId;
                    resetPasswordDto.Token = paramToken;
                }
                else
                {
                    resetPasswordDto.HasTokenValid = false;
                }
            }

            return View("/Views/Neamb.Account/ResetPassword.cshtml", resetPasswordDto);
        }

        /// <summary>
        /// Post Reset password form
        /// </summary>
        /// <param name="username">Email</param>
        /// <param name="password">New password</param>
        /// <param name="confirmPassword">Password</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateFormHandler]

        public ActionResult ResetPassword(string username, string password, string confirmPassword, string token)
        {
            var model = new ResetPasswordDTO();

            model.Initialize(RenderingContext.Current.Rendering);
            model.Username = username;
            model.Token = token;
            model.RedirectPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));

            var isTokenValid = _validateResetTokenService.ValidateResetToken(username, token, (int)Union.NEA);
            if (isTokenValid)
            {
                model.PasswordData.Password = password;
                model.PasswordData.ConfirmPassword = confirmPassword;
                model.HasTokenValid = true;
                model.Username = username;
                model.RedirectPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));

                var hasErrors = false;
                if (string.IsNullOrEmpty(password))
                {
                    model.PasswordData.ErrorsPassword.Add(ErrorStatusEnum.Required);
                    hasErrors = true;
                }

                if (string.IsNullOrEmpty(confirmPassword))
                {
                    model.PasswordData.ErrorsConfirmPassword.Add(ErrorStatusEnum.Required);
                    hasErrors = true;
                }

                var customPasswordErrors = _accountRepository.HasPasswordCustomValidationErrors(model.PasswordData);
                if (!hasErrors && !customPasswordErrors)
                {
                    var resetPwdResponse = _accountManager.ResetPassword(username, password, confirmPassword, (int)Union.NEA);

                    if (resetPwdResponse)
                        model.ProcessedSucessfully = true;
                    else
                        model.HasGeneralError = true;
                }
            }
            else
            {
                model.HasGeneralError = true;
            }
            return View("/Views/Neamb.Account/ResetPassword.cshtml", model);
        }
    }
}