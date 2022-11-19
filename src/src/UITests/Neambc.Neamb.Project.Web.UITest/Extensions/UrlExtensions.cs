using System;
using System.Threading;
using NUnit.Framework;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Extensions
{
    public static class UrlExtensions
    {
        public static T IsValidUrl<T>(this AssertingPageBase assertingPageBase) where T : class, IPage
        {
            var urlOpened = assertingPageBase.Driver.Url;
            Uri uriResult;

            var validUrl = Uri.TryCreate(urlOpened, UriKind.Absolute, out uriResult)
                           && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            Assert.IsTrue(validUrl, "The url is not valid " + urlOpened);
            return Activator.CreateInstance(typeof(T), assertingPageBase.Driver, assertingPageBase.Settings) as T;
        }

        public static T UrlContainsString<T>(this AssertingPageBase assertingPageBase, string urlExpected, string param = "") where T : class, IPage
        {
            Thread.Sleep(3000);

            var urlOpened = assertingPageBase.Driver.Url;
            if (!string.IsNullOrEmpty(param))
            {
                urlExpected += param;
            }

            Assert.IsTrue(urlOpened.Contains(urlExpected), "Url " + urlExpected + " was expected, got " + urlOpened + " instead");
            return Activator.CreateInstance(typeof(T), assertingPageBase.Driver, assertingPageBase.Settings) as T;
        }
    }
}
