using Neambc.Seiumb.Feature.Language.Models;
using System;
using System.Collections.Generic;

namespace Neambc.Seiumb.Feature.Language.Repositories
{
    public interface ILanguageRepository
    {
        IEnumerable<LanguageModel> GetAllLanguages();
        LanguageModel GetActiveLanguage();
        void SetLanguage(string languageName, DateTime ExpirationDate);
        LanguageModel CreateLanguage(Sitecore.Globalization.Language language);
    }
}
