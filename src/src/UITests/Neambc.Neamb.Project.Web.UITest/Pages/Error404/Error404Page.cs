using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Error404
{
    public class Error404Page : NeambPage
    {
        public Error404Page(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public Error404Page(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public Error404Page(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        public new Error404Page AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Body"
            });
            return this;
        }
    }
}
