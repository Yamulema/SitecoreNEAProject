using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;


namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.AfiliateRelations
{
    public static class AfiliateRelationsSpecialistData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];

        static AfiliateRelationsSpecialistData()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory,
                $@"Tests\Functional\AfiliateRelations\TestData.json");
            _data = JsonConvert
                .DeserializeObject<Dictionary<string, IEnumerable<object[]>>>(File.ReadAllText(path));
        }
    }
}
