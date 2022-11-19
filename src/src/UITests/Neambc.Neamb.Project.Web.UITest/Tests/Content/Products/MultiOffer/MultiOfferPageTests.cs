using Neambc.Neamb.Project.Web.UITest.Pages.Products.MultiOffer;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.MultiOffer
{
    [TestFixture]
    public class MultiOfferPageTests : NeambTestBaseLarge<MultiOfferPage>
    {
        [TestCaseSource(typeof(MultiOfferPageTestData),
            nameof(MultiOfferPageTestData.TestDataSource),
            new object[] { "WarmState" })]
        [Test, Category("Content")]
        public void MultiOffer_Warm(string mdsid, string gtmPrimaryAction)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<MultiOfferPage>(mdsid)
                .AssertIsLoaded()
                .MultiOffer_Warm(gtmPrimaryAction);
        }

        [TestCaseSource(typeof(MultiOfferPageTestData),
            nameof(MultiOfferPageTestData.TestDataSource),
            new object[] { "DataPass" })]
        [Test, Category("Content")]
        public void MultiOffer_DataPass(string username, string password, string gtmPrimaryAction)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("Product disabled in live environment");

            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<MultiOfferPage>(username, password)
                .AssertIsLoaded()
                .MultiOffer_CtaHot(gtmPrimaryAction);
        }

        [TestCaseSource(typeof(MultiOfferPageTestData),
          nameof(MultiOfferPageTestData.TestDataSource),
          new object[] { "SSO" })]
        [Test, Category("Content")]
        public void MultiOffer_SSO(string username, string password, string gtmPrimaryAction)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("Product disabled in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<MultiOfferPage>(username, password)
                .AssertIsLoaded()
                .MultiOffer_SSO(gtmPrimaryAction);
        }

        [TestCaseSource(typeof(MultiOfferPageTestData),
                  nameof(MultiOfferPageTestData.TestDataSource),
                  new object[] { "PDF" })]
        [Test, Category("Content")]
        public void MultiOffer_PDF(string username, string password, string gtmPrimaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<MultiOfferPage>(username, password)
                .AssertIsLoaded()
                .MultiOffer_PDF(gtmPrimaryAction);
        }
        [TestCaseSource(typeof(MultiOfferPageTestData),
          nameof(MultiOfferPageTestData.TestDataSource),
          new object[] { "Link" })]
        [Test, Category("Content")]
        public void MultiOffer_Link(string username, string password, string gtmPrimaryAction)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("Product disabled in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<MultiOfferPage>(username, password)
                .AssertIsLoaded()
                .MultiOffer_Link(gtmPrimaryAction);
        }

    }
}
