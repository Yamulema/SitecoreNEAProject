using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.MultiProducts
{
    public class TestDataPassPage : Base.NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "TestDataPassPage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "TestDataPassPage_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public TestDataPassPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public TestDataPassPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public TestDataPassPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public new TestDataPassPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public ClickAndSavePartnerPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return new ClickAndSavePartnerPage(this.Driver, this.Settings);
        }

        public ClickAndSavePartnerPage ClickOnSecondaryCta()
        {
            AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
            return new ClickAndSavePartnerPage(this.Driver, this.Settings);
        }
    }
}
