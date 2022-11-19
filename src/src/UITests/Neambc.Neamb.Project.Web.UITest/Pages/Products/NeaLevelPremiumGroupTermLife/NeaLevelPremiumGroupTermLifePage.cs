using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaLevelPremiumGroupTermLife {
    public class NeaLevelPremiumGroupTermLifePage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaLevelPremiumGroupTermLifePage_CtaHot_PrimaryCta";
		private const string SecondaryCtaHot = "NeaLevelPremiumGroupTermLifePage_CtaHot_SecondaryCta";
		
		#endregion
		#region Constructor
		public NeaLevelPremiumGroupTermLifePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaLevelPremiumGroupTermLifePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaLevelPremiumGroupTermLifePage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new NeaLevelPremiumGroupTermLifePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public MemberEnrollPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
			return new MemberEnrollPage(this.Driver, this.Settings);
		}
		public MercerPage ClickOnSecondaryCta() {
			AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
			return new MercerPage(this.Driver, this.Settings);
		}
	}
}
