using Neambc.Neamb.Project.Web.UITest.Tests.Core.Registration;
using Neambc.Seiumb.UITests.Pages.Tokens;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Token
{
    [TestFixture]
    public class TokenPageTests : SeiumbTestBaseLarge<TokenPage>
    {
        /// <summary>
        /// Test token in cold user
        /// </summary>
        [TestCaseSource(typeof(TokenPageTestData),
            nameof(TokenPageTestData.TestDataSource),
            new object[] { "Test_Token_Cold_User" })]

        [Test, Category("Core")]
        public void VerifyTokenValueCold(string inputText)
        {
            Page.AssertIsLoaded().CheckContentToken(inputText);
        }
        /// <summary>
        /// Test token in warm user
        /// </summary>
        [TestCaseSource(typeof(TokenPageTestData),
            nameof(TokenPageTestData.TestDataSource),
            new object[] { "Test_Token_Warm_User" })]

        [Test, Category("Core")]
        public void VerifyTokenValueWarm(string inputText, string mdsId)
        {
            Page.AssertIsLoaded().SetAsWarm<TokenPage>(mdsId).CheckContentToken(inputText);
        }
        /// <summary>
        /// Test token in hot user
        /// </summary>
        [TestCaseSource(typeof(TokenPageTestData),
            nameof(TokenPageTestData.TestDataSource),
            new object[] { "Test_Token_Hot_User" })]

        [Test, Category("Core")]
        public void VerifyTokenValueHot(string inputText, string username, string password)
        {
            Page.AssertIsLoaded().Login<TokenPage>(username, password).CheckContentToken(inputText);
        }
    }
}
