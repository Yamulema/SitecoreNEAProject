using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Search
{
    public class SearchResultPage : NeambPageBase
    {

        public SearchResultPage(
            IWebDriver driver,
            ISettings settings) : base(
                "SearchResultPage",
                "/search", driver, settings
            )
        {  }
        public void EnterDataForSearch(string dataforsearch)
        {
            AssertSetTextBoxValue("SearchResultBox", dataforsearch);
            AssertClick("SearchResultEnter");
        }
    }
}
