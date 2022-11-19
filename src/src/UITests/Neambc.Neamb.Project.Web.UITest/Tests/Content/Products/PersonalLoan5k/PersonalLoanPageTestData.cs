using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.PersonalLoan5k {
    public static class PersonalLoanPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static PersonalLoanPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Content\Products\PersonalLoan5k\TestData");
        }
    }
}
