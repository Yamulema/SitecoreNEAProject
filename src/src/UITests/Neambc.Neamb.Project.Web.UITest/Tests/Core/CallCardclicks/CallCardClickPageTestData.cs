using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.CallCardclicks
{
    public static class CallCardClickPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static CallCardClickPageTestData()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory,
                $@"Tests\Core\CallCardClicks\TestData.json");
            _data = JsonConvert
                .DeserializeObject<Dictionary<string, IEnumerable<object[]>>>(File.ReadAllText(path));
        }
    }
}
