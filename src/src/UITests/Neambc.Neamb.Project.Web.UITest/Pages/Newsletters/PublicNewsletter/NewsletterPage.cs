using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Newsletters.PublicNewsletter
{
    public class NewsletterPage : NeambPage
    {
        #region ControlKeys
        private const string NewsletterCTA_Subscribe_Cold = "Newsletter_Subscribe";
        private const string NewsletterCTA_Unsubscribe_Cold = "Newsletter_Unsubscribe";
        private const string NewsletterCTA_Subscribe_Warm = "Newsletter_Subscribe";
        private const string NewsletterCTA_Unsubscribe_Warm = "Newsletter_Unsubscribe";
        private const string NewsletterCTA_Subscribe_Hot = "Newsletter_Subscribe";
        private const string NewsletterCTA_Unsubscribe_Hot = "Newsletter_Unsubscribe";
        private const string EmailField = "Newsletter_Email";
        private static int Timeout => 60;


        #endregion
        #region Constructor
        public NewsletterPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NewsletterPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NewsletterPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new NewsletterPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "headline-hero bg-blue"
            });
            return this;
        }

        public new NewsletterPage Newsletter_ValidateEmail(string email)
        {
            AssertSetTextBoxValue(EmailField, email);

            var errorEmailValidation = GetElementFromControlKey(EmailField)?
                .GetAttribute("data-msg-validatemail");

            AssertIsTrue(!string.IsNullOrEmpty(errorEmailValidation), $"Email validation doesn't show'");

            return this;
        }
        public new NewsletterPage Subscribe(string email)
        {
            AssertSetTextBoxValue(EmailField, email);
            AssertClick(NewsletterCTA_Subscribe_Cold, timeoutSeconds: Timeout);
            return this;
        }

        public new NewsletterPage PublicNewsletter_SubscribeGTM_Cold(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(NewsletterCTA_Subscribe_Cold)?
                 .GetAttribute("onclick");
            if (buttonOnClick != null)
            {
                AssertIsTrue(buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");
            }
            return this;
        }

        public new NewsletterPage PublicNewsletter_UnsubscribeGTM_Cold(string clickSecondaryFunction)
        {

            var buttonSecondaryOnClick = GetElementFromControlKey(NewsletterCTA_Unsubscribe_Cold)?
                .GetAttribute("onclick");
            if (buttonSecondaryOnClick != null)
            {
                AssertIsTrue(buttonSecondaryOnClick.Contains(clickSecondaryFunction), $"ClickAction {buttonSecondaryOnClick} doesn't match {clickSecondaryFunction}");
            }
            return this;
        }

        public new NewsletterPage PublicNewsletter_SubscribeGTM_Warm(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(NewsletterCTA_Subscribe_Warm)?
                 .GetAttribute("onclick");
            if (buttonOnClick != null)
            {
                AssertIsTrue(buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");
            }
            return this;
        }

        public new NewsletterPage PublicNewsletter_UnsubscribeGTM_Warm(string clickSecondaryFunction)
        {

            var buttonSecondaryOnClick = GetElementFromControlKey(NewsletterCTA_Unsubscribe_Warm)?
                .GetAttribute("onclick");
            if (buttonSecondaryOnClick != null)
            {
                AssertIsTrue(buttonSecondaryOnClick.Contains(clickSecondaryFunction), $"ClickAction {buttonSecondaryOnClick} doesn't match {clickSecondaryFunction}");
            }
            return this;
        }

        public new NewsletterPage PublicNewsletter_SubscribeGTM_Hot(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(NewsletterCTA_Subscribe_Hot)?
                 .GetAttribute("onclick");
            if (buttonOnClick != null)
            {
                AssertIsTrue(buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");
            }
            return this;
        }

        public new NewsletterPage PublicNewsletter_UnsubscribeGTM_Hot(string clickSecondaryFunction)
        {

            var buttonSecondaryOnClick = GetElementFromControlKey(NewsletterCTA_Unsubscribe_Hot)?
                .GetAttribute("onclick");
            if (buttonSecondaryOnClick != null)
            {
                AssertIsTrue(buttonSecondaryOnClick.Contains(clickSecondaryFunction), $"ClickAction {buttonSecondaryOnClick} doesn't match {clickSecondaryFunction}");
            }
            return this;
        }
    }
}
