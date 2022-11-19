using Neambc.Neamb.Project.Web.UITest.Pages.ProductLinkPost;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Products.ProductLinkPost
{
    [TestFixture]
    public class ProductLinkPostTests : NeambTestBaseLarge<ProductLinkPostPage>
    {
        /// <summary>
        /// Test hot state when the user clicks on a primary cta button and has some post data
        /// </summary>
        /// <param name="title"></param>
        [TestCaseSource(typeof(ProductLinkPostData),
            nameof(ProductLinkPostData.TestDataSource),
            new object[] { "TitleCard" })]
        [Test, Category("Core")]
        public void TestVerifyTitleCard(string title)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.VerifyTitleCard(title);
        }
    }
}
