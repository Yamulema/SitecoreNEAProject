using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using System.Threading;

namespace Neambc.Neamb.Project.Web.UITest.Pages.CompLife
{
    public class CompLifePage : NeambPage
    {
        #region ControlKeys
        private const string Update_Profile = "Update_Profile";
        private const string Profile_FirstName = "Profile_FirstName";
        private const string Profile_LastName = "Profile_LastName";
        private const string Update= "Update";
        private const string Profile_Phone = "Profile_Phone";
        private const string Submit_Button = "CompLife_Save";
        private const string Del_Bene1 = "Del_Bene1";
        private const string CompLife_Add = "CompLife_Add";
        private const string Add_Bene = "Add_Bene";
        private const string Payout = "PayoutPercent";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string Relationship = "RelationshipCode";
        private const string Housing = "Housing";
        private const string Total_Percent = "Totalpayout";
        private const string Other_Entity = "Otherentity";
        private const string Other_Entity_Name = "OtherentityName";

        private static int Timeout => 500;
        #endregion
        #region Constructor
        public CompLifePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }
        public CompLifePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public CompLifePage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public CompLifePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "headline-hero bg-blue"
            });
            return this;
        }
        public new CompLifePage Edit_Profile(string FirstName, string LastName, string Phone)
        {
            AssertClick(Update_Profile, timeoutSeconds: Timeout);
            AssertHasAllControlsForSections(new[]
           {
                "Edit/Update Your Information",
            });

            Thread.Sleep(1000);
            AssertElementExists(Profile_FirstName).Clear();
            AssertSetTextBoxValue(Profile_FirstName, FirstName);
            AssertElementExists(Profile_LastName).Clear();
            AssertSetTextBoxValue(Profile_LastName, LastName);
            AssertElementExists(Profile_Phone).Clear();
            AssertSetTextBoxValue(Profile_Phone, Phone);
            AssertClick(Update, timeoutSeconds: Timeout);

            return this;
        }

        public new CompLifePage Del_Beneficiary()
        {
            var Totalpercent = GetElementFromControlKey(Total_Percent).Text;
            if (!Totalpercent.Equals("0"))
            {
                AssertClick(Del_Bene1, timeoutSeconds: Timeout);
                AssertClick(Del_Bene1, timeoutSeconds: Timeout);

            }
            return this;
        }

        public new CompLifePage CompLife_Submit_GTM(string clickPrimaryFunction, string housing)
        {

            AssertSetTextBoxValue(Housing, housing);
            Thread.Sleep(1000);
            var Check_Submit = GetElementFromControlKey(Submit_Button);
            
            var buttonOnClick = GetElementFromControlKey(Submit_Button)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonOnClick,clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");
            
            return this;
        }

        public new CompLifePage CompLife_Add_Bene(string FirstName, string LastName, string relationship, string payout)
        {
            AssertClick(CompLife_Add, timeoutSeconds: Timeout);
            AssertHasAllControlsForSections(new[]
           {
                "Add Beneficiary",
            });

            Thread.Sleep(1000);
            AssertSetTextBoxValue(FirstName, FirstName);
            AssertSetTextBoxValue(LastName, LastName);
            AssertSetTextBoxValue(Relationship, relationship);
            AssertSetTextBoxValue(Payout, payout);
            AssertClick(Add_Bene, timeoutSeconds: Timeout);

            return this;
        }
       
        public new CompLifePage CompLife_Add_Bene_Other(string otherentityname, string payout)
        {
            AssertClick(CompLife_Add, timeoutSeconds: Timeout);
            AssertHasAllControlsForSections(new[]
           {
                "Add Beneficiary",
            });

            Thread.Sleep(1000);
            AssertClick(Other_Entity, timeoutSeconds: 10);
            AssertSetTextBoxValue(Other_Entity_Name, Other_Entity_Name);
            AssertSetTextBoxValue(Payout, payout);
            AssertClick(Add_Bene, timeoutSeconds: Timeout);

            return this;
        }

        public new CompLifePage CompLife_Submit(string housing)
        {
            AssertSetTextBoxValue(Housing, housing);
            Thread.Sleep(1000);
            var Check_Submit = GetElementFromControlKey(Submit_Button);
            AssertClick(Submit_Button, timeoutSeconds: Timeout);
            
            return this;
        }

    }
}
