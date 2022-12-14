using System;
using System.Linq;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Seiumb.UITests.Extensions
{
    public static class AssertingPageBaseExtensions
    {
        public static T SwitchToNewestTab<T>(this AssertingPageBase assertingPageBase) where T : class, IPage
        {
            var windowName = assertingPageBase.Driver.WindowHandles.LastOrDefault();
            if (!string.IsNullOrEmpty(windowName))
            {
                assertingPageBase.Driver.SwitchTo().Window(windowName);
            }
            return Activator.CreateInstance(typeof(T), assertingPageBase.Driver, assertingPageBase.Settings) as T;
        }
        public static T CloseCurrentTab<T>(this AssertingPageBase assertingPageBase) where T : class, IPage
        {
            assertingPageBase.Driver.Close();
            return assertingPageBase.SwitchToNewestTab<T>();
        }
    }
}
