using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.TestProducts.Efulfillment
	{
    public class Efulfillment : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "Efulfillent_CtaHot_PrimaryCta";
		
		
		#endregion
		#region Constructor
		public Efulfillment(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public Efulfillment(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public Efulfillment(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        

        public new Efulfillment AssertIsLoaded()
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
