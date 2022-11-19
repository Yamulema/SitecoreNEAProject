using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Feature.GeneralContent.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Sitecore.Diagnostics;
using Neambc.Neamb.Foundation.Configuration.Manager;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class ContactUsController : BaseController
    {
		private readonly ISessionAuthenticationManager _sessionManager;
        private readonly IContactUsRepository _contactUsRepository;
        private readonly ICacheManager _cacheManager;
	    private readonly ICaptchaManager _captchaManager;
		private readonly IContactUsManager _contactUsManager;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		public ContactUsController(ISessionAuthenticationManager sessionManager, IContactUsRepository contactUsRepository, ICacheManager cacheManager, ICaptchaManager captchaManager, IContactUsManager contactUsManager, IGlobalConfigurationManager globalConfigurationManager)
        {
            _contactUsRepository = contactUsRepository;
			_sessionManager = sessionManager;
            _cacheManager = cacheManager;
	        _captchaManager = captchaManager;
			_contactUsManager = contactUsManager;
			_globalConfigurationManager = globalConfigurationManager;
		}
        public ActionResult ContactUs()
        {
            return View("/Views/Neamb.GeneralContent/ContactUs/ContactUs.cshtml", CreateModel());
        }

        private ContactUsDTO CreateModel()
        {
            var contactUsDTO = new ContactUsDTO(_sessionManager, _cacheManager);
            contactUsDTO.Initialize(RenderingContext.Current.Rendering);
            contactUsDTO.LoadFields();
            return contactUsDTO ;
        }

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult ContactUs(ContactUsDTO model, FormCollection formCollection)
		{
			Log.Info($"Starting calling the Recaptcha verification first name: {model.FirstName}, last name: {model.LastName}, email: {model.Email}, state: {model.StateAffiliate}, topic:{model.Topic}, message: {model.Message}",this);
			var resultCaptcha= _captchaManager.ExecutePostRecaptcha(formCollection["g-recaptcha-response"], _globalConfigurationManager.CatpchaSecret);
			Log.Info("Ending calling the Recaptcha verification", this);
			model._cacheManager = _cacheManager;
			model.Initialize(RenderingContext.Current.Rendering);

			if (resultCaptcha)
			{
				if (string.IsNullOrEmpty(model.Emailconfirmation))
				{
					_contactUsRepository.SubmitContactUs(ref model, ViewData);
					if (model.IsModelValid)
					{
						model.GtmAction = _contactUsManager.GetGtmAction(RenderingContext.Current.Rendering.Item);
						model.Topic = null;
						return View("/Views/Neamb.GeneralContent/ContactUs/ContactUs.cshtml", model);
					}
					else
					{
						return View("/Views/Neamb.GeneralContent/ContactUs/ContactUs.cshtml", model);
					}
				}
				else
				{
					model.IsModelValid = true;
					model.Topic = null;
					model.WasProcessedSuccessfully = true;
					return View("/Views/Neamb.GeneralContent/ContactUs/ContactUs.cshtml", model);
				}
			}
			else
			{
				model.HasCaptchaError = true;
				return View("/Views/Neamb.GeneralContent/ContactUs/ContactUs.cshtml", model);
			}
		}
    }
}