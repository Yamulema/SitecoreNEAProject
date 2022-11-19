using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Product.Manager;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Product.UnitTest.Manager {
	[TestFixture]
	public class QueryStringManagerTest : QueryStringManager {
		#region Fields
		private Mock<ISessionManager> _sessionManagerMock;

		#endregion

		#region Pre/Post Actions

		[SetUp]
		public void SetUp() {
			// before each test
			_sessionManagerMock = new Mock<ISessionManager>();

		}

		[TearDown]
		public void TearDown() {
			// after each test
		}

		#endregion

		#region Tests

		[Test]
		public void AppendQueryStringParameter_Should_Return_URL() {
			//Arrange
			var urlInput = "https://www.neamb.com";
			_sessionManagerMock.Setup(x => x.RetrieveFromSession<string>(ConstantsNeamb.QueryParameter)).Returns("camp_code=20");
			var expectedResult = "https://www.neamb.com?camp_code=20";

			//Act
			var result = AppendQueryStringParameter(urlInput, _sessionManagerMock.Object);

			//Assert
			Assert.AreEqual(expectedResult, result);
		}

		#endregion
	}
}
