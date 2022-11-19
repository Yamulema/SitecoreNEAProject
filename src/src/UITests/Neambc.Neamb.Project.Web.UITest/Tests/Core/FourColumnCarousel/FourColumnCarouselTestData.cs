using System.Collections.Generic;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.FourColumnCarousel
{
    public static class FourColumnCarouselTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static FourColumnCarouselTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Core\FourColumnCarousel\TestData");
        }
    }
}