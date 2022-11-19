using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products {
    public abstract class ProductPageBase : NeambPageBase {

        #region Fields
        protected const string LoginKey = "Products.products_signin_button";
        #endregion

        #region Constructors
        protected ProductPageBase(
            string name,
            string urlPath,
            IWebDriver driver,
            ISettings settings) : base(name, urlPath, driver, settings) {
        }
        #endregion

        #region Protected Methods
        public override void Login(string username, string password, string signinKey = null, bool force = false) {
            base.Login(username, password, signinKey ?? LoginKey, force);
        }
		public void CleanCache() {

			Driver.Manage().Cookies.DeleteAllCookies();
		}
		#endregion

	}
}
