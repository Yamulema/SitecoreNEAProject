using System.Collections.Generic;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NEALongTermCare
{
    public static class NeaLTCPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static NeaLTCPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Content\Products\NEALongTermCare\TestData");
        }
    }
}
