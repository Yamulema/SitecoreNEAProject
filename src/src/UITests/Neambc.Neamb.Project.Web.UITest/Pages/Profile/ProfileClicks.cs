using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Profile
{
    public class ProfileClicks : NeambPage
    {
        #region ControlKeys
        private const string AccountPasswordButton = "AccountButton";
        #endregion

        #region Constructor

        public ProfileClicks(IWebDriver driver, ISettings settings) : base(name: "ProfileClicks", driver: driver,
            settings: settings)
        {

        }

        #endregion
        public new ProfileClicks AssertIsLoaded()
  
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }

        public ProfileClicks CheckGtmActionProfile(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(AccountPasswordButton)?
                .GetAttribute("onclick");
            AssertIsTrue(buttonOnClick.Contains(clickFunction), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");
            return this;
        }

    }
}

