using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services.ForgotUserName;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers
{
    public class ForgotUsernameController : BaseController
    {

        private const string FORGOT_USERNAME_VIEW = "/Views/Neamb.Account/ForgotUsername.cshtml";
        private readonly IAccountRepository _accountRepository;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IForgotUserNameService _forgotUserNameService;

        public ForgotUsernameController(
            IAccountServiceProxy serviceManager, IAccountRepository accountRepository,
            IGlobalConfigurationManager globalConfigurationManager, IForgotUserNameService forgotUserNameService)
        {
            _accountRepository = accountRepository;
            _globalConfigurationManager = globalConfigurationManager;
            _forgotUserNameService = forgotUserNameService;

        }

        /// <summary>
        /// Form Reset Username
        /// </summary>
        /// <returns>View</returns>
        public ActionResult ForgotUsername()
        {
            var model = new ForgotUsernameDTO();
            model.Initialize(RenderingContext.Current.Rendering);
            var contextItem = RenderingContext.Current.Rendering.Item;
            SetModelValue(model, contextItem);

            return View(FORGOT_USERNAME_VIEW, model);
        }

        /// <summary>
        /// Fill some values in the model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="contextItem"></param>
        private void SetModelValue(ForgotUsernameDTO model, Item contextItem)
        {
            model.HasTooltipFirstName = !string.IsNullOrEmpty(contextItem[Templates.RetrieveUsername.Fields.FirstNameTooltip]);
            model.HasTooltipLastName = !string.IsNullOrEmpty(contextItem[Templates.RetrieveUsername.Fields.LastNameTooltip]);
            model.HasTooltipBirthDate = !string.IsNullOrEmpty(contextItem[Templates.RetrieveUsername.Fields.BirthdateTooltip]);
            model.HasTooltipZip = !string.IsNullOrEmpty(contextItem[Templates.RetrieveUsername.Fields.ZipTooltip]);
        }

        /// <summary>
        /// Form Reset Username Post
        /// </summary>
        /// <param name="model">data entered by the user</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateFormHandler]
        public ActionResult ForgotUsername(ForgotUsernameDTO model)
        {
            model.Initialize(RenderingContext.Current.Rendering);
            model.BirthDate = string.Format("{0}{1}{2}", model.Month, model.Day, model.Year);
            var contextItem = RenderingContext.Current.Rendering.Item;
            SetModelValue(model, contextItem);

            if (string.IsNullOrEmpty(model.Zipconfirmation))
            {
                //Verify the date birth errors
                var customErrorsBirth = _accountRepository.HasDateBirthCustomValidationErrors(model);

                if (ModelState.IsValid && !customErrorsBirth)
                {
                    //Call the webservice to retrieve the username
                    var resultService = _forgotUserNameService.ForgotUserNameStatus(model.FirstName, model.LastName, model.Zip, model.BirthDate, Convert.ToInt32(_globalConfigurationManager.Unionid));
                    if (resultService != null && resultService.Success)
                    {
                        if (resultService.Data.matchFound == true)
                        {
                            //Case processed successfully
                            model.ProcessedSucessfully = true;
                            model.Username = resultService.Data.username;
                            LinkField retrievePasswordLink = contextItem.Fields[Templates.RetrieveUsername.Fields.RetrievePassword];
                            model.TextResetPassword = retrievePasswordLink.Text;
                            model.PathResetPassword = string.Format("{0}?{1}={2}", LinkManager.GetItemUrl(retrievePasswordLink.TargetItem),
                                ConstantsNeamb.UsernamePar, resultService.Data.username);
                        }
                        else
                        {
                            model.HasErrorUserName = true;
                        }
                    }
                    else
                    {
                        model.HasGeneralError = true;
                    }

                }
                else
                {
                    _accountRepository.SetErrorUserBasicData(model, ViewData);
                }

            }
            else
            {
                model.HasErrorUserName = true;
            }
            return View(FORGOT_USERNAME_VIEW, model);
        }
    }
}