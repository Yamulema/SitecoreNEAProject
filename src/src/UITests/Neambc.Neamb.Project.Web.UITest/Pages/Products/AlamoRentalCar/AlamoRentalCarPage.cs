using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.AlamoRentalCar
	{
    public class AlamoRentalCarPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "AlamoRentalCarPage_CtaHot_PrimaryCta";
		private const string SecondaryCtaHot = "AlamoRentalCarPage_CtaHot_SecondaryCta";
		
		#endregion
		#region Constructor
		public AlamoRentalCarPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public AlamoRentalCarPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public AlamoRentalCarPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new AlamoRentalCarPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public RentalCarPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
			return new RentalCarPage(this.Driver, this.Settings);
		}
		public PdfMultirowPage ClickOnSecondaryCta() 
		{
			AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
			return new PdfMultirowPage(this.Driver, this.Settings);
		}
	}
}
