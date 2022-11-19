using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Links;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers
{
    public class WelcomeController : SitecoreController
    {
        #region Private Attributes
        private readonly IWelcomeManager _welcomeManager;
        #endregion

        #region Constructor
        public WelcomeController(IWelcomeManager welcomeManager)
        {
            _welcomeManager = welcomeManager;
        }
        #endregion

        #region Member Welcome
        public ActionResult MemberWelcome()
        {
            var datasource = RenderingContext.Current.Rendering.Item;
            var model = _welcomeManager.MemberWelcomeModel(datasource);

            return View("/Views/Neamb.Account/Welcome/MemberWelcome.cshtml", model);
        }

        [HttpPost]
        [ValidateFormHandler]

        public ActionResult MemberWelcome(MemberWelcome memberWelcome)
        {
            memberWelcome.Item = RenderingContext.Current.Rendering.Item;
            var model = _welcomeManager.MemberWelcomeRegister(memberWelcome);
            if (model.Status == WelcomeStatus.None)
            {
                return View("/Views/Neamb.Account/Welcome/MemberWelcome.cshtml", model);
            }
            else
            {
                var url =
                    $"{LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.MemberVerification))}?{ConstantsNeamb.Ref}={memberWelcome.Mdsid}";
                return Redirect(url);
            }
        }
        #endregion

        #region Member Validation

        public ActionResult MemberVerification()
        {
            var datasource = RenderingContext.Current.Rendering.Item;
            var model = _welcomeManager.MemberVerificationModel(datasource);
            if (model.Status == WelcomeStatus.None && !Sitecore.Context.PageMode.IsExperienceEditor)
            {
                var url = $"{LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.MemberWelcome))}";
                return Redirect(url);
            }
            else
            {
                return View("/Views/Neamb.Account/Welcome/MemberVerification.cshtml", model);
            }
        }

        [HttpPost]
        [ValidateFormHandler]

        public ActionResult MemberVerification(MemberVerification model)
        {
            model.Item = RenderingContext.Current.Rendering.Item;
            switch (model.Action)
            {
                case WelcomeAction.VerifyZip:
                    _welcomeManager.VerifyZip(ref model, ViewData.ModelState);
                    if (model.WasProcessed)
                    {
                        return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.RegistrationPage.ID)));
                    }
                    else
                    {
                        //If the attempt number is greater than 3 redirect logout the user and redirect to the registration page
                        if (model.ErrorStatus.HasFlag(WelcomeErrorStatus.TooManyAttempts))
                        {
                            return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.RegistrationPage.ID)));
                        }
                        return View("/Views/Neamb.Account/Welcome/MemberVerification.cshtml", model);
                    }
                case WelcomeAction.VerifyPassword:
                    var resultAuthentication = _welcomeManager.VerifyPassword(ref model, ViewData);
                    switch (resultAuthentication)
                    {
                        case AuthenticationResultEnum.Valid:
                            {
                                return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID)));
                            }
                        case AuthenticationResultEnum.Duplicated:
                            {
                                return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID)));
                            }
                        default:
                            {
                                return View("/Views/Neamb.Account/Welcome/MemberVerification.cshtml", model);
                            }
                    }
                default:
                    return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.MemberVerification)));
            }
        }
        #endregion
    }
}