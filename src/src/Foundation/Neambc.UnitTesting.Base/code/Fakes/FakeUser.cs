namespace Neambc.UnitTesting.Base.Fakes {
	internal class FakeUser : Sitecore.Security.Accounts.User {

		public new Sitecore.Security.UserProfile Profile { get; set; }

		public FakeUser(string name, bool authenticated) : base(name, authenticated) { }

	}
}
