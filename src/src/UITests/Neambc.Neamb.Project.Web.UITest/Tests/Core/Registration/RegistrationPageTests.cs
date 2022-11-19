using Neambc.Neamb.Project.Web.UITest.Pages.Registration;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.Registration
{
    [TestFixture]
    public class RegistrationPageTests : NeambTestBaseLarge<RegistrationPage>
    {
        /// <summary>
        /// Check validation in email field
        /// </summary>
        [TestCaseSource(typeof(RegistrationPageTestData),
            nameof(RegistrationPageTestData.TestDataSource),
            new object[] { "Test_EmailValidation" })]

        [Test, Category("Core")]
        public void CheckErrorEmailField(string email)
        {
            Page.AssertIsLoaded().CheckErrorEmailField(email);
        }

        /// <summary>
        /// Test duplicate registration
        /// </summary>
        [TestCaseSource(typeof(RegistrationPageTestData),
            nameof(RegistrationPageTestData.TestDataSource),
            new object[] { "Test_DuplicateRegistration" })]

        [Test, Category("Core")]
        public void ExecuteDuplicateProcess(string firstName, string lastName, string birthDateMonth, 
            string birthDateDay, string birhDateYear, string address, string city, string state, 
            string zip, string phone, string email, string password)
        {
            Page.AssertIsLoaded().SubmitStep1(firstName, lastName, birthDateMonth, birthDateDay,
                    birhDateYear, address, city, state, zip, phone, email, password)
                .SubmitStep2(firstName, lastName, birthDateMonth, birthDateDay,
                    birhDateYear, address, city, state, zip, phone, email, password)
                .ProcessDuplicateRegistrationWizard1(password)
                .ProcessDuplicateRegistrationWizard2()
                .ProcessDuplicateRegistrationWizard3();
        }
    }
}
