using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.ForgotEmail;
using Neambc.Neamb.Project.Web.UITest.Pages.ForgotPassword;
using Neambc.Neamb.Project.Web.UITest.Pages.Home;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Login
{
    public class ProductPage : NeambPage
    {
        #region Constants
        private static int Timeout => 30;
        #endregion
        #region Constructor
        public ProductPage(IWebDriver driver, ISettings settings) : base(name: "ProductPage", driver: driver,
            settings: settings)
        { }
        #endregion
        #region ControlKeys
        private const string SignInButton = "ProductPage_Cta_SignInButton";
        #endregion

        #region Public Methods
        public new ProductPage AssertIsLoadedCold()
        {
            AssertHasAllControlsForSections(new[] {
                "Common",
                "CtaComponentCold"
            });
            return this;
        }

        public new LoginPage AssertClickSignInButton()
        {
            AssertClick(SignInButton, timeoutSeconds: Timeout);
            return new LoginPage(this.Driver, this.Settings);
        }

        public ProductPage AssertNotEligibleComponents()
        {
            AssertHasAllControlsForSections(new[] {
                "NotEligible"
            });
            return this;
        }


        #endregion
    }

}
