using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Feature.Language.Models;
using Neambc.Seiumb.Foundation.Analytics.GTM;
using Neambc.Seiumb.Foundation.Analytics.GTM.Models;
using Sitecore;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;

namespace Neambc.Seiumb.Feature.Language.Repositories
{
    [Service(typeof(ILanguageRepository))]
    public class LanguageRepository : ILanguageRepository
    {
        private readonly IGTMServiceSeiumb _gtmService;

        public LanguageRepository(IGTMServiceSeiumb gtmService)
        {
            _gtmService = gtmService;
        }

        /// <summary>
        /// Get all languages
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LanguageModel> GetAllLanguages()
        {
            var languages = Context.Database.GetLanguages();
            return languages.Select(CreateLanguage).OrderBy(item => item.Name);
        }

        /// <summary>
        /// Get the active language
        /// </summary>
        /// <returns></returns>
        public LanguageModel GetActiveLanguage()
        {
            return CreateLanguage(Context.Language);
        }

        /// <summary>
        /// Sets the current language.
        /// This to extend the sitecore default SetLanguage behavior by adding expiration date for the language cookie
        /// </summary>

        /// <param name="languageName">The language Name.</param>
        /// <param name="ExpirationDate">The cookie expiration date, persistent should be set to <c>true</c> too </param>
        public void SetLanguage(string languageName, DateTime ExpirationDate)
        {
            var site = Context.Site;

            if (site != null)
            {
                var cookieKey = site.GetCookieKey("lang");
                var languageSelected = Sitecore.Web.WebUtil.GetCookieValue(cookieKey);
                if (string.IsNullOrEmpty(languageSelected))
                {
                    languageSelected = "en";
                }

                if (languageSelected != languageName)
                {
                    Sitecore.Web.WebUtil.SetCookieValue(cookieKey, languageName, ExpirationDate);
                }
            }
        }

        public LanguageModel CreateLanguage(Sitecore.Globalization.Language language)
        {
            //only used for language so that it doesn't change the schema, otherwise it switches it causing other issues.
            var options = new ItemUrlBuilderOptions
            {
                AlwaysIncludeServerUrl = false,  //not include the server
                LanguageEmbedding = LanguageEmbedding.Always,
                LowercaseUrls = true
            };

            return new LanguageModel
            {
                Name = language.Name,
                NativeName = GetEquivalenceLanguage(language.CultureInfo.TwoLetterISOLanguageName),
                Url = GetItemUrlByLanguageWithOptions(language, options),
                RedirectUrl = GetItemUrlByLanguageWithOptions(language, options),
                TwoLetterCode = language.CultureInfo.TwoLetterISOLanguageName,
                OnClickGTMContent = _gtmService.GetGtmEvent(new LanguageSeiumb
                {
                    Event = "language",
                    LanguageAction = "toggle " + GetEquivalenceLanguage(language.CultureInfo.TwoLetterISOLanguageName).ToLower(),
                    LanguageSelection = language.Name,
                })
            };
        }

        /// <summary>
        /// Get the url in a specific language
        /// </summary>
        /// <param name="language">Language selected by the user</param>
        /// <returns></returns>
        private string GetItemUrlByLanguage(Sitecore.Globalization.Language language)
        {
            using (new LanguageSwitcher(language))
            {
                var options = new ItemUrlBuilderOptions
                {
                    AlwaysIncludeServerUrl = true,
                    LanguageEmbedding = LanguageEmbedding.Always,
                    LowercaseUrls = true
                };
                var url = LinkManager.GetItemUrl(Context.Item, options);
                return StringUtil.EnsurePostfix('/', url).ToLower();
            }
        }

        /// <summary>
        /// Get the url in a specific language with options
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private string GetItemUrlByLanguageWithOptions(Sitecore.Globalization.Language language, ItemUrlBuilderOptions options)
        {
            using (new LanguageSwitcher(language))
            {
                var url = LinkManager.GetItemUrl(Context.Item, options);
                return StringUtil.EnsurePostfix('/', url).ToLower();
            }
        }

        /// <summary>
        /// Get the language name to be displayed in the screen
        /// </summary>
        /// <param name="isoLanguageName">Language iso name</param>
        /// <returns></returns>
        private string GetEquivalenceLanguage(string isoLanguageName)
        {
            switch (isoLanguageName)
            {
                case "en":
                    {
                        return "ENG";
                    }
                case "es":
                    {
                        return "ESP";
                    }
                default:
                    {
                        return isoLanguageName;
                    }
            }
        }
    }
}