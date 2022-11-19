using Neambc.Neamb.Project.Web.UITest.Tests.Core.Registration;
using Neambc.Seiumb.UITests.Pages.Registration;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Registration
{
    [TestFixture]
    public class RegistrationPageTests : SeiumbTestBaseLarge<RegistrationPage>
    {
        /// <summary>
        /// Test duplicate registration
        /// </summary>
        [TestCaseSource(typeof(RegistrationPageTestData),
            nameof(RegistrationPageTestData.TestDataSource),
            new object[] { "Test_DuplicateRegistration" })]

        [Test, Category("Core")]
        public void SEIUExecuteDuplicateProcess(string firstName, string lastName, string birthDate,string address, string city, string state, 
            string zip, string phone, string email, string password)
        {
            Page.AssertIsLoaded().SubmitDuplicateRegistration(firstName, lastName, birthDate, address, city, state, zip, phone, email, password)
                .ProcessDuplicateRegistrationWizard1(password)
                .ProcessDuplicateRegistrationWizard2()
                .ProcessDuplicateRegistrationWizard3();
        }
    }
}
