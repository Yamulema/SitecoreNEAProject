using Neambc.Neamb.Project.Web.UITest.Pages.LeadCapture;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.LeadCapture
{
    [TestFixture]
    public class LeadCapturePageTests : NeambTestBaseLarge<LeadCapturePage>
    {
        [Test]
        public void VerifyFormPresence()
        {
            Page.AssertIsLoaded();
        }

        [TestCaseSource(typeof(LeadCapturePageTestData),
            nameof(LeadCapturePageTestData.TestDataSource),
            new object[] { "Test_FirstName" })]
        [Test]
        public void ValidateFirstName(string text, string errorMessage)
        {
            Page.FillFirstNameField(text)
                .ClickSubmitForm()
                .AssertFirstNameErrors(errorMessage);
        }

        [TestCaseSource(typeof(LeadCapturePageTestData),
           nameof(LeadCapturePageTestData.TestDataSource),
           new object[] { "Test_LastName" })]
        [Test]
        public void ValidateLastName(string text, string errorMessage)
        {
            Page.FillLastNameField(text)
                .ClickSubmitForm()
                .AssertLastNameErrors(errorMessage);
        }

        [TestCaseSource(typeof(LeadCapturePageTestData),
           nameof(LeadCapturePageTestData.TestDataSource),
           new object[] { "Test_Email" })]
        [Test]
        public void ValidateEmail(string text, string errorMessage)
        {
            Page.FillEmailField(text)
                .ClickSubmitForm()
                .AssertEmailErrors(errorMessage);
        }

        [TestCaseSource(typeof(LeadCapturePageTestData),
           nameof(LeadCapturePageTestData.TestDataSource),
           new object[] { "Test_Address" })]
        [Test]
        public void ValidateAddress(string text, string errorMessage)
        {
            Page.FillAddressField(text)
                .ClickSubmitForm()
                .AssertAddressErrors(errorMessage);
        }

        [TestCaseSource(typeof(LeadCapturePageTestData),
           nameof(LeadCapturePageTestData.TestDataSource),
           new object[] { "Test_State" })]
        [Test]
        public void ValidateState(string text, string errorMessage)
        {

            if (string.IsNullOrEmpty(text))
            {
                Page.ClickSubmitForm()
                .AssertStateErrors(errorMessage);
            }
            else {
                Page.FillStateField(text)
                    .ClickSubmitForm()
                    .AssertStateErrors(errorMessage);
            }
        }

        [TestCaseSource(typeof(LeadCapturePageTestData),
           nameof(LeadCapturePageTestData.TestDataSource),
           new object[] { "Test_City" })]
        [Test]
        public void ValidateCity(string text, string errorMessage)
        {
            Page.FillCityField(text)
                .ClickSubmitForm()
                .AssertCityErrors(errorMessage);
        }

        [TestCaseSource(typeof(LeadCapturePageTestData),
           nameof(LeadCapturePageTestData.TestDataSource),
           new object[] { "Test_ZipCode" })]
        [Test]
        public void ValidateZipCode(string text, string errorMessage)
        {
            Page.FillZipCodeField(text)
                .ClickSubmitForm()
                .AssertZipCodeErrors(errorMessage);
        }

        [TestCaseSource(typeof(LeadCapturePageTestData),
           nameof(LeadCapturePageTestData.TestDataSource),
           new object[] { "Test_Phone" })]
        [Test]
        public void ValidatePhone(string text, string errorMessage)
        {
            Page.FillPhoneField(text)
                .ClickSubmitForm()
                .AssertPhoneErrors(errorMessage);
        }

        [TestCaseSource(typeof(LeadCapturePageTestData),
           nameof(LeadCapturePageTestData.TestDataSource),
           new object[] { "Test_Warm" })]
        [Test]
        public void ValidateFormWarmUser(string mdsId, 
            string firstName, string lastName, string email, string address, string city,
            string state, string zipCode, string phone)
        {
            Page.SetAsWarm<LeadCapturePage>(mdsId)
                .AssertIsLoaded()
                .AssertWarmUserData(firstName, lastName, email, address, city, state, zipCode, phone);
        }
    }
}
