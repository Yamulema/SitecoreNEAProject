using System;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Language.Infrastructure.Pipelines;
using Neambc.Seiumb.Feature.Language.Models;
using Neambc.Seiumb.Feature.Language.Repositories;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore.Pipelines;

namespace Neambc.Seiumb.Feature.Language.Controllers {
	public class LanguageController : BaseController {

        private readonly ILanguageRepository _languageRepository;
        private readonly ILog _log;

        public LanguageController(ILanguageRepository languageRepository, ILog log)
        {
            _languageRepository = languageRepository;
            _log = log;
        }

        public ActionResult LanguageSelector() {
            var result = new LanguageList
            {
                Languages = _languageRepository.GetAllLanguages().ToList(),
                ActiveLanguage = _languageRepository.GetActiveLanguage()
            };

            foreach (var language in result.Languages) {
                if (Request != null && Request?.QueryString?.Keys.Count > 0)
                    language.Url = $"{language.Url}?{Request.QueryString}";

                if (Request?.QueryString?["id"] != null)
                    language.RedirectUrl = language.Url;

            }
            return View("/Views/Language/Renderings/LanguageSelector.cshtml", result);
        }

        public ActionResult LanguageSelectorMobile()
        {
            var result = new LanguageList
            {
                Languages = _languageRepository.GetAllLanguages().ToList(),
                ActiveLanguage = _languageRepository.GetActiveLanguage()
            };

            foreach (var language in result.Languages)
            {
                if (Request != null && Request?.QueryString?.Keys.Count > 0)
                    language.Url = $"{language.Url}?{Request.QueryString}";

                if (Request?.QueryString?["id"] != null)
                    language.RedirectUrl = language.Url;

            }
            return View("/Views/Language/Renderings/LanguageSelectorMobile.cshtml", result);
        }

        /// <summary>
        /// Function when the user changes the language in the browser
        /// </summary>
        /// <param name="newLanguage">New language selected by the user</param>
        /// <param name="currentLanguage">Current language selected in the screen</param>
        /// <returns></returns>
        [HttpPost]
		
		public JsonResult ChangeLanguage(string newLanguage, string currentLanguage) {
			_log.Info("Start Setting Language-" + DateTime.Now, this);

			var args = new ChangeLanguagePipelineArgs(currentLanguage, newLanguage);
			CorePipeline.Run("language.changeLanguage", args, false);
			//Set the expiration date the current date + 1 day the Cookie of Context.Language in Sitecore
			var expirationDate = DateTime.Now.AddDays(1);
            _languageRepository.SetLanguage(args.NewLanguage, expirationDate);
			return new JsonResult { Data = args.CustomData };
		}
	}
}