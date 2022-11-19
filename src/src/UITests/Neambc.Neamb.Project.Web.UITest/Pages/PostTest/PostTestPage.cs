using System.Threading;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.PostTest
{
    public class PostTestPage : NeambPage
    {
        #region ControlKeys

        private const string PostTestPage_PrimaryCtaButton = "PostTestPage_PrimaryCtaButton";
        private const string PostTestPage_CardButton = "PostTestPage_CardButton";
        private const string PostTestPage_SpecialButton = "PostTestPage_SpecialButton";
        private const string PostTestPage_IdRow0 = "PostTestPage_IdRow0";
        private const string PostTestPage_IdRow1 = "PostTestPage_IdRow1";
        private const string PostTestPage_IdRow3 = "PostTestPage_IdRow3";
        private const string PostTestPage_IdRow4 = "PostTestPage_IdRow4";
        #endregion

        #region Constructor

        public PostTestPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName,
            urlPath, driver, settings)
        {
        }

        public PostTestPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public PostTestPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }

        #endregion

        public LoginPage ClickCtaButtonRedirectLogin()
        {
            AssertClick(PostTestPage_PrimaryCtaButton, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public LoginPage ClickMultiCardButtonRedirectLogin()
        {
            AssertClick(PostTestPage_CardButton, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public LoginPage ClickSpecialOfferButtonRedirectLogin()
        {
            AssertClick(PostTestPage_SpecialButton, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public PostTestPage ClickCtaButtonHotState()
        {
            AssertClick(PostTestPage_PrimaryCtaButton, timeoutSeconds: 30);
            return new PostTestPage(this.Driver, this.Settings);
        }

        public PostTestPage ClickMultiCardHotState()
        {
            AssertClick(PostTestPage_CardButton, timeoutSeconds: 30);
            return new PostTestPage(this.Driver, this.Settings);
        }

        public PostTestPage ClickSpecialOfferHotState()
        {
            AssertClick(PostTestPage_SpecialButton, timeoutSeconds: 30);
            return new PostTestPage(this.Driver, this.Settings);
        }

        public PostTestPage AssertHotStateCtaButton(string urlexpected,string td0, string td1)
        {
            Thread.Sleep(8000);
            var urlOpened = this.Driver.Url;
            AssertIsTrue(urlOpened.Contains(urlexpected));
            var tdRow0 = GetElementFromControlKey(PostTestPage_IdRow0)?.Text;
            AssertIsTrue(tdRow0 != null && tdRow0.Equals(td0));
            var tdRow1 = GetElementFromControlKey(PostTestPage_IdRow1)?.Text;
            AssertIsTrue(tdRow1 != null && tdRow1.Equals(td1));
            return this;
        }
        public PostTestPage AssertHotStateMultiCardSpecialButton(string urlexpected, string td0)
        {
            Thread.Sleep(8000);
            var urlOpened = this.Driver.Url;
            AssertIsTrue(urlOpened.Contains(urlexpected));
            var tdRow0 = GetElementFromControlKey(PostTestPage_IdRow0)?.Text;
            AssertIsTrue(tdRow0 != null && tdRow0.Equals(td0));
            return this;
        }

        public PostTestPage AssertForCarouselCards(string urlexpected, string td0,string td3, string td4)
        {
            Thread.Sleep(8000);
            var urlOpened = this.Driver.Url;
            AssertIsTrue(urlOpened.Contains(urlexpected));
            var tdRow0 = GetElementFromControlKey(PostTestPage_IdRow0)?.Text;
            AssertIsTrue(tdRow0 != null && tdRow0.Equals(td0));
            var tdRow3 = GetElementFromControlKey(PostTestPage_IdRow3)?.Text;
            AssertIsTrue(tdRow3 != null && tdRow3.Equals(td3));
            var tdRow4 = GetElementFromControlKey(PostTestPage_IdRow4)?.Text;
            AssertIsTrue(tdRow4 != null && tdRow4.Equals(td4));
            return this;
        }
    }
}
