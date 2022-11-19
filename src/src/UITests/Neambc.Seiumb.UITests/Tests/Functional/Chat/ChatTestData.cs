using System.Collections.Generic;

namespace Neambc.Seiumb.UITests.Tests.Functional.Chat
{
    public class ChatTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static ChatTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Functional\Chat\TestData");
        }
    }
}
