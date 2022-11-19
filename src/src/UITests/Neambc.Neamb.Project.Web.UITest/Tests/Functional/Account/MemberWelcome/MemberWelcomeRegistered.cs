using Neambc.Neamb.Project.Web.UITest.Pages.Account;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Account.MemberWelcome
{
    [TestFixture]
    public class MemberWelcomeRegisteredTest : NeambTestBaseLarge<MemberWelcomePage>
    {
        // -> Enter page -> Fill form with registered user
        // -> Redirect to a password page
        [TestCaseSource(typeof(MemberWelcomeData),
            nameof(MemberWelcomeData.TestDataSource),
            new object[] { "Registered" })]
        [Test, Category("Functional")]
        public void MemberWelcome_ColdState(string mdsId, string placeholderExpected)
        {
            Page.AssertIsLoaded()
                .FillForm(mdsId)
                .ClickToSubmitButton()
                .CheckIfElementHasPlaceholder("password", placeholderExpected);
        }

        // -> Enter page -> Set to warm
        // -> Redirect to a password page
        [TestCaseSource(typeof(MemberWelcomeData),
            nameof(MemberWelcomeData.TestDataSource),
            new object[] { "RegisteredWarm" })]
        [Test, Category("Functional")]
        public void MemberWelcome_WarmState(string mdsId, string mdsIdComplete, string placeholderExpected)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<MemberWelcomePage>(mdsId)
                .CheckMdsIdFromHidden(mdsIdComplete)
                .ClickToSubmitButton()
                .CheckIfElementHasPlaceholder("password", placeholderExpected);
        }
    }
}
