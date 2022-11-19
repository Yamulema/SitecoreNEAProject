using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.Jdt;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.PersonalLoanStudents {
    public static class PersonalLoanStudentsPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];

        static PersonalLoanStudentsPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Content\Products\PersonalLoanStudents\TestData");
        }
    }
}
