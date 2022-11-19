using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaAutoBuyingProgram
{
    public class NeaAutoBuyingProgramPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaAutoBuyingProgramPage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "NeaAutoBuyingProgramPage_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public NeaAutoBuyingProgramPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaAutoBuyingProgramPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaAutoBuyingProgramPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new NeaAutoBuyingProgramPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public NeaAutoBuyingProgramPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return this;
        }
        public NeaAutoBuyingProgramPage ClickOnSecondaryCta()
        {
            AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
            return this;
        }
    }
}
