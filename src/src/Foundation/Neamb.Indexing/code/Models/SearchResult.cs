using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Indexing.Models
{
    public class SearchResult : BaseModel
    {
        [IndexField("include_in_search_results_b")]
        public bool _IncludeInSearchResults { get; set; }
        [IndexField("page_title_t")]
        public string _PageTitle { get; set; }
        [IndexField("short_description_t")]
        public string _ShortDescription { get; set; }
        [IndexField("body_t")]
        public string _Body { get; set; }
        [IndexField("smallThumbnailUrl")]
        public string _SmallThumbnailUrl { get; set; }
        [IndexField("genre_t")]
        public string Genre { get; set; }
        [IndexField("goal_sm")]
        public List<Guid> _Goals { get; set; }
        [IndexField("topics_sm")]
        public List<Guid> _Topics { get; set; }
        [IndexField("product_category_sm")]
        public List<Guid> _ProductCategory { get; set; }
        [IndexField("life_events_sm")]
        public List<Guid> _LifeEvents { get; set; }
        [IndexField("seasonality_sm")]
        public List<Guid> _Seasonality { get; set; }
        [IndexField("last_publish_date_tdt")]
        public DateTime _LastPublishDate { get; set; }
        [IndexField("short_description_html_s")]
        public string _ShortDescriptionHtml { get; set; }
    }
}