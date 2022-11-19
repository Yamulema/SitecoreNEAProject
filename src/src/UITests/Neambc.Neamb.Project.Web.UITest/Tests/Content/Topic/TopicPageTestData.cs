using System.Collections.Generic;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Topic
{
    public static class TopicPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static TopicPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Content\Topic\TestData");
        }
    }
}
