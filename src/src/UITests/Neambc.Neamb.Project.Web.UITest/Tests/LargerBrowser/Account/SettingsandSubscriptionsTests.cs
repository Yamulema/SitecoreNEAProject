using Neambc.Neamb.Project.Web.UITest.Pages;
using NUnit.Framework;
using Neambc.Neamb.Project.Web.UITest.Pages.Account;
using OpenQA.Selenium;

namespace Neambc.Neamb.Project.Web.UITest.Tests.LargerBrowser.Account
{
    public class SettingsandSubscriptionsTests : NeambTestBaseLarge<SettingsandSubscriptionsPage>
    {
        private const string Financial_Whiteboard_Subscribe = "Financial_Whiteboard_Subscribe";
        private const string Financial_Whiteboard_Unsubscribe = "Financial_Whiteboard_Unsubscribe";
        private const string Shoppers_Subscribe = "Shoppers_Subscribe";
        private const string Shoppers_Unsubscribe = "Shoppers_Unsubscribe";
        private const string Travel_Subscribe = "Travel_Subscribe";
        private const string Travel_Unsubscribe = "Travel_Unsubscribe";

        #region Tests

        [Test, Category("Settings and Subscriptions")]
        public void PageOpenssuccessful()
        {
            Page.AssertClick("SS_SignIn");
            Page.LoginSS(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword);
            Page.AssertIsLoaded();
            Page.AssertElementExists("ManagePS_Title");               
        }

        [TestCaseSource(typeof(SettingsandSubscriptionsPageTestData),
                   nameof(SettingsandSubscriptionsPageTestData.TestDataSource),
                   new object[] { "Subscribe_FWB" })]
        [Test, Category("Content")]
        public void Subscription_SubscribeGTM_FWB(string gtmPrimaryAction)
        {
            Page.AssertClick("SS_SignIn");
            Page.LoginSS(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword);
            Page.AssertIsLoaded();
            Page.Subscription_SubscribeGTM(Financial_Whiteboard_Subscribe, Financial_Whiteboard_Unsubscribe);
            var jsExec = (IJavaScriptExecutor)Page.Driver;
            var mydata = (string)jsExec.ExecuteScript("return JSON.stringify(dataLayer[ dataLayer.map(e=>e.event).lastIndexOf( 'account' ) ])");
            Page.AssertIsTrue(mydata.Contains(gtmPrimaryAction) );
       }

        [TestCaseSource(typeof(SettingsandSubscriptionsPageTestData),
           nameof(SettingsandSubscriptionsPageTestData.TestDataSource),
           new object[] { "Unsubscribe_FWB" })]
        [Test, Category("Content")]
        public void Subscription_UnsubscribeGTM_FWB(string gtmSecondaryAction)
        {
            Page.AssertClick("SS_SignIn");
            Page.LoginSS(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword);
            Page.AssertIsLoaded();
            Page.Subscription_UnsubscribeGTM(Financial_Whiteboard_Subscribe, Financial_Whiteboard_Unsubscribe);
            var jsExec = (IJavaScriptExecutor)Page.Driver;
            var mydata = (string)jsExec.ExecuteScript("return JSON.stringify(dataLayer[ dataLayer.map(e=>e.event).lastIndexOf( 'account' ) ])");
            Page.AssertIsTrue(mydata.Contains(gtmSecondaryAction));
        }
        
        [TestCaseSource(typeof(SettingsandSubscriptionsPageTestData),
                  nameof(SettingsandSubscriptionsPageTestData.TestDataSource),
                  new object[] { "Subscribe_Shoppers" })]
        [Test, Category("Content")]
        public void Subscription_SubscribeGTM_Shoppers(string gtmPrimaryAction)
        {
            Page.AssertClick("SS_SignIn");
            Page.LoginSS(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword);
            Page.AssertIsLoaded();
            Page.Subscription_SubscribeGTM(Shoppers_Subscribe, Shoppers_Unsubscribe);
            var jsExec = (IJavaScriptExecutor)Page.Driver;
            var mydata = (string)jsExec.ExecuteScript("return JSON.stringify(dataLayer[ dataLayer.map(e=>e.event).lastIndexOf( 'account' ) ])");
            Page.AssertIsTrue(mydata.Contains(gtmPrimaryAction));
        }

        [TestCaseSource(typeof(SettingsandSubscriptionsPageTestData),
           nameof(SettingsandSubscriptionsPageTestData.TestDataSource),
           new object[] { "Unsubscribe_Shoppers" })]
        [Test, Category("Content")]
        public void Subscription_UnsubscribeGTM_Shoppers(string gtmSecondaryAction)
        {
            Page.AssertClick("SS_SignIn");
            Page.LoginSS(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword);
            Page.AssertIsLoaded();
            Page.Subscription_UnsubscribeGTM(Shoppers_Subscribe, Shoppers_Unsubscribe);
            var jsExec = (IJavaScriptExecutor)Page.Driver;
            var mydata = (string)jsExec.ExecuteScript("return JSON.stringify(dataLayer[ dataLayer.map(e=>e.event).lastIndexOf( 'account' ) ])");
            Page.AssertIsTrue(mydata.Contains(gtmSecondaryAction));
        }

        [TestCaseSource(typeof(SettingsandSubscriptionsPageTestData),
                  nameof(SettingsandSubscriptionsPageTestData.TestDataSource),
                  new object[] { "Subscribe_Travel" })]
        [Test, Category("Content")]
        public void Subscription_SubscribeGTM_Travel(string gtmPrimaryAction)
        {
            Page.AssertClick("SS_SignIn");
            Page.LoginSS(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword);
            Page.AssertIsLoaded();
            Page.Subscription_SubscribeGTM(Travel_Subscribe, Travel_Unsubscribe);
            var jsExec = (IJavaScriptExecutor)Page.Driver;
            var mydata = (string)jsExec.ExecuteScript("return JSON.stringify(dataLayer[ dataLayer.map(e=>e.event).lastIndexOf( 'account' ) ])");
            Page.AssertIsTrue(mydata.Contains(gtmPrimaryAction));
        }

        [TestCaseSource(typeof(SettingsandSubscriptionsPageTestData),
           nameof(SettingsandSubscriptionsPageTestData.TestDataSource),
           new object[] { "Unsubscribe_Travel" })]
        [Test, Category("Content")]
        public void Subscription_UnsubscribeGTM_Travel(string gtmSecondaryAction)
        {
            Page.AssertClick("SS_SignIn");
            Page.LoginSS(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword);
            Page.AssertIsLoaded();
            Page.Subscription_UnsubscribeGTM(Travel_Subscribe, Travel_Unsubscribe);
            var jsExec = (IJavaScriptExecutor)Page.Driver;
            var mydata = (string)jsExec.ExecuteScript("return JSON.stringify(dataLayer[ dataLayer.map(e=>e.event).lastIndexOf( 'account' ) ])");
            Page.AssertIsTrue(mydata.Contains(gtmSecondaryAction));
        }

        #endregion
    }
}
