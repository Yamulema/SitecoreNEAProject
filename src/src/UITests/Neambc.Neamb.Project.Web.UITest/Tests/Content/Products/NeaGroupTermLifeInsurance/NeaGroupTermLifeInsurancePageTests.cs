using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaGroupTermLifeInsurance;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaGroupTermLifeInsurance
{
    [TestFixture]
    public class NeaGroupTermLifeInsurancePageTests : NeambTestBaseLarge<NeaGroupTermLifeInsurancePage>
    {
        /// <summary>
        /// Mercer
        /// </summary>
        [TestCaseSource(typeof(NeaGroupTermLifeInsurancePageTestData),
            nameof(NeaGroupTermLifeInsurancePageTestData.TestDataSource),
            new object[] { "Mercer" })]
        [Test, Category("Content")]
        public void Mercer(string username, string password, string coverage, string certificate)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaGroupTermLifeInsurancePage>(username, password)
                .AssertIsLoaded()
                .ClickOnSecondaryCta()
                .SwitchToNewestTab<MercerPage>()
                .AssertLinkHotState(coverage,certificate)
                .CloseCurrentTab<NeaGroupTermLifeInsurancePage>();
        }

		/// <summary>
		/// MemberEnroll
		/// </summary>
		[TestCaseSource(typeof(NeaGroupTermLifeInsurancePageTestData),
			nameof(NeaGroupTermLifeInsurancePageTestData.TestDataSource),
			new object[] { "MemberEnroll" })]
		[Test, Category("Content")]
		public void MemberEnroll(string username, string password, string firstName, string lastName, string dob) {
			Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<NeaGroupTermLifeInsurancePage>(username, password)
				.AssertIsLoaded()
				.ClickOnPrimaryCta()
				.SwitchToNewestTab<MemberEnrollPage>()
				.AssertFormIsLoaded(firstName, lastName, dob)
				.CloseCurrentTab<NeaGroupTermLifeInsurancePage>();
		}
	}
}
