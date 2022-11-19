using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaGuaranteedIssueLifeInsurancePlanPage;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaGuaranteedIssueLifeInsurancePlan
{
    [TestFixture]
    public class NeaGuaranteedIssueLifeInsurancePlanPageTests : NeambTestBaseLarge<NeaGuaranteedIssueLifeInsurancePlanPage>
    {
        [TestCaseSource(typeof(NeaGuaranteedIssueLifeInsurancePlanPageTestData),
            nameof(NeaGuaranteedIssueLifeInsurancePlanPageTestData.TestDataSource),
            new object[] { "Test_1253_1" })]
        [Test, Category("Content")]
        public void MemberEnroll(string username, string password,string firstName, string lastName, string dob)
        {
			Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<NeaGuaranteedIssueLifeInsurancePlanPage>(username, password)
				.AssertIsLoaded()
				.ClickOnPrimaryCta()
				.SwitchToNewestTab<MemberEnrollPage>()
				.AssertFormIsLoaded(firstName, lastName, dob)
				.CloseCurrentTab<NeaGuaranteedIssueLifeInsurancePlanPage>();
		}

		/// <summary>
		/// Mercer
		/// </summary>
		[TestCaseSource(typeof(NeaGuaranteedIssueLifeInsurancePlanPageTestData),
			nameof(NeaGuaranteedIssueLifeInsurancePlanPageTestData.TestDataSource),
			new object[] { "Mercer" })]
		[Test, Category("Content")]
		public void Mercer(string username, string password, string coverage, string certificate) {
			Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<NeaGuaranteedIssueLifeInsurancePlanPage>(username, password)
				.AssertIsLoaded()
				.ClickOnSecondaryCta()
				.SwitchToNewestTab<MercerPage>()
				.AssertLinkHotState(coverage, certificate)
				.CloseCurrentTab<NeaGuaranteedIssueLifeInsurancePlanPage>();
		}
	}
}
