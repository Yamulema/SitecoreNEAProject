using Neambc.Neamb.Feature.Language.Managers;
using Neambc.Neamb.Feature.Language.Models;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using System;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Language.Controllers
{
    public class LanguageToggleController : BaseController
    {
        public ICookieManager _cookieManager { get; set; }
        private const int EXPIRATION_YEARS_IN_CACHE = 10;
        public LanguageToggleController(ICookieManager cookieManager)
        {
            _cookieManager = cookieManager;
        }
        public ActionResult LanguageToggle()
        {
            return View("/Views/Neamb.Language/LanguageToggle.cshtml", CreateModel());
        }

        private LanguageToggleDTO CreateModel()
        {
            var languageToggleDTO = new LanguageToggleDTO();
            languageToggleDTO.Initialize(RenderingContext.Current.Rendering);
            SetLanguage(languageToggleDTO);
            SetRedirect(languageToggleDTO);
            return languageToggleDTO;
        }

        private void SetRedirect(LanguageToggleDTO languageToggleDTO)
        {
            var activeLanguagefield = RenderingContext.Current.Rendering.Item.Fields[Templates.LanguageToggle.Fields.Default].Value;
			ID.TryParse(activeLanguagefield, out var defaultID);
			if (languageToggleDTO.SelectedLanguage != null && languageToggleDTO.SelectedLanguage == languageToggleDTO.EnglishName && defaultID == Items.Spanish)
            {
                languageToggleDTO.RedirectEnglish = true;
                languageToggleDTO.RedirectSpanish = false;
            }
            else if (languageToggleDTO.SelectedLanguage != null && languageToggleDTO.SelectedLanguage == languageToggleDTO.SpanishName && defaultID == Items.English)
            {
                languageToggleDTO.RedirectEnglish = false;
                languageToggleDTO.RedirectSpanish = true;
            }
        }

        private void SetLanguage(LanguageToggleDTO languageToggleDTO)
        {
            if (_cookieManager.Exists(GetKey()))
            {
                languageToggleDTO.SelectedLanguage = HttpUtility.UrlDecode(_cookieManager.GetCookie(GetKey()).Value);
            }
        }

        public ActionResult English(string itemid, string language)
        {
            StoreInCache(language);
            if (!String.IsNullOrEmpty(itemid) && !String.IsNullOrEmpty(language))
            {
                var item = Context.Database.GetItem(new ID(itemid));
                var url = GetUrl(Templates.LanguageToggle.Fields.English, item);
                return Redirect(url);
            } else {
                return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.Home.ID)));
            }
        }
        public ActionResult Spanish(string itemid, string language)
        {
            if (!String.IsNullOrEmpty(itemid) && !String.IsNullOrEmpty(language))
            {
                StoreInCache(language);
            
                var item = Context.Database.GetItem(new ID(itemid));
                var url = GetUrl(Templates.LanguageToggle.Fields.Spanish, item);
                return Redirect(url);
            }
            else
            {
                return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.Home.ID)));
            }
        }

        private string GetUrl(ID language, Item item)
        {
            string url = null;
            LinkField languageField = item.Fields[language];
            if (languageField?.TargetItem != null)
            {
                url = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(languageField.TargetItem.ID));
            }
            return url;
        }

        private void StoreInCache(string language)
        {
            _cookieManager.CreateCookie(GetKey(), HttpUtility.UrlEncode(language), TimeSpan.FromDays(365 * EXPIRATION_YEARS_IN_CACHE));
        }
        private string GetKey()
        {
            return ConstantsNeamb.LanguageToggleCookieName;
        }
    }
}