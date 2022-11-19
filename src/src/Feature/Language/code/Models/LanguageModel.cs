namespace Neambc.Seiumb.Feature.Language.Models
{
    /// <summary>
    /// Model with the information of the Language configured in Sitecore
    /// </summary>
    public class LanguageModel
    {
        public string NativeName { get; set; }
        public string Url { get; set; }
        public string RedirectUrl { get; set; }
        public string TwoLetterCode { get; set; }
        public string Name { get; set; }
        public string OnClickGTMContent { get; set; }
    }
}