using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Indexing.Models
{
    public class SuggestionPageResult : BaseModel
    {
        [IndexField("page_title_t")]
        public string PageTitle { get; set; }
    }
}