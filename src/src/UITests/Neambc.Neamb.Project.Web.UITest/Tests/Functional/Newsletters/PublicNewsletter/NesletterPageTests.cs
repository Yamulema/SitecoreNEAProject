using Neambc.Neamb.Project.Web.UITest.Pages.Newsletters.PublicNewsletter;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Newsletters.PublicNewsletter
{
    [TestFixture]
    public class NewsletterPageTests : NeambTestBaseLarge<NewsletterPage>
    {
        [TestCaseSource(typeof(NewsletterPageTestData),
          nameof(NewsletterPageTestData.TestDataSource),
          new object[] { "Newsletter_ValidateEmail" })]
        [Test, Category("Content")]

        public void Newsletter_ValidateEmail(string email)
        {
            Page.AssertIsLoaded().Newsletter_ValidateEmail(email);
        }

        [TestCaseSource(typeof(NewsletterPageTestData),
          nameof(NewsletterPageTestData.TestDataSource),
          new object[] { "Subscribe_Cold" })]
        [Test, Category("Content")]
        public void PublicNewsletter_SubscribeGtm_Cold(string email, string gtmPrimaryAction)
        {
            Page.AssertIsLoaded().Subscribe(email)
                .PublicNewsletter_SubscribeGTM_Cold(gtmPrimaryAction);
        }

        [TestCaseSource(typeof(NewsletterPageTestData),
           nameof(NewsletterPageTestData.TestDataSource),
           new object[] { "Unsubscribe_Warm" })]
        [Test, Category("Content")]
        public void PublicNewsletter_UnsubscribeGtm_Warm(string mdsid, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                 .SetAsWarm<NewsletterPage>(mdsid)
                .PublicNewsletter_UnsubscribeGTM_Warm(gtmSecondaryAction);
        }

        [TestCaseSource(typeof(NewsletterPageTestData),
        nameof(NewsletterPageTestData.TestDataSource),
        new object[] { "Subscribe_Warm" })]
        [Test, Category("Content")]
        public void PublicNewsletter_SubscribeGtm_Warm(string mdsid, string gtmPrimaryAction)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<NewsletterPage>(mdsid)
                .PublicNewsletter_SubscribeGTM_Warm(gtmPrimaryAction);
        }

        [TestCaseSource(typeof(NewsletterPageTestData),
          nameof(NewsletterPageTestData.TestDataSource),
          new object[] { "Unsubscribe_Hot" })]
        [Test, Category("Content")]
        public void PublicNewsletter_UnsubscribeGtm_Hot(string username, string password, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NewsletterPage>(username, password)
                .AssertIsLoaded()
                .PublicNewsletter_UnsubscribeGTM_Hot(gtmSecondaryAction);
        }

        [TestCaseSource(typeof(NewsletterPageTestData),
            nameof(NewsletterPageTestData.TestDataSource),
            new object[] { "Subscribe_Hot" })]
        [Test, Category("Content")]
        public void PublicNewsletter_SubscribeGtm_Hot(string username, string password, string gtmPrimaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NewsletterPage>(username, password)
                .AssertIsLoaded()
                .PublicNewsletter_SubscribeGTM_Hot(gtmPrimaryAction);
        }
    }
}
