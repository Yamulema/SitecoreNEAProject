using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.Registration
{
    public static class RegistrationPageTestData
    {
        public static readonly IDictionary<string, IEnumerable<object[]>> _data;
        public static IEnumerable<object[]> TestDataSource(string key) => _data[key];
        static RegistrationPageTestData()
        {
            _data = TestSourceHelper.GetDataSource(@"Tests\Core\Registration\RegistrationPageTestData");
        }
    }
}
