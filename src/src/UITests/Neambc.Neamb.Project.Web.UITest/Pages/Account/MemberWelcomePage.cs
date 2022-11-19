using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using NUnit.Framework;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using System.Threading;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Account
{
    public class MemberWelcomePage : NeambPage
    {

        #region ControlKeys
        private const string PersonalCodeInput = "PersonalCodeInput";
        private const string FormSubmit = "FormSubmit";
        private const string ServerErrorMessage = "ServerError";
        #endregion

        public MemberWelcomePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public MemberWelcomePage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }

        public MemberWelcomePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public new MemberWelcomePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[]
            {
                "Form"
            });
            return this;
        }

        public MemberWelcomePage FillForm(string mdsId)
        {
            AssertSetTextBoxValue(PersonalCodeInput, mdsId);
            return this;
        }

        public MemberWelcomePage ClickToSubmitButton()
        {
            AssertClick(FormSubmit, "Could not find the submit button");
            return this;
        }

        public MemberWelcomePage CheckMdsIdFromHidden(string mdsId)
        {
            var mdsIdInput = AssertElementExists(PersonalCodeInput, "MdsID input hidden not found").GetAttribute("value");
            AssertIsTrue(mdsIdInput.Contains(mdsId), "MdsId from input hidden is different than expected");
            return this;
        }

        public MemberWelcomePage CheckIfElementHasPlaceholder(string idElement, string placeholderExpected)
        {
            Thread.Sleep(2000);
            var zipInput = Driver.FindElement(By.Id(idElement));
            var placeholderValue = zipInput.GetAttribute("placeholder");
            AssertIsTrue(placeholderValue.ToUpper().Contains(placeholderExpected.ToUpper()));
            return this;
        }

        public MemberWelcomePage CheckErrorMessage()
        {
            Thread.Sleep(2000);
            var errorMessageVisible = AssertElementExists(ServerErrorMessage, "Server Error Message not found").Displayed;
            AssertIsTrue(errorMessageVisible, "Server error Message is not visible");
            return this;
        }
    }
}
