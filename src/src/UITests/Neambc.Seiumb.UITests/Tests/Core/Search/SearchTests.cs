using Neambc.Seiumb.UITests.Pages.Search;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Search
{
    [TestFixture]
    public class SearchTests : SeiumbTestBaseLarge<SearchPage>
    {

        [TestCaseSource(typeof(SearchTestData),
            nameof(SearchTestData.TestDataSource),
            new object[] { "Test_GTM_SearchText" })]
        [Test, Category("GTM")]
        public void ValidateGTMSearchBar(string searchText)
        {
            Page.SearchText(searchText);
        }

        [TestCaseSource(typeof(SearchTestData),
           nameof(SearchTestData.TestDataSource),
           new object[] { "Test_GTM_SearchResults" })]
        [Test, Category("GTM")]
        public void ValidateGTMSearchResult(string url, string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText, string gtmUrlText)
        {
            Page.GoToPage<SearchPage>(url)
                .ValidateSearchResults(gtmDataLayer, gtmEvent, gtmModule, gtmText, gtmUrlText);
        }
    }
}
