using Neambc.Neamb.Project.Web.UITest.Pages.Profile;
using Neambc.Neamb.Project.Web.UITest.Tests.Core.Registration;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.Login
{

    [TestFixture]
    public class ProfileClicksTests : NeambTestBaseLarge<ProfileClicks>
    {
         [TestCaseSource(typeof(ProfileClicksTestData),
            nameof(ProfileClicksTestData.TestDataSource),
            new object[] { "AccountButton" })]

         [Test, Category("core")]

           public void CheckGtmActionProfile(string clickFunction)
           {
             Page.AssertIsLoaded()
                 .AssertIsLoaded()
     
                 .CheckGtmActionProfile(clickFunction);
           }
          
    }
}
