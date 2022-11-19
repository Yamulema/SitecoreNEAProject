using Neambc.Neamb.Project.Web.UITest.Pages.ContactUs;
using Neambc.Neamb.Project.Web.UITest.Tests.Core.Login;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.ContactUs
{
    [TestFixture]
    public class ContactUsPageTest : NeambTestBaseLarge<ContactUsPage>
    {
        [TestCaseSource(typeof(ContactUsPageTestData),
            nameof(ContactUsPageTestData.TestDataSource),
            new object[] { "ToogleEnglish" })]
        [Test, Category("Core")]
        public void VerifyToogleEnglish(string title)
        {
            Page.AssertIsLoaded()
                .ClickOnEnglishToogle()
                .VerifyEnglishToogle(title);
        }
        [TestCaseSource(typeof(ContactUsPageTestData),
            nameof(ContactUsPageTestData.TestDataSource),
            new object[] { "ToogleSpanish" })]
        [Test, Category("Core")]
        public void VerifyToogleSpanish(string title)
        {
            Page.AssertIsLoaded()
                .ClickOnSpanishToogle()
                .VerifySpanishToogle(title);
        }
    }
}
