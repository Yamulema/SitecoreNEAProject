using Neambc.Seiumb.Foundation.Indexing.PageTypes;
using Neambc.Seiumb.Foundation.Indexing.Repository;
using System.Collections.Generic;

namespace Neambc.Seiumb.Foundation.Indexing.Managers
{
    public static class SearchManager
    {
        public static IEnumerable<SearchResult> GlobalSearch(string searchText)
        {
            return GlobalSearchRepository.Instance.GetSearchResult(searchText);
        }
    }
}