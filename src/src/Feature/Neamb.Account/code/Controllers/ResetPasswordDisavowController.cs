using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Sitecore.Mvc.Presentation;
using System;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.CancelResetToken;

namespace Neambc.Neamb.Feature.Account.Controllers
{
    public class ResetPasswordDisavowController : BaseController
	{
	    private readonly IBase64Service _base64Service;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly ICancelResetTokenService _cancelResetTokenService;

        public ResetPasswordDisavowController(
			IAccountServiceProxy serviceManager, IBase64Service base64Service, IGlobalConfigurationManager globalConfigurationManager,
            ICancelResetTokenService cancelResetTokenService)
        {
            _base64Service = base64Service;
			_globalConfigurationManager = globalConfigurationManager;
            _cancelResetTokenService = cancelResetTokenService;

        }

		/// <summary>
		/// Form Reset Password Disavow
		/// </summary>
		/// <returns>View</returns>
		public ActionResult ResetPasswordDisavow()
		{
			var resetPasswordDisavowDto = new ResetPasswordDisavowDTO();
			resetPasswordDisavowDto.Initialize(RenderingContext.Current.Rendering);
			var paramId = Request.QueryString[ConstantsNeamb.ParamId];
			if (!string.IsNullOrEmpty(paramId))
			{
			    var decodedParamId = _base64Service.Decode(paramId);
                var result = _cancelResetTokenService.CancelResetToken(decodedParamId, Convert.ToInt32(_globalConfigurationManager.Unionid));
				if (result)
				{
					resetPasswordDisavowDto.ProcessedSucessfully = true;
				}
				else
				{
					resetPasswordDisavowDto.ProcessedSucessfully = false;
				}
			}
			else
			{
				resetPasswordDisavowDto.ProcessedSucessfully = false;
			}

			return View("/Views/Neamb.Account/ResetPasswordDisavow.cshtml", resetPasswordDisavowDto);
		}
	}
}