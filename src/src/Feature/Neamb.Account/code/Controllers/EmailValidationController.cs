using System;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Feature.Account.Controllers
{
    public class EmailValidationController : BaseController
    {
        private readonly IEmailValidationManager _emailValidationManager;

        public EmailValidationController(IEmailValidationManager emailValidationManager)
        {
            _emailValidationManager = emailValidationManager;
        }


        /// <summary>
        /// API to check NEAMB validations for emails
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <param name="validateUsername">True to check if email should be verify against users registered - already registered</param>
        /// <param name="invalidMailError">Error message to show when invalid domain error is found, required for jquery plugin validator in NEAMB</param>
        /// <param name="registeredUserError">Error message to show when email is already registed, required for jquery plugin validator in NEAMB</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult EmailValidationRules(string email, bool? validateUsername, string invalidMailError, string registeredUserError,
            string reservedDomainError)
        {
            try
            {
                var validationResult = _emailValidationManager.IsValid(email, validateUsername);

                if (validationResult.IsValid) {
                    return Json(true, JsonRequestBehavior.AllowGet);
                } else {
                    //if comes from NEAMB, send error messages, SEIUMB not required
                    if (!string.IsNullOrEmpty(invalidMailError))
                    {
                        var errorType = validationResult.ErrorMessage;
                        if (errorType == "EmailDomainError")
                        {
                            validationResult.ErrorMessage = invalidMailError;
                        }
                        if (errorType == "EmailReservedDomainError")
                        {
                            validationResult.ErrorMessage = reservedDomainError;
                        }
                        else if (errorType == "EmailAlreadyRegistered")
                        {
                            var fullRegisteredUserError = registeredUserError.Replace("{0}", email);
                            validationResult.ErrorMessage = fullRegisteredUserError;
                        }
                    }
                    return Json(validationResult.ErrorMessage, JsonRequestBehavior.AllowGet);
                }      
            }
            catch (Exception e)
            {
                Log.Error($"Error while trying to validate email:{email} with IsValid method.", e, this);
                return Json("Internal server error.");
            }
        }
    }
}