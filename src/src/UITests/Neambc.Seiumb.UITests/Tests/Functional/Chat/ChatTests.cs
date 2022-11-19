using Neambc.Seiumb.UITests.Pages.Home;
using Neambc.Seiumb.UITests.Pages.Products.GroupTermLifeInsurance;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Functional.Chat
{
    [TestFixture]
    public class ChatTests : SeiumbTestBaseLarge<GroupTermLifeInsurancePage>
    {
        [Test, Category("Functional")]
        public void VerityChatIcon()
        {
            Page.VerifyChatIcon();
        }

        [TestCaseSource(typeof(ChatTestData),
           nameof(ChatTestData.TestDataSource),
           new object[] { "Home" })]
        [Test, Category("Functional")]
        public void VerityChatIconOnHome(string url)
        {
            Page.GoToPage<HomePage>(url)
                .VerifyChatIcon();
        }
    }
}
