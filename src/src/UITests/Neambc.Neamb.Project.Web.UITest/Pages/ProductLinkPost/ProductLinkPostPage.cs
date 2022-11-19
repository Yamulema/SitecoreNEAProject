using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.ProductLinkPost
{
    public class ProductLinkPostPage : NeambPage
    {
        #region ControlKeys

        private const string ProductLinkPostPage_TitleCard = "ProductLinkPostPage_TitleCard";
        
        #endregion

        #region Constructor

        public ProductLinkPostPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName,
            urlPath, driver, settings)
        {
        }

        public ProductLinkPostPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public ProductLinkPostPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        public ProductLinkPostPage VerifyTitleCard(string title)
        {
            var card = GetElementFromControlKey(ProductLinkPostPage_TitleCard)?.Text;
            AssertIsTrue(card.Contains(title), "Error in title");
            return this;
        }

        #endregion


    }
}
