using Moq;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using MN = Neambc.Neamb.Foundation.Product.Manager;


namespace Neambc.Neamb.Foundation.Product.UnitTest.Manager {
	[TestFixture]
	public class UserStatusHandlerTest {
		[Test]
		public void Return_Url_Empty_UserNotAuthenticated() {
			//Arrange
			var expectedValue = "";
			var sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>().Object;
			var userStatusHandlerManager =
				new MN.UserStatusHandler(sessionAuthenticationManager);

			//Act
			var result = userStatusHandlerManager.GetUrlUserNotAuthenticated();

			//Assert
			Assert.AreEqual(result, expectedValue);
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Return_Url_NotEmpty_UserAuthenticated() {
			////Arrange
			var expectedValue = string.Empty;

			var sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
			sessionAuthenticationManager
				.Setup(x => x.GetAccountMembership())
				.Returns(new AccountMembership() {
					Status = StatusEnum.WarmHot
				});

			using (var db = new Db
			{
				new DbItem("Login",new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}"))
				{
					{"Title", "Testing the Id"}
				}
			}) {
				var userStatusHandlerManager =
					new MN.UserStatusHandler(sessionAuthenticationManager.Object);
				//Act
				var result = userStatusHandlerManager.GetUrlUserNotAuthenticated();
				//Assert
				Assert.AreNotEqual(result, expectedValue);
			}
		}
	}
}
