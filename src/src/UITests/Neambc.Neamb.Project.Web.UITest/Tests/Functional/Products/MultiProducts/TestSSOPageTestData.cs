using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Products.MultiProducts {
    public static class TestSSOPageTestData {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static TestSSOPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Functional\Products\MultiProducts\TestData");
        }
    }
}
