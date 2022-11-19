using Neambc.Neamb.Project.Web.UITest.Pages.Profile;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.LargerBrowser.ProfileAndPassword {
	[TestFixture]
	public class ProfileandPasswordTests : NeambTestBaseLarge<ProfilePage> {

        #region Tests
        [Test, Category("Profile")]
        public void AllControlsExistProfilePage()
        {
            Page.AssertHasAllControlsForSections(new[] { "HeaderProfile" });
        }
  //      [Test, Category("Profile")]
		//public void ChangePassword() {
  //          Page.AssertClick("ProfilePage.Profile_h_SignIn");
  //          Page.LoginProfile(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword);
  //          Page.AssertIsLoaded();
  //          Page.AssertElementExists("ProfilePage.Prof_l_FirstName");
  //          Page.AssertSetTextBoxValue("ProfilePage.Profile_p_CurrentPwd", "secret12");
  //          Page.AssertSetTextBoxValue("ProfilePage.Profile_p_NewPwd", "secret13");
  //          Page.AssertSetTextBoxValue("ProfilePage.Profile_p_ConfirmPwd", "secret13");
  //          Page.MemorizeExistingTabs();
  //          Page.TryClick("Profile_p_SavePwd");
  //          Page.SwitchToNewestTab();
  //          Page.AssertIsLoaded();
  //          Page.AssertElementExists("ProfilePage.Profile_t_Modal");
  //          Page.TryClick("ProfilePage.Profile_p_closemodal");
  //          Page.ReturnToPriorTab(true);
  //          Page.Logout();

            //Page.AssertIsLoaded();
            //Page.Login(NeambPageBase.JessicaEmail, "secret13", "ProfilePage.Profile_h_SignIn");
            //Page.AssertElementExists("ProfilePage.Profile_h_title");
            //Page.AssertElementExists("ProfilePage.Profile_p_CurrentPwd");
            //Page.AssertSetTextBoxValue("ProfilePage.Profile_p_CurrentPwd", "secret13");
            //Page.AssertSetTextBoxValue("ProfilePage.Profile_p_NewPwd", NeambPageBase.JessicaPassword);
            //Page.AssertSetTextBoxValue("ProfilePage.Profile_p_ConfirmPwd", NeambPageBase.JessicaPassword);
            //Page.TryClick("ProfilePage.Profile_p_SavePwd");
            //Page.AssertIsLoaded();
            //Page.AssertElementExists("ProfilePage.Profile_t_Modal");
            //Page.AssertClick("Profile_p_ReturnProfile");
            //Page.AssertIsLoaded();
            //Page.Login(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword, "ProfilePage.Profile_h_SignIn");
            //Page.AssertElementExists("ProfilePage.Profile_h_title");
            //Page.Logout();
      //  }
        #endregion
    }
}
