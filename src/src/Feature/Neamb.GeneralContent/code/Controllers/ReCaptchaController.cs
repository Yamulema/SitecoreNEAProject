using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class RecaptchaController : BaseController
    {
	    private readonly ICaptchaManager _captchaManager;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;

		public RecaptchaController(ICaptchaManager captchaManager, IGlobalConfigurationManager globalConfigurationManager)
        {
	        _captchaManager = captchaManager;
			_globalConfigurationManager = globalConfigurationManager;
		}

		[HttpPost]
		public ActionResult ValidateUserToken(string responseToken)
		{
			if (!string.IsNullOrEmpty(responseToken)) {
				var resultCaptcha = _captchaManager.ExecutePostRecaptcha(responseToken, _globalConfigurationManager.CatpchaSecret);
				if (resultCaptcha)
				{
					return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
				}
			}
			return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
		}
    }
}