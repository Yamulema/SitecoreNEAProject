using Neambc.Neamb.Project.Web.UITest.Pages.FreqAQ;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.FreqAQ
{
    [TestFixture]
    public class FreqAQTest : NeambTestBaseLarge<FreqAQPage>
    {
        [TestCaseSource(typeof(FreqAQPageTestData),
            nameof(FreqAQPageTestData.TestDataSource),
            new object[] { "login" })]
        [Test, Category("Core")]
        public void CheckGtmLogin(string LoginLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmFQALogin(LoginLink);
        }

        [TestCaseSource(typeof(FreqAQPageTestData),
                nameof(FreqAQPageTestData.TestDataSource),
                new object[] { "account" })]
        [Test, Category("Core")]
        public void CheckGtmAccount(string AccountLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmFQAAccount(AccountLink);
        }

        [TestCaseSource(typeof(FreqAQPageTestData),
               nameof(FreqAQPageTestData.TestDataSource),
               new object[] { "reset" })]
        [Test, Category("Core")]
        public void CheckGtmReset(string ResetLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmFAQReset(ResetLink);
        }
    }
}

