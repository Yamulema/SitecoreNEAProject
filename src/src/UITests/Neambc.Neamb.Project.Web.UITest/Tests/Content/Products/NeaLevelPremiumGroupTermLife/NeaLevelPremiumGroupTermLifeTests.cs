using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaLevelPremiumGroupTermLife;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaLevelPremiumGroupTermLife
	{
    [TestFixture]
    public class NeaAccidentalDeathAndDismembermentTests : NeambTestBaseLarge<NeaLevelPremiumGroupTermLifePage>
    {
		/// <summary>
		/// MemberEnroll
		/// </summary>
		[TestCaseSource(typeof(NeaLevelPremiumGroupTermLifePageTestData),
			nameof(NeaLevelPremiumGroupTermLifePageTestData.TestDataSource),
			new object[] { "MemberEnroll" })]
		[Test, Category("Content")]
		public void MemberEnroll(string username, string password, string firstName, string lastName, string dob) {
			Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<NeaLevelPremiumGroupTermLifePage>(username, password)
				.AssertIsLoaded()
				.ClickOnPrimaryCta()
				.SwitchToNewestTab<MemberEnrollPage>()
				.AssertFormIsLoaded(firstName, lastName, dob)
				.CloseCurrentTab<NeaLevelPremiumGroupTermLifePage>();
		}


		/// <summary>
		/// Mercer
		/// </summary>
		[TestCaseSource(typeof(NeaLevelPremiumGroupTermLifePageTestData),
			nameof(NeaLevelPremiumGroupTermLifePageTestData.TestDataSource),
			new object[] { "Mercer" })]
		[Test, Category("Content")]
		public void Mercer(string username, string password, string coverage, string certificate) {
			var x = Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<NeaLevelPremiumGroupTermLifePage>(username, password)
				.AssertIsLoaded()
				.ClickOnSecondaryCta()
				.SwitchToNewestTab<MercerPage>()
				.AssertLinkHotState(coverage, certificate)
				.CloseCurrentTab<NeaLevelPremiumGroupTermLifePage>();
		}


	}
}
