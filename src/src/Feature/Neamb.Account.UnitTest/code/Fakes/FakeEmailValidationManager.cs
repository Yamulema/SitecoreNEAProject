using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.Account.UnitTest.Fakes {
	public class FakeEmailValidationManager : IEmailValidationManager {

		#region IEmailValidationManager
		public ResultEmailValidation IsValid(string email, bool? validateusername) {
			throw new System.NotImplementedException();
		}
		#endregion
	}

}
