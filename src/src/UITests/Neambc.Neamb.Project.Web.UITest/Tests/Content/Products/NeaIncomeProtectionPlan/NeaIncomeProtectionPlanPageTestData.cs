using System.Collections.Generic;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaIncomeProtectionPlan
{
    public static class NeaIncomeProtectionPlanPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static NeaIncomeProtectionPlanPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Content\Products\NeaIncomeProtectionPlan\TestData");
        }
    }
}
