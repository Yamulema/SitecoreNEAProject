using System.Collections.Generic;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.CarouselC014
{
    public static class CarouselC014PageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static CarouselC014PageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Core\CarouselC014\TestData");
        }
    }
}