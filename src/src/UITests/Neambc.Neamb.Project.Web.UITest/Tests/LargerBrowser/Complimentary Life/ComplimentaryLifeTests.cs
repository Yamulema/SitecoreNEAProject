using Neambc.Neamb.Project.Web.UITest.Pages;
using Neambc.Neamb.Project.Web.UITest.Pages.Complimentary_Life;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.LargerBrowser.Complimentary_Life {
	public class ComplimentaryLifeTests : NeambTestBaseLarge<ComplimentaryLifePage> {

		#region Tests
		[Test, Order(1), Category("CompLife")]
		public void AllControlsExistCompLifePage() {
            Page.AssertElementExists("CompLife_h_SignIn");
            Page.AssertClick("CompLife_h_SignIn");
            Page.LoginCompLife("nea.jessica@gmail.com", "secret12");
            Page.AssertIsLoaded();
            Page.AssertHasAllControlsForSections(new[] { "CompLifeTitles" });
        }
		[Test, Order(2), Category("CompLife")]
		public void EditUpdatePageControlsExist() {
            Page.AssertElementExists("CompLife_h_SignIn");
            Page.AssertClick("CompLife_h_SignIn");
            Page.LoginCompLife("nea.jessica@gmail.com", "secret12");
            Page.AssertIsLoaded();
            Page.AssertHasAllControlsForSections(new[] { "CompLifeTitles" });
            Page.AssertClick("CompLife_yi_EditButton");
			Page.AssertIsLoaded();
			Page.AssertElementExists("CompLife_yi_InsideTitle");
			Page.AssertHasAllControlsForSections(new[] { "YourInformation" });
			Page.AssertClick("CompLife_yi_Back");
			Page.AssertIsLoaded();
			Page.AssertElementExists("CompLife_yi_Title");
		}
		[Test, Order(3), Category("CompLife Add Beneficiary Page")]
		public void AddBeneficiaryPageControlsExist() {
            Page.AssertElementExists("CompLife_h_SignIn");
            Page.AssertClick("CompLife_h_SignIn");
            Page.LoginCompLife("nea.jessica@gmail.com", "secret12");
            Page.AssertIsLoaded();
            Page.AssertHasAllControlsForSections(new[] { "CompLifeTitles" });
			Page.AssertClick("CompLife_b_AddButton");
			Page.AssertIsLoaded();
			Page.AssertElementExists("CompLife_b_AddPageTitle");
			Page.AssertHasAllControlsForSections(new[] { "AddBeneficiaryPage" });
			Page.AssertClick("CompLife_b_AddPageBack");
			Page.AssertIsLoaded();
			Page.AssertElementExists("CompLife_yi_Title");
		}
		#endregion
	}
}
