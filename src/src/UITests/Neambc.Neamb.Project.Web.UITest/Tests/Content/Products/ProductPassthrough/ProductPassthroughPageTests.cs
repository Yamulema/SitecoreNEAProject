using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaAutoBuying;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.ProductPassthrough;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.ProductPassthrough
{
    [TestFixture]
    public class ProductPassthroughPageTests : NeambTestBaseLarge<ProductPassthroughPage>
    {
        /// <summary>
        /// AutoBuying
        /// </summary>
        [TestCaseSource(typeof(ProductPassthroughPageTestData),
            nameof(ProductPassthroughPageTestData.TestDataSource),
            new object[] { "Auto" })]
        [Test, Category("Content")]
        public void Auto(string url, string firstname, string mdsId)
        {
            Page
            //.SetParameterUrl<ProductPassthroughPage>("ref", mdsid)
            //.SetParameterUrl<ProductPassthroughPage>("productcode", productcode)
            //.GoToPage<ProductPassthrough>(url)
            .GoToPage<IcePage>(url)
            .AssertHotState(mdsId);
            
        }

    }
}
