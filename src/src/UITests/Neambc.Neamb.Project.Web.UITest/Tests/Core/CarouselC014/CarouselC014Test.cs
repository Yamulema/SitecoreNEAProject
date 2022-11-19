using Neambc.Neamb.Project.Web.UITest.Pages.CarouselC014;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.CarouselC014
{
    [TestFixture]
    public class CarouselC014PageTest : NeambTestBaseLarge<CarouselC014Page>
    {
        [TestCaseSource(typeof(CarouselC014PageTestData),
            nameof(CarouselC014PageTestData.TestDataSource),
            new object[] { "itemtaxprep" })]
        [Test, Category("Content")]
        public void CheckGtmActionTopicLifeInsurance(string ItemTaxPrepLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionItemTaxPrep(ItemTaxPrepLink);
        }

        [TestCaseSource(typeof(CarouselC014PageTestData),
            nameof(CarouselC014PageTestData.TestDataSource),
            new object[] { "items" })]
        [Test, Category("Content")]
        public void CheckGtmActionTopicQuestion(string ItemTaxLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionItems(ItemTaxLink);
        }
    }
}