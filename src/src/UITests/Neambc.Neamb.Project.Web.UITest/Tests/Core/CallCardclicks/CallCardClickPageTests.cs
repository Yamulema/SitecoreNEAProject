using Neambc.Neamb.Project.Web.UITest.Pages.CallCardClicks;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.CallCardclicks
{ 
    [TestFixture]
    public class CallCardClickPageTests : NeambTestBaseLarge<CallCardClickPage>
    {
        [TestCaseSource(typeof(CallCardClickPageTestData),
            nameof(CallCardClickPageTestData.TestDataSource),
            new object[] { "Test_ColdAction" })]

        [Test, Category("Core")]
        public void ValidateGtmActionButton(string gtmAction)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.AssertIsLoaded().CheckGtmActionCold(gtmAction);
        }

    }
}

