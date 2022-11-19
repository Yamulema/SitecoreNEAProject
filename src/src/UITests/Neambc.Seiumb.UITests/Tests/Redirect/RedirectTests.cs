using Neambc.Seiumb.UITests.Pages.Home;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Redirect
{
    [TestFixture]
    public class RedirectTests : SeiumbTestBaseLarge<HomePage>
    {
        /// <summary>
        /// RedirectModule
        /// </summary>
        [TestCaseSource(typeof(RedirectTestData),
            nameof(RedirectTestData.TestDataSource),
            new object[] { "RedirectModule" })]
        [Test, Category("Login")]
        public void RedirectModule(string request, string redirect)
        {
            Page.GoToPage(request)
                .AssertUrl(redirect);
        }
    }
}
