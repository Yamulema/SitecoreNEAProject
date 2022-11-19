using System.Collections.Generic;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.PersonalLoanParents {
    public static class PersonalLoanParentsPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static PersonalLoanParentsPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Content\Products\PersonalLoanParents\TestData");
        }
    }
}
