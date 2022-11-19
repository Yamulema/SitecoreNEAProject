using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Topic
{
    public class TopicPage : NeambPage
    {
        #region ControlKeys
        private const string SeeMore = "Topic_seemore";
        private const string ItemTaxPrep = "Topic_tax_prep";
        private const string ItemTax = "Topic_item_tax";
        #endregion
        #region Constructor
        public TopicPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public TopicPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public TopicPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new TopicPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        
        public new TopicPage CheckGtmActionSeeMore(string SeeMoreLink)
        {
            var buttonOnClick = GetElementFromControlKey(SeeMore)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(SeeMoreLink), $"ClickAction {buttonOnClick} doesn't match {SeeMoreLink}");

            return this;
        }

        public new TopicPage CheckGtmActionItems(string ItemTaxLink)
        {
            var buttonOnClick = GetElementFromControlKey(ItemTax)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(ItemTaxLink), $"ClickAction {buttonOnClick} doesn't match {ItemTaxLink}");

            return this;
        }

        public new TopicPage CheckGtmActionItemTaxPrep(string ItemTaxPrepLink)
        {
            var buttonOnClick = GetElementFromControlKey(ItemTaxPrep)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(ItemTaxPrepLink), $"ClickAction {buttonOnClick} doesn't match {ItemTaxPrepLink}");

            return this;
        }

    }
}
