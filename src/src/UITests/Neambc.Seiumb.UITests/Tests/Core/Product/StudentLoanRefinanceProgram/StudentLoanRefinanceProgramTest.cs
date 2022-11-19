using Neambc.Seiumb.UITests.Pages.Products.StudentLoanRefinanceProgram;
using NUnit.Framework;
using Neambc.Seiumb.UITests.Extensions;


namespace Neambc.Seiumb.UITests.Tests.Core.Product.StudentLoanRefinanceProgram
{
    [TestFixture]
    public class StudentLoanRefinanceProgramTest : SeiumbTestBaseLarge<StudentLoanRefinanceProgramPage>
    {
        /// <summary>
        /// Test gtm tracking in cold user
        /// </summary>
        [TestCaseSource(typeof(StudentLoanRefinanceProgramData),
            nameof(StudentLoanRefinanceProgramData.TestDataSource),
            new object[] { "Test_Link_Hot_User" })]

        [Test, Category("Core")]
        public void VerifyLinkHotUser(string username, string password, string url)
        {
            Page.Login<StudentLoanRefinanceProgramPage>(username, password)
                .ClickCtaButtonHotState()
                .SwitchToNewestTab<StudentLoanRefinanceProgramPage>()
                .AssertHotStateCtaButton(url);
        }
        
    }
}
