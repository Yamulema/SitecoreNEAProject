using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Redirect
{
    public static class RedirectTestData
    {
        //private string Http = HttpContext.Current.Request.IsSecureConnection

        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static RedirectTestData()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory,
                $@"Tests\Redirect\RedirectTestData.json");
            _data = JsonConvert
                .DeserializeObject<Dictionary<string, IEnumerable<object[]>>>(File.ReadAllText(path));
        }
    }
}
