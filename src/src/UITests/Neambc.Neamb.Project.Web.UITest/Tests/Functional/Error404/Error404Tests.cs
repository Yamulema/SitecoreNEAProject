using Neambc.Neamb.Project.Web.UITest.Pages.Error404;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Error404
{
    [TestFixture]
    public class Error404Tests : NeambTestBaseLarge<Error404Page>
    {
        /// <summary>
        /// Redirect
        /// </summary>
        [TestCaseSource(typeof(Error404TestData),
            nameof(Error404TestData.TestDataSource),
            new object[] { "Redirect" })]
        [Test]
        public void Redirect(string request, string redirect)
        {
            Page.GoToPage<Error404Page>(request)
                .AssertIsLoaded();
        }
    }
}
