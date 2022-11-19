using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Models;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class PassThroughAuthenticationController : BaseController {
		private readonly IPassthroughService _passthroughService;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        public PassThroughAuthenticationController(IPassthroughService passthroughService, ISeiumbProfileManager seiumbProfileManager) {
            _passthroughService = passthroughService;
            _seiumbProfileManager = seiumbProfileManager;
        }

		public ActionResult PassThroughAuthentication() {
			var model = new PassThroughAuthenticationModel();
            var seiuProfile = _seiumbProfileManager.GetProfile();
            model.Initialize(RenderingContext.Current.Rendering);
			model.UserName = seiuProfile.Email;
			model.Mdsid = seiuProfile.MdsId;
			model.QueryString = seiuProfile.QueryString;
			model.ProductCode = model.Item.Fields[Templates.PassThroughAuthenticationTemplate.Fields.ProductCode].Value;
			LinkField destinationLinkField = model.Item.Fields[Templates.PassThroughAuthenticationTemplate.Fields.DefaultClickSaveURL];
			var urlForRedirection = destinationLinkField.TargetItem != null ? 
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(destinationLinkField.TargetItem.ID)) : LinkManager.GetItemUrl(model.Item);
			if (string.IsNullOrEmpty(model.Mdsid) || string.IsNullOrEmpty(model.UserName)) //Error in query parameters in url
                return Redirect(urlForRedirection);
            return View("/Views/Forms/PassThroughAuthenticationForm.cshtml", model);

        }

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult PassThroughAuthentication(PassThroughAuthenticationModel model) {
			model.Initialize(RenderingContext.Current.Rendering);
			var passthroughRequest = new PassthroughRequest {
				Username = model.UserName,
				Password = model.Password,
				ProductCode = model.ProductCode,
				QueryStringParameters = GetQueryStringParameters(model.QueryString)
			};
			var response = _passthroughService.Authenticate(passthroughRequest);

            if (response.LoggedIn)
            {
                if (response.IsEligible) {
                    model.HasErrorEligible = true;
                    return Redirect(response.ReturnUrl);
                } else {
                    return View("/Views/Forms/PassThroughAuthenticationForm.cshtml", model);
                }
            } else {
                switch (response.Status)
                {
                    case LoginUserErrorCodeEnum.AccountLocked:
                        if (response.LoginErrors.Equals(LoginErrors.ACCOUNT_LOCKED_NOT_SENT_MAIL))
                        {
                            model.HasAlreadyLockedError = true;
                        }
                        else if (response.LoginErrors.Equals(LoginErrors.ACCOUNT_LOCKED_SENT_MAIL))
                        {
                            model.HasLockedError = true;
                        }
                        return View("/Views/Forms/PassThroughAuthenticationForm.cshtml", model);
                    case LoginUserErrorCodeEnum.FailedLogin:
                        model.HasErrorInvalidCredentials = true;
                        return View("/Views/Forms/PassThroughAuthenticationForm.cshtml", model);
                    case LoginUserErrorCodeEnum.UsernameNotExist:
                        model.HasErrorInvalidUser = true;
                        return View("/Views/Forms/PassThroughAuthenticationForm.cshtml", model);
                    default:
                        model.HasErrorTimeout = true;
                        return View("/Views/Forms/PassThroughAuthenticationForm.cshtml", model);
                }
            }
		}

		private Dictionary<string, string> GetQueryStringParameters(string QueryString) {
			var QueryStringParameters = new Dictionary<string, string>();
			var parameters = QueryString.Split('&');
			foreach (var parameter in parameters) {
				var query = parameter.Split('=');
				QueryStringParameters.Add(query[0], query[1]);
			}
			return QueryStringParameters;
		}
	}
}