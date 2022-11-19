using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.ClickSave
{
    public class LoginClickandSave : NeambPageBase
    {

        public LoginClickandSave(
            IWebDriver driver,
            ISettings settings) : base(
                "LoginClickSave",
                "/emailnxjaccess?ref=000000991&url=https%3A%2F%2Fneamb.affinityperks.com",
                driver, settings)
        {

        }
        public void EnterPassword(string password)
        {
           AssertSetTextBoxValue("ClickSave_Password", password);
            AssertElementExists("ClickSave_Submit");
           
        }

        
    }
}
