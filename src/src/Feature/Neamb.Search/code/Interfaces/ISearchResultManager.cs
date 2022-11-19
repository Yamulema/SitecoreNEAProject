using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Search.Models;

namespace Neambc.Neamb.Feature.Search.Interfaces
{
    public interface ISearchResultManager
    {
        IEnumerable<SuggestionResult> GetSuggestions(string term, int? take);
        Tuple<IEnumerable<SearchResultCard>, int> GetSearchResultCards(IEnumerable<string> terms, int? take, int? skip, List<string> genre);
    }
}