using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaRetirementProgram
{
    public static class NeaRetirementProgramPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static NeaRetirementProgramPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Content\Products\NeaRetirementProgram\TestData");
        }
    }
}
