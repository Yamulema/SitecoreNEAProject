using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.UnitTest.Model {
	[TestFixture()]
	public class Beneficiary {

		[Test]
		public void Beneficiary_CreatesSample() {
			var result = SUT.Beneficiary.CreateSample();
			Assert.IsNotNull(result.Id);
			Assert.IsNotNull(result.DisplayName);
			Assert.IsNotNull(result.DisplayRelationship);
			Assert.IsNotNull(result.DisplayShare);
		}
	}
}
