using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Neambc.Seiumb.Foundation.Indexing
{
    public class BaseModel : SearchResultItem
    {
        [IndexField("_template")]
        public ID _TemplateID { get; set; }

        [IndexField("_fullpath")]
        public string _FullPath { get; set; }

        [IndexField("friendlyUrl")]
        public string FriendlyUrl { get; set; }
    }
}