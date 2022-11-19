using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.TestProducts.SingleSignOn
{
    public class SingleSignOn : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "SingleSignOn_CtaHot_PrimaryCta";
		
		
		#endregion
		#region Constructor
		public SingleSignOn(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public SingleSignOn(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public SingleSignOn(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        

        public new SingleSignOn AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        
		public PdfMultirowPage ClickOnPrimaryCta() 
		{
			AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
			return new PdfMultirowPage(this.Driver, this.Settings);
		}
	}
}
