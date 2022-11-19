using System;
using Neambc.Seiumb.UITests.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Seiumb.UITests.Pages.Tokens {
    public class TokenPage : SeiumbPage
    {
        #region ControlKeys

        private const string ThanksDiv = "ThanksDiv";
        
        #endregion

        public TokenPage(
            IWebDriver driver,
            ISettings settings) : base(driver, settings)
        {
        }

        public new TokenPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[]
            {
                "RichText"
            });
            return this;
        }
        public new TokenPage CheckContentToken(string inputText)
        {
            var textDiv = GetElementFromControlKey(ThanksDiv)?
                .Text;
            AssertIsTrue(string.Equals(textDiv, inputText, StringComparison.InvariantCultureIgnoreCase), $"ClickAction {inputText} doesn't match {textDiv}");

            return this;
        }
    }
}
