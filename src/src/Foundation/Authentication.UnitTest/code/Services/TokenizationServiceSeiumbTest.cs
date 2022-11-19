using Moq;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Models;
using Neambc.Seiumb.Foundation.Authentication.Services;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Membership.UnitTest.Services
{

    [TestFixture]
	public class TokenizationServiceSeiumbTest {

		#region Fields
		protected ITokenizationServiceSeiumb _tokenizationServiceSeiumb;
        protected Mock<ISeiumbProfileManager> _sharedProfileMock;
        #endregion

        [OneTimeSetUp]
        public void SetUpOnce() {
            _sharedProfileMock = new Mock<ISeiumbProfileManager>();
            _tokenizationServiceSeiumb = new TokenizationServiceSeiumb(_sharedProfileMock.Object);
        }

        [Test]
        public void Unmatched_Elements_Returns_Input()
        {
            const string rawText = "<span><h1>Test</h1></span>";
            var result = _tokenizationServiceSeiumb.DeTokenize(rawText);
            Assert.AreEqual(result, rawText);
        }

        [Test]
        public void FirstNameTokens_Replaced()
        {
            //Arrange
            var token = "[FirstName]";
            var firstName = "Mariela";
            var rawText = $"<h1>Hello {token}</h1>";
            var expectedResult = $"<h1>Hello {firstName}</h1>";

            _sharedProfileMock.Setup(x => x.GetProfile()).Returns(new SeiuProfile{FirstName = firstName});

            //Act
            var result = _tokenizationServiceSeiumb.DeTokenize(rawText);

            //Assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void MdsidTokens_Replaced()
        {
            //Arrange
            var token = "[mdsid_clear]";
            var mdsid = "940";
            var rawText = $"<h1>Hello {token}</h1>";
            var expectedResult = $"<h1>Hello {mdsid}</h1>";

            _sharedProfileMock.Setup(x => x.GetProfile()).Returns(new SeiuProfile{MdsId = mdsid});

            //Act
            var result = _tokenizationServiceSeiumb.DeTokenize(rawText);

            //Assert
            Assert.AreEqual(result, expectedResult);
        }
    }
}
