using Neambc.Neamb.Project.Web.UITest.Pages.Search;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neambc.Neamb.Project.Web.UITest.Tests.LargerBrowser.Search
{
    public class SearchResultPageTests : NeambTestBaseLarge<SearchResultPage>
    {
        #region Tests
        [Test, Category("SearchPage")]
        public void AllControlsExistSearchResultsPage()
        {
            Page.AssertHasAllControlsForSections(new[] { "SearchResults" });
        }
        [Test, Category("SearchItems")]
        public void ResultsOnPage()
        {
            Page.EnterDataForSearch("Insurance");
            Page.AssertElementExists("ResultsItems");
        }

        #endregion
    }
}
