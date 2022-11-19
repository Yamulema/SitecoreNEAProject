using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Complimentary_Life {
	public class CompLifeEditPage : NeambPageBase {
		public CompLifeEditPage(
		   IWebDriver driver,
		   ISettings settings) : base(
			   "ComplimentaryLifePage",
			   driver, settings) {

		}
	}
}
