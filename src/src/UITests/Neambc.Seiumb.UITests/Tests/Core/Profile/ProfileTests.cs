using Neambc.Seiumb.UITests.Pages.Home;
using Neambc.Seiumb.UITests.Pages.Profile;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Profile
{
    [TestFixture]
    public class ProfileTests : SeiumbTestBaseLarge<ProfilePage>
    {

        [TestCaseSource(typeof(ProfileTestData),
            nameof(ProfileTestData.TestDataSource),
            new object[] { "Test_GTM_ProfileUpdate" })]
        [Test, Category("GTM")]
        public void ValidateGTMUpdateProfile(string username, 
            string password, string urlProfile, 
            string newFirstName,
            string onClickFunction)
        {
            Page.AssertIsLoaded()
                 .Login<HomePage>(username, password)
                 .GoToPage<ProfilePage>(urlProfile)
                 .UpdateProfile(newFirstName);
        }
    }
}
