using Neambc.Neamb.Feature.Language.Managers;
using Neambc.Neamb.Feature.Language.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;

namespace Neambc.Neamb.Feature.Language.Controllers
{
    public class SpanishLanguageModalController : BaseController
    {
        private readonly ICookieManager _cookieManager;
        public SpanishLanguageModalController(ICookieManager cookieManager)
        {
            _cookieManager = cookieManager;
        }
        public ActionResult SpanishLanguageModal()
        {
            return View("/Views/Neamb.Language/SpanishLanguageModal.cshtml", CreateModel());
        }

        private SpanishLanguageModalDTO CreateModel()
        {
            var spanishLanguageModalDTO = new SpanishLanguageModalDTO(Request.UserLanguages, _cookieManager);
            spanishLanguageModalDTO.Initialize(RenderingContext.Current.Rendering);
            return spanishLanguageModalDTO;
        }
    }
}