using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.Jdt;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests
{
    public static class TestSourceHelper
    {
        public static IDictionary<string, IEnumerable<object[]>> GetDataSource(string pathBase)
        {
            string path = "";
            string pathEnvironment = "";
            bool resultEnvironment = false;
            IDictionary<string, IEnumerable<object[]>> data = null;
            string valueSetting = System.Configuration.ConfigurationManager.AppSettings["site"];
            path = Path.Combine(TestContext.CurrentContext.TestDirectory,
                $@"{pathBase}.json");

            if (!string.IsNullOrEmpty(valueSetting))
            {
                pathEnvironment = Path.Combine(TestContext.CurrentContext.TestDirectory,
                    $@"{pathBase}{valueSetting}.json");
                if (File.Exists(pathEnvironment))
                {
                    resultEnvironment = true;
                }
            }

            if (resultEnvironment)
            {
                JsonTransformation jsonEnvironment = new JsonTransformation(pathEnvironment);
                var transformJson = jsonEnvironment.Apply(path);
                transformJson.Position = 0;
                var sr = new StreamReader(transformJson);
                var result = sr.ReadToEnd();
                data = JsonConvert
                    .DeserializeObject<Dictionary<string, IEnumerable<object[]>>>(result);
            }
            else
            {
                data = JsonConvert
                    .DeserializeObject<Dictionary<string, IEnumerable<object[]>>>(File.ReadAllText(path));
            }

            return data;
        }
    }
}
