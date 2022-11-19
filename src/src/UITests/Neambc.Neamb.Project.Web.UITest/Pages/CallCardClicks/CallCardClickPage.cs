using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.CallCardClicks
{
    public class CallCardClickPage : NeambPage
    {
        #region ControlKeys
        private const string LearnMoreColdButton = "LearnMore_CardButton";
        private object LearMore_CardButton;
        #endregion

        #region Constructor
        public CallCardClickPage(IWebDriver driver, ISettings settings) : base(name: "CallCardClickPage", driver: driver,
            settings: settings)
        {

        }
        #endregion

        public new CallCardClickPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "test"
            });
            return this;
        }

        public new CallCardClickPage CheckGtmActionCold(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(LearnMoreColdButton)?
                .GetAttribute("onclick");
            AssertIsTrue(buttonOnClick.StartsWith(clickFunction), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");

            return this;
        }
    }
}
