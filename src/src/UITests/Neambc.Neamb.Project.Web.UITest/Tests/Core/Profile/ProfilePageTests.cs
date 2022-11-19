using Neambc.Neamb.Project.Web.UITest.Pages.Profile;
using Neambc.Neamb.Project.Web.UITest.Tests.Core.Registration;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.Login
{
    [TestFixture]
    public class ProfilePageTests : NeambTestBaseLarge<ProfilePage>
    {
        /// <summary>
        /// 
        /// </summary>
        [TestCaseSource(typeof(ProfilePageTestData),
            nameof(ProfilePageTestData.TestDataSource),
            new object[] { "Test_EmailValidation" })]

        [Test, Category("Core")]
        public void CheckErrorEmailField(string email,string username, string password)
        {
            Page.ClickOnSignInLink()
                .SignIn<ProfilePage>(username, password)
                .AssertIsLoaded()
                .CheckErrorEmailField(email);
        }

        
    }
}
