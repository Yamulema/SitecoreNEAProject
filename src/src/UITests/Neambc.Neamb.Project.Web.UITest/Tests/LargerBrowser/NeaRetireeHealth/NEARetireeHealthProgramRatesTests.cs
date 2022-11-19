using Neambc.Neamb.Project.Web.UITest.Pages.NeaRetireeHealthPage;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.LargerBrowser.NeaRetireeHealth
{
   public class NEARetireeHealthProgramRatesTests : NeambTestBaseLarge<NEARetireeHealthProgramRatesPage>
    {
        #region Tests
        [Test, Category("Rates Page")]
        public void AllControlsExist()
        {
            Page.AssertHasAllControlsForSections(new[] { "Rates" });
                       
        }

        [Test, Category("States")]
        public void SelectAlabamaStateRange1()
        {
            Page.AssertElementExists("Rates_Title");
            Page.AssertClick("Rates_States");
            Page.AssertSetComboBoxValueByValue("Rates_States", "AL");
            Page.AssertIsLoaded();
			Page.SearchForClickableElement("Rates_Ages");
			Page.AssertClick("Rates_Ages");
			Page.AssertSetComboBoxValueByValue("Rates_Ages", "65,66,67,68,69");
			Page.AssertClick("Rates_Submit");
            Page.AssertIsLoaded();
            Page.AssertElementExists("Result_Table");
        }
		[Test, Category("States")]
		public void SelectAlabamaStateRange2() {
			Page.AssertElementExists("Rates_Title");
			Page.AssertClick("Rates_States");
			Page.AssertSetComboBoxValueByValue("Rates_States", "AL");
			Page.AssertIsLoaded();
			Page.SearchForClickableElement("Rates_Ages");
			Page.AssertClick("Rates_Ages");
			Page.AssertSetComboBoxValueByValue("Rates_Ages", "70,71,72,73,74");
			Page.AssertClick("Rates_Submit");
			Page.AssertIsLoaded();
			Page.AssertElementExists("Result_Table");
		}
		[Test, Category("States")]
		public void SelectAlabamaStateRange3() {
			Page.AssertElementExists("Rates_Title");
			Page.AssertClick("Rates_States");
			Page.AssertSetComboBoxValueByValue("Rates_States", "AL");
			Page.AssertIsLoaded();
			Page.SearchForClickableElement("Rates_Ages");
			Page.AssertClick("Rates_Ages");
			Page.AssertSetComboBoxValueByValue("Rates_Ages", "75,76,77,78,79,80");
			Page.AssertClick("Rates_Submit");
			Page.AssertIsLoaded();
			Page.AssertElementExists("Result_Table");
		}
		[Test, Category("States")]
		public void SelectCaliforniaState() {
			Page.AssertElementExists("Rates_Title");
			Page.AssertClick("Rates_States");
			Page.AssertSetComboBoxValueByValue("Rates_States", "CA");
			Page.AssertIsLoaded();
			Page.SearchForClickableElement("Rates_Ages");
			Page.SetTextBoxValue("Rates_ZipCode", "90011");
			Page.AssertClick("Rates_Ages");
			Page.AssertSetComboBoxValueByValue("Rates_Ages", "75,76,77,78,79,80");			
			Page.AssertIsLoaded();
			Page.AssertElementExists("Result_Table");
		}
		#endregion

	}
}
