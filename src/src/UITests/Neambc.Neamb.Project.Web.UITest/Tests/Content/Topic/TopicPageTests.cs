using Neambc.Neamb.Project.Web.UITest.Pages.Topic;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Topic
{
    [TestFixture]
    public class TopicPageTests : NeambTestBaseLarge<TopicPage>
    {
        [TestCaseSource(typeof(TopicPageTestData),
            nameof(TopicPageTestData.TestDataSource),
            new object[] { "seemore" })]
        [Test, Category("Content")]
        public void CheckGtmActionContentFilterSeeMore(string SeeMoreLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionSeeMore(SeeMoreLink);
        }

        [TestCaseSource(typeof(TopicPageTestData),
                nameof(TopicPageTestData.TestDataSource),
                new object[] { "itemtaxprep" })]
        [Test, Category("Content")]
        public void CheckGtmAction5ColumnContent(string ItemTaxPrepLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionItemTaxPrep(ItemTaxPrepLink);
        }

        [TestCaseSource(typeof(TopicPageTestData),
           nameof(TopicPageTestData.TestDataSource),
           new object[] { "items" })]
        [Test, Category("Content")]
        public void CheckGtmActionContentFilter(string ItemLink)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionItems(ItemLink);
        }
    }
}
