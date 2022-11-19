using System.Collections.Generic;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.FreqAQ
{
    public static class FreqAQPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static FreqAQPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Core\FreqAQ\TestData");
        }
    }
}

