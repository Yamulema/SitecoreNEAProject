using Neambc.Neamb.Project.Web.UITest.Pages.ForgotPassword;
using Neambc.Neamb.Project.Web.UITest.Pages.Sweepstake;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.Login
{
    [TestFixture]
    public class SweepstakesPageTestsNoSendEmail : NeambTestBaseLarge<SweepstakesPageNoSendEmail>
    {
        
        [TestCaseSource(typeof(SweepstakesPageTestDataNoSendEmail),
            nameof(SweepstakesPageTestDataNoSendEmail.TestDataSource),
            new object[] { "Test_SendEmailAction" })]
        [Test, Category("Core")]
        public void VerifyPopupWindowShown(string username, string password)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("This test only runs in QA");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<SweepstakesPageNoSendEmail>(username, password)
                .AssertIsLoaded()
                .ClickSweepstakeButton();
        }

    }
}
