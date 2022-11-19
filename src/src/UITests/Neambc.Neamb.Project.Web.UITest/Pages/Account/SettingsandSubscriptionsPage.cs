using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Account
{
    public class SettingsandSubscriptionsPage : NeambPageBase
    {
    public SettingsandSubscriptionsPage(
            IWebDriver driver,
            ISettings settings) : base(
                "SettingsSubscriptions",
                "/account/settings-and-subscriptions", driver, settings)
        { }

        public void LoginSS(string username, string password)
        {
            AssertSetTextBoxValue("LoginPage.LoginPage_Form_EmailTextbox", username);
            AssertSetTextBoxValue("LoginPage.LoginPage_Form_PasswordTextbox", password);
            AssertClick("LoginPage.LoginPage_Form_SignInButton");
        }

        public new SettingsandSubscriptionsPage Subscription_SubscribeGTM(string SubscribeKey, string UnsubscribeKey)
        {
            var buttonOnClick = GetElementFromControlKey(SubscribeKey, timeoutSeconds:1);
            if (buttonOnClick == null)  // if already subscribed, click Unsubscribe first
            {
                if( GetElementFromControlKey(UnsubscribeKey) != null )
                    AssertClick(UnsubscribeKey, timeoutSeconds: 500);
            }
            if (GetElementFromControlKey(SubscribeKey) != null)
                AssertClick(SubscribeKey, timeoutSeconds: 500);
            return this;
        }

        public new SettingsandSubscriptionsPage Subscription_UnsubscribeGTM(string SubscribeKey, string UnsubscribeKey)
        {
            var buttonOnClick = GetElementFromControlKey(UnsubscribeKey, timeoutSeconds:1);
            if (buttonOnClick == null)  // if already unsubscribed, click Subscribe first
            {
                if (GetElementFromControlKey(SubscribeKey) != null)
                    AssertClick(SubscribeKey, timeoutSeconds: 500);
            }
            if (GetElementFromControlKey(UnsubscribeKey) != null)
                AssertClick(UnsubscribeKey, timeoutSeconds: 500);
            return this;
        }
    }
}
