using Neambc.Neamb.Project.Web.UITest.Pages.Sweepstake;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.Login
{
    [TestFixture]
    public class SweepstakesPageTests : NeambTestBaseLarge<SweepstakesPage>
    {
        /// <summary>
        /// 
        /// </summary>
        [TestCaseSource(typeof(SweepstakesPageTestData),
            nameof(SweepstakesPageTestData.TestDataSource),
            new object[] { "Test_ColdAction" })]

        [Test, Category("Core")]
        public void ValidateGtmActionButton(string gtmAction)
        {
            Page.AssertIsLoaded().CheckGtmActionCold(gtmAction);
        }

        [TestCaseSource(typeof(SweepstakesPageTestData),
            nameof(SweepstakesPageTestData.TestDataSource),
            new object[] { "Test_HotAction" })]
        [Test, Category("Core")]
        public void ValidateGtmActionButtonHot(string username, string password,string gtmAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<SweepstakesPage>(username, password)
                .AssertIsLoaded()
                .CheckGtmActionHot(gtmAction);
        }

        [TestCaseSource(typeof(SweepstakesPageTestData),
            nameof(SweepstakesPageTestData.TestDataSource),
            new object[] { "Test_PopupAction" })]
        [Test, Category("Core")]
        public void ValidateGtmActionPopup(string username, string password, string gtmAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<SweepstakesPage>(username, password)
                .AssertIsLoaded()
                .ClickOnCtaButton()
                .CheckGtmActionPopup(gtmAction);
        }
        [TestCaseSource(typeof(SweepstakesPageTestData),
            nameof(SweepstakesPageTestData.TestDataSource),
            new object[] { "Test_ExecuteAction" })]
        [Test, Category("Core")]
        public void VerifyPopupWindowShown(string username, string password)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<SweepstakesPage>(username, password)
                .AssertIsLoaded()
                .ClickSweepstakeButton();
        }

        //TASK MBREQ-751
        [TestCaseSource(typeof(SweepstakesPageTestData),
            nameof(SweepstakesPageTestData.TestDataSource),
            new object[] { "Test_ExecuteAction" })]
        [Test, Category("Core")]
        public void VerifyProcessFromColdSignInExecuteProcess(string username, string password)
        {
            Page.AssertIsLoaded()
                .ClickSweepstakeButtonColdWarmUser()
                .SignIn<SweepstakesPage>(username, password)
                .AssertIsLoaded()
                .CheckPopupIsDisplayed();
        }
        //TASK MBREQ-751
        [TestCaseSource(typeof(SweepstakesPageTestData),
            nameof(SweepstakesPageTestData.TestDataSource),
            new object[] { "Test_ExecuteActionWarm" })]
        [Test, Category("Core")]
        public void VerifyProcessFromWarmSignInExecuteProcess(string mdsid, string username, string password)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<SweepstakesPage>(mdsid)
                .ClickSweepstakeButtonColdWarmUser()
                .SignIn<SweepstakesPage>(username, password)
                .AssertIsLoaded()
                .CheckPopupIsDisplayed();
        }
    }
}
