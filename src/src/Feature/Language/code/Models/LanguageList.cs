using System.Collections.Generic;

namespace Neambc.Seiumb.Feature.Language.Models
{
    public class LanguageList
    {
        public List<LanguageModel> Languages { get; set; }
        public LanguageModel ActiveLanguage { get; set; }
        //public List<string> OnClickEventContent { get; set; }
    }
}