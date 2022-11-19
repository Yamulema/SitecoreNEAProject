using Neambc.Neamb.Project.Web.UITest.Pages.Newsletters.PrivateNewsletter;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Newsletters.PrivateNewsletter
{
    [TestFixture]
    public class NewsletterPageTests : NeambTestBaseLarge<PrivateNewsletterPage>
    {
        [TestCaseSource(typeof(PrivateNewsletterPageTestData),
            nameof(PrivateNewsletterPageTestData.TestDataSource),
            new object[] { "Subscribe" })]
        [Test, Category("Content")]
        public void PrivateNewsletter_SubscribeGtm(string username, string password, string gtmPrimaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<PrivateNewsletterPage>(username, password)
                .AssertIsLoaded()
                .PrivateNewsletter_SubscribeGTM(gtmPrimaryAction);
        }

        [TestCaseSource(typeof(PrivateNewsletterPageTestData),
           nameof(PrivateNewsletterPageTestData.TestDataSource),
           new object[] { "Unsubscribe" })]
        [Test, Category("Content")]
        public void PrivateNewsletter_UnsubscribeGtm(string username, string password, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<PrivateNewsletterPage>(username, password)
                .AssertIsLoaded()
                .PrivateNewsletter_UnsubscribeGTM(gtmSecondaryAction);
        }

    }
}
