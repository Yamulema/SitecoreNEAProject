using Neambc.Neamb.Project.Web.UITest.Pages.Account;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Account.MemberWelcome
{
    [TestFixture]
    public class MemberWelcomeNotRegisteredTest : NeambTestBaseLarge<MemberWelcomePage>
    {
        // -> Enter page -> Fill form with no registered user
        // -> Redirect to a zip code page
        [TestCaseSource(typeof(MemberWelcomeData),
            nameof(MemberWelcomeData.TestDataSource),
            new object[] { "NotRegistered" })]
        [Test, Category("Functional")]
        public void MemberWelcome_ColdState(string mdsId, string placeholderExpected)
        {
            Page.AssertIsLoaded()
                .FillForm(mdsId)
                .ClickToSubmitButton()
                .CheckIfElementHasPlaceholder("zip", placeholderExpected);
        }

        // -> Enter page -> Set to warm
        // -> Redirect to a zip code page
        [TestCaseSource(typeof(MemberWelcomeData),
            nameof(MemberWelcomeData.TestDataSource),
            new object[] { "NotRegisteredWarm" })]
        [Test, Category("Functional")]
        public void MemberWelcome_WarmState(string mdsId, string mdsIdComplete, string placeholderExpected)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<MemberWelcomePage>(mdsId)
                .CheckMdsIdFromHidden(mdsIdComplete)
                .ClickToSubmitButton()
                .CheckIfElementHasPlaceholder("zip", placeholderExpected);
        }

        [TestCaseSource(typeof(MemberWelcomeData),
            nameof(MemberWelcomeData.TestDataSource),
            new object[] { "NotFound" })]
        [Test, Category("Functional")]
        public void MemberWelcome_WarmState_MdsNotFound(string mdsId)
        {
            Page.AssertIsLoaded()
                .FillForm(mdsId)
                .ClickToSubmitButton()
                .CheckErrorMessage();
        }
    }
}
