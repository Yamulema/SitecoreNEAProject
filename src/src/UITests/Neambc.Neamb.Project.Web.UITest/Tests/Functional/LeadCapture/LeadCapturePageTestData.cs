using System.Collections.Generic;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.LeadCapture
{
    public static class LeadCapturePageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static LeadCapturePageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Functional\LeadCapture\LeadCapturePageTestData");
            
        }
    }
}
