using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaAccidentalDeathPlus;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaAccidentalDeathPlus
	{
    [TestFixture]
    public class NeaAccidentalDeathPlusPageTests : NeambTestBaseLarge<NeaAccidentalDeathPlusPage>
    {
		
		/// <summary>
		/// MemberEnroll
		/// </summary>
		[TestCaseSource(typeof(NeaAccidentalDeathPlusPageTestData),
			nameof(NeaAccidentalDeathPlusPageTestData.TestDataSource),
			new object[] { "MemberEnroll" })]
		[Test, Category("Content")]
		public void MemberEnroll(string username, string password, string firstName, string lastName, string dob) {
			
			Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<NeaAccidentalDeathPlusPage>(username, password)
				.AssertIsLoaded()
				.ClickOnPrimaryCta()
				.SwitchToNewestTab<MemberEnrollPage>()
				.AssertFormIsLoaded(firstName, lastName, dob)
				.CloseCurrentTab<NeaAccidentalDeathPlusPage>();
		}
    }
}
