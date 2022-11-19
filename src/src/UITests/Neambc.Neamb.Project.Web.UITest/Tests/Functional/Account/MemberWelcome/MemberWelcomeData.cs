using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Account.MemberWelcome
{
    public class MemberWelcomeData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static MemberWelcomeData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Functional\Account\MemberWelcome\MemberWelcomeData");

        }
    }
}