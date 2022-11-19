using Sitecore.ContentSearch;


namespace Neambc.Seiumb.Foundation.Indexing.PageTypes
{
    public class SearchResult: BaseModel
    {
        [IndexField("metatitle")]
        public string _Metatitle { get; set; }
        [IndexField("metadescription")]
        public string _MetaDescription { get; set; }
        [IndexField("includeinsearchresults")]
        public bool _IncludeInSearchResults { get; set; }
    }
}