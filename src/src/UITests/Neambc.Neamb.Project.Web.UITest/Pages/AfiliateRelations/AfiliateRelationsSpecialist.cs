using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using System.Threading;

namespace Neambc.Neamb.Project.Web.UITest.Pages.AfiliateRelations
{
    public class AfiliateRelationsSpecialist : NeambPage
    {
        private const string State_Dropdown = "State_Dropdown";
        private const string Submit_Button = "Submit_Button";
        private const string Representative_Name = "Representative_Name";

        #region Constructor
        public AfiliateRelationsSpecialist(IWebDriver driver, ISettings settings)
            : base("AfiliateRelationsSpecialist", driver, settings)
        {

        }
        #endregion

        public new AfiliateRelationsSpecialist AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "header",
                "form"
            });
            return this;
        }

        public new AfiliateRelationsSpecialist AssertSelectState(string selectText)
        {
            AssertSetComboBoxValueByText(State_Dropdown, selectText, timeoutSeconds: 1);
            return this;
        }

        public new AfiliateRelationsSpecialist AssertClickOnSubmit()
        {
            AssertControlClick(Submit_Button);
            return this;
        }

        public new AfiliateRelationsSpecialist AssertValidateFinalContent()
        {
            Thread.Sleep(2000);
            AssertElementTextEquals(Representative_Name, "Antonio Galindo");
            return this;
        }
    }
}
