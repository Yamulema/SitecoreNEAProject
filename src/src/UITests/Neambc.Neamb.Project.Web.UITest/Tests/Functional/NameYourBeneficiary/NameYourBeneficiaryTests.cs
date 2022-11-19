using Neambc.Neamb.Project.Web.UITest.Pages.MarketPlace;
using Neambc.Neamb.Project.Web.UITest.Pages.NameYourBeneficiary;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.NameYourBeneficiary
{
    [TestFixture]
    public class NameYourBeneficiaryTests : NeambTestBaseLarge<NameYourBeneficiaryPage>
    {
        /// <summary>
        /// Test hot state when the user clicked in tote bag button
        /// </summary>
        /// <param name="username">User name to logged in</param>
        /// <param name="password">Password to logged in</param>
        /// <param name="flag">Name of the parameter redirected to the complimentary page</param>
        [TestCaseSource(typeof(NameYourBeneficiaryData),
            nameof(NameYourBeneficiaryData.TestDataSource),
            new object[] {"NameYourBeneficiaryHot"})]
        [Test, Category("Functional")]
        public void TestHotState(string username, string password, string flag)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.ClickOnSignInLink()
                .SignIn<NameYourBeneficiaryPage>(username, password)
                .ClickNameYourBeneficiaryButtonHot()
                .VerifyUrlHasParameter(flag)
                .ClickOnSignOutLink();
        }
    }
}
