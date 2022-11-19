using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Newsletters.PrivateNewsletter
{
    public class PrivateNewsletterPage : NeambPage
    {
        #region ControlKeys
        private const string NewsletterCTA_Subscribe = "Newsletter_Subscribe";
        private const string NewsletterCTA_Unsubscribe = "Newsletter_Unsubscribe";

        #endregion
        #region Constructor
        public PrivateNewsletterPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public PrivateNewsletterPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public PrivateNewsletterPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new PrivateNewsletterPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "headline-hero bg-blue"
            });
            return this;
        }

        public new PrivateNewsletterPage PrivateNewsletter_SubscribeGTM(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(NewsletterCTA_Subscribe)?
                 .GetAttribute("onclick");
            if (buttonOnClick != null)
            {
                AssertIsTrue(buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");
            }
            return this;
        }

        public new PrivateNewsletterPage PrivateNewsletter_UnsubscribeGTM(string clickSecondaryFunction)
        {

            var buttonSecondaryOnClick = GetElementFromControlKey(NewsletterCTA_Unsubscribe)?
                .GetAttribute("onclick");
            if (buttonSecondaryOnClick != null)
            {
                AssertIsTrue(buttonSecondaryOnClick.Contains(clickSecondaryFunction), $"ClickAction {buttonSecondaryOnClick} doesn't match {clickSecondaryFunction}");
            }
            return this;
        }
    }
}
