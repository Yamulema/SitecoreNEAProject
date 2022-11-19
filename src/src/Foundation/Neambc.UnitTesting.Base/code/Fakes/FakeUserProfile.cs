using System;
using Sitecore.Security.Accounts;

namespace Neambc.UnitTesting.Base.Fakes {
	internal class FakeUserProfile : Sitecore.Security.UserProfile {

		public override string Email { get; set; }

		public DateTime? SavedAtUtc;
		public override void Save() {
			SavedAtUtc = DateTime.UtcNow;
		}
	}
}
