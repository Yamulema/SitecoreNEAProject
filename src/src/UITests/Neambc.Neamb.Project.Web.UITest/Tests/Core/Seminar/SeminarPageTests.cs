using Neambc.Neamb.Project.Web.UITest.Pages.Seminar;
using Neambc.Neamb.Project.Web.UITest.Pages.Sweepstake;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.Login
{
    [TestFixture]
    public class SeminarPageTests : NeambTestBaseLarge<SeminarPage>
    {
        //TASK MBREQ-797
        //COLD USER
        [TestCaseSource(typeof(SeminarPageTestData),
            nameof(SeminarPageTestData.TestDataSource),
            new object[] { "Test_ExecuteAction" })]
        [Test, Category("Core")]
        public void VerifyProcessFromColdSignInExecuteProcess(string username, string password)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickSeminarButtonColdWarmUser()
                .SignIn<SeminarPage>(username, password)
                .AssertIsLoaded()
                .CheckPopupIsDisplayed();
        }
        //TASK MBREQ-797
        //WARM USER
        [TestCaseSource(typeof(SeminarPageTestData),
            nameof(SeminarPageTestData.TestDataSource),
            new object[] { "Test_ExecuteActionWarm" })]
        [Test, Category("Core")]
        public void VerifyProcessFromWarmSignInExecuteProcess(string mdsid, string username, string password)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.AssertIsLoaded()
                .SetAsWarm<SeminarPage>(mdsid)
                .ClickSeminarButtonColdWarmUser()
                .SignIn<SeminarPage>(username, password)
                .AssertIsLoaded()
                .CheckPopupIsDisplayed();
        }

        //TASK MBREQ-797
        //HOT USER
        [TestCaseSource(typeof(SeminarPageTestData),
            nameof(SeminarPageTestData.TestDataSource),
            new object[] { "Test_ExecuteAction" })]
        [Test, Category("Core")]
        public void VerifyProcessFromHotSignInExecuteProcess(string username, string password)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<SeminarPage>(username, password)
                .AssertIsLoaded()
                .ClickOnCtaButtonHotUser()
                .CheckPopupIsDisplayed();
        }
    }
}
