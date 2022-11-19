using System;
using Neambc.Seiumb.UITests.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Seiumb.UITests.Pages.Search
{
    public class SearchPage : SeiumbPage
    {
        private static int Timeout => 30;

        #region ControlKeys
        private const string SearchBarLink = "TopSearchBarLink";
        private const string SearchBarInput = "SearchBarInput";
        private const string SearchButton = "SearchButton";
        private const string SearchFirstResultDiv = "SearchFirstResultDiv";
        private const string SearchFirstResultTitle = "SearchFirstResultTitle";
        private const string SearchFirstResultLink = "SearchFirstResultLink";
        #endregion

        #region Constructor
        public SearchPage(string pageName, IWebDriver driver, ISettings settings) : base(name: pageName, driver: driver,
            settings: settings)
        {

        }

        public SearchPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }
        #endregion

        public SearchPage SearchText(string searchtext)
        {
            AssertControlClick(SearchBarLink);
            PerformSearchAction(searchtext);
            return this;
        }

        public SearchPage ValidateSearchResults(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText, string gtmUrlText)
        {
            AssertElementExists(SearchFirstResultDiv);

            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               SearchFirstResultTitle, "ValidateSearchResults");

            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmUrlText,
               SearchFirstResultLink, "ValidateSearchResults");
            return this;
        }

        private void PerformSearchAction(string searchtext)
        {    
            AssertSetTextBoxValue(SearchBarInput, searchtext);
            AssertClick(SearchButton, timeoutSeconds: Timeout);
        }

        private SearchPage CheckGtmAction(string gtmDataLayer, string gtmEvent,
           string gtmModule, string gtmText, string controlKey, string section)
        {
            var onClickFunction = GetElementFromControlKey(controlKey)?
                .GetAttribute("onclick");

            var hasDatalayerFunction = onClickFunction.Contains(gtmDataLayer);
            var hasGTMEvent = onClickFunction.Contains(gtmEvent);
            var hasGTMModule = onClickFunction.Contains(gtmModule);
            var hasGTMText = onClickFunction.Contains(gtmText);

            AssertIsTrue(hasDatalayerFunction, $"Error in {section} checking {gtmDataLayer}");
            AssertIsTrue(hasGTMEvent, $"Error in {section} checking {gtmEvent}");
            AssertIsTrue(hasGTMModule, $"Error in {section} checking {gtmModule}");
            AssertIsTrue(hasGTMText, $"Error in {section} checking {gtmText}");
            return this;
        }
    }
}
