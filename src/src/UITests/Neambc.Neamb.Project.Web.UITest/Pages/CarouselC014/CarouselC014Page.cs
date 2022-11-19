using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.CarouselC014
{
    public class CarouselC014Page : NeambPage
    {
        #region ControlKeys
        private const string SeeMore = "Topic_seemore";
        private const string ItemTaxPrep = "Topic_tax_prep";
        private const string ItemTax = "Topic_item_tax";
        #endregion
        #region Constructor
        public CarouselC014Page(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public CarouselC014Page(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public CarouselC014Page(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new CarouselC014Page AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                 "Component"
             });
            return this;
        }

        public new CarouselC014Page CheckGtmActionItems(string ItemTaxLink)
        {
            var buttonOnClick = GetElementFromControlKey(ItemTax)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(ItemTaxLink), $"ClickAction {buttonOnClick} doesn't match {ItemTaxLink}");

            return this;
        }

        public new CarouselC014Page CheckGtmActionItemTaxPrep(string ItemTaxPrepLink)
        {
            var buttonOnClick = GetElementFromControlKey(ItemTaxPrep)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(ItemTaxPrepLink), $"ClickAction {buttonOnClick} doesn't match {ItemTaxPrepLink}");

            return this;
        }

    }
}