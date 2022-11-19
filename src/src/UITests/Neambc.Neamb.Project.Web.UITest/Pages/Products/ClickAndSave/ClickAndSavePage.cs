using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.ClickAndSave
{
    public class ClickAndSavePage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "ClickAndSavePage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "ClickAndSavePage_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public ClickAndSavePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public ClickAndSavePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public ClickAndSavePage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public new ClickAndSavePage AssertIsLoaded()
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
