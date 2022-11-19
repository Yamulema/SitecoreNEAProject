using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.FamilyAndWellness {

	public class FamilyAndWellnessPage : NeambPageBase {
		
		#region Constructor
		public FamilyAndWellnessPage(
			IWebDriver driver,
			ISettings settings
		) : base(driver, settings) {}
		#endregion
	}
}
