using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.HRBlockTax
{
    public static class HRBlockTaxPageTestData 
	{
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static HRBlockTaxPageTestData()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory,
                $@"Tests\Content\Products\HRBlockTax\TestData.json");
            _data = JsonConvert
                .DeserializeObject<Dictionary<string, IEnumerable<object[]>>>(File.ReadAllText(path));
        }
    }
}
