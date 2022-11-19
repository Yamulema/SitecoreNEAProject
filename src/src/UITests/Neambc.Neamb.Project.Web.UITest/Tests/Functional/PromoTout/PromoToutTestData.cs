using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.PromoTout
{
    public static class PromoToutTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static PromoToutTestData()
        {
			_data = TestSourceHelper.GetDataSource(@"Tests\Functional\PromoTout\PromoToutTestData");			
        }
    }
}
