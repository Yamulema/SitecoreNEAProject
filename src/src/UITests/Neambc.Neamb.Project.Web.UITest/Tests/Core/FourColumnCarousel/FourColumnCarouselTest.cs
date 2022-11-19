using Neambc.Neamb.Project.Web.UITest.Pages.FourColumnCarousel;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.FourColumnCarousel
{
    [TestFixture]
    public class FourColumnCarouselTest : NeambTestBaseLarge<FourColumnCarouselPage>
    {

        [TestCaseSource(typeof(FourColumnCarouselTestData),
            nameof(FourColumnCarouselTestData.TestDataSource),
            new object[] { "itemtaxprep" })]
        [Test, Category("Core")]
        public void CheckGtmActionTravel(string ItemTaxPrepLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionItemTaxPrep(ItemTaxPrepLink);
        }

        [TestCaseSource(typeof(FourColumnCarouselTestData),
            nameof(FourColumnCarouselTestData.TestDataSource),
            new object[] { "items" })]
        [Test, Category("Core")]
        public void CheckGtmActionRetirement(string ItemTaxLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionItems(ItemTaxLink);
        }
    }
}