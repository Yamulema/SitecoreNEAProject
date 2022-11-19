using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Guide
{
    public class GuidePage : NeambPageBase
    {
        public GuidePage(
                IWebDriver driver,
                ISettings settings) : base(
                    "GuidePage",
                    "/Guides/Homeowners-Insurance-Guide", driver, settings)
        { }

        public void LoginSS(string username, string password)
        {
            AssertSetTextBoxValue("LoginPage.LoginPage_Form_EmailTextbox", username);
            AssertSetTextBoxValue("LoginPage.LoginPage_Form_PasswordTextbox", password);
            AssertClick("LoginPage.LoginPage_Form_SignInButton");
        }

     }
}

