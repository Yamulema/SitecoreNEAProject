using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Manager;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Eligibility.UnitTest.Manager
{
    [TestFixture]
    public class EligibilityManagerMarketplaceTest
    {
        #region Fields
        protected IEligibilityManagerMarketplace _sut;
        private Mock<IEligibilityManager> _eligibilityManagerMock;
        private Mock<ICacheManager> _cacheManagerMock;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private FakeLog _log;
        #endregion
        #region Pre/Post Actions

        [OneTimeSetUp]
        public void SetUpOnce()
        {
            // before each test
            _eligibilityManagerMock = new Mock<IEligibilityManager>();
            _cacheManagerMock = new Mock<ICacheManager>();
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
            _log= new FakeLog();
            _sut = new EligibilityManagerMarketplace(_eligibilityManagerMock.Object,_cacheManagerMock.Object,_globalConfigurationManagerMock.Object,_log);

        }
        [Test]
        public void CheckEligibility_ReturnEligibilityFromService()
        {
            //Arrange
            string mdsid = "000000995";
            string productCode = "123";
            //Act
            EligibilityResultEnum? ret = null;
            _cacheManagerMock.Setup(x => x.RetrieveFromCache<EligibilityResultEnum?>(It.IsAny<string>())).Returns(ret);
            _eligibilityManagerMock.Setup(x => x.IsMemberEligible(mdsid, productCode, 12)).Returns(EligibilityResultEnum.NotEligible);
            var result = _sut.IsMemberEligible(mdsid, productCode);
            //Assert
            Assert.AreEqual(EligibilityResultEnum.NotEligible, result);
        }

        [Test]
        public void CheckEligibility_ReturnEligibilityFromCache()
        {
            //Arrange
            string mdsid = "000000995";
            string productCode = "123";
            //Act
            EligibilityResultEnum? ret = EligibilityResultEnum.Eligible;
            _cacheManagerMock.Setup(x => x.RetrieveFromCache<EligibilityResultEnum?>(It.IsAny<string>())).Returns(ret);
            _eligibilityManagerMock.Setup(x => x.IsMemberEligible(mdsid, productCode, 12)).Returns(EligibilityResultEnum.NotEligible);
            var result = _sut.IsMemberEligible(mdsid, productCode);
            //Assert
            Assert.AreEqual(EligibilityResultEnum.Eligible, result);
        }

        #endregion
    }
}
