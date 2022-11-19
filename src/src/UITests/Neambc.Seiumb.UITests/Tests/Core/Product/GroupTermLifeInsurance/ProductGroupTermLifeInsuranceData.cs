using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Product
{
    public static class ProductGroupTermLifeInsuranceData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static ProductGroupTermLifeInsuranceData()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory,
                $@"Tests\Core\Product\GroupTermLifeInsurance\ProductGroupTermLifeInsurance.json");
            _data = JsonConvert
                .DeserializeObject<Dictionary<string, IEnumerable<object[]>>>(File.ReadAllText(path));
        }
    }
}
