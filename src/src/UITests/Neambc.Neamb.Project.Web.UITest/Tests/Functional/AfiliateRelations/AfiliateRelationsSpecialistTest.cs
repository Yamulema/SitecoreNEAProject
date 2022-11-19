using Neambc.Neamb.Project.Web.UITest.Pages.AfiliateRelations;
using NUnit.Framework;


namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.AfiliateRelations
{
    [TestFixture]
    public class AfiliateRelationsSpecialistTest: NeambTestBaseLarge<AfiliateRelationsSpecialist>
    {
        [TestCaseSource(typeof(AfiliateRelationsSpecialistData),
            nameof(AfiliateRelationsSpecialistData.TestDataSource),
            new object[] { "SelectState" })]
        [Test]
        public void Select_State(string  stateName)
        {
            Page.AssertIsLoaded()
                .AssertSelectState(stateName)
                .AssertClickOnSubmit()
                .AssertValidateFinalContent();
        }
    }
}
