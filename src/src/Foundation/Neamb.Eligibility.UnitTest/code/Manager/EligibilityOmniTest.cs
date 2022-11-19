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
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Eligibility.UnitTest.Manager
{
    [TestFixture]
    public class EligibilityOmniTest
    {
        #region Fields
        protected IEligibilityOmni _sut;
        private Mock<IOracleDatabase> _oracleManagerMock;
        private Mock<ICacheManager> _cacheManagerMock;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;

        #endregion
        #region Pre/Post Actions

        [OneTimeSetUp]
        public void SetUpOnce()
        { 
            // before each test
            _oracleManagerMock = new Mock<IOracleDatabase>();
            _cacheManagerMock = new Mock<ICacheManager>();
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
            _sut = new EligibilityOmni(_oracleManagerMock.Object,_cacheManagerMock.Object,_globalConfigurationManagerMock.Object);

        }
        [Test]
        public void CheckEligibility_ReturnData_WithCorrectValuesRetrievedCache()
        {
            //Arrange
            string mdsid = "000000995";
            string productCode = "123";
            IList<ViewOmni> ret = new List<ViewOmni>();
            ret.Add(new ViewOmni { WebAppUrl = "http://www.google.com", WebSoctUrl = "http://www.google.com" });
            //Act
            _oracleManagerMock.Setup(x => x.ExecuteViewOmni(mdsid, productCode)).Returns(ret);
            _cacheManagerMock.Setup(x => x.RetrieveFromCache<IList<ViewOmni>>(It.IsAny<string>())).Returns(ret);
            var result = _sut.CheckEligibility(mdsid, productCode);

            //Assert
            Assert.AreEqual(ret, result);
        }

        [Test]
        public void CheckEligibility_ReturnNoData_WithIncorrectValues()
        {
            //Arrange
            string mdsid = null;
            string productCode = null;
            IList<ViewOmni> ret = null;
            //Act
            _oracleManagerMock.Setup(x => x.ExecuteViewOmni(mdsid, productCode)).Returns(ret);
            _cacheManagerMock.Setup(x => x.RetrieveFromCache<IList<ViewOmni>>(It.IsAny<string>())).Returns(ret);
            var result = _sut.CheckEligibility(mdsid, productCode);

            //Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void CheckEligibility_ReturnNoData_WithCorrectValues()
        {
            //Arrange
            string mdsid = "000000995";
            string productCode = "123";
            IList<ViewOmni> ret = null;
            //Act
            _oracleManagerMock.Setup(x => x.ExecuteViewOmni(mdsid, productCode)).Returns(ret);
            _cacheManagerMock.Setup(x => x.RetrieveFromCache<IList<ViewOmni>>(It.IsAny<string>())).Returns(ret);
            var result = _sut.CheckEligibility(mdsid, productCode);

            //Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void CheckEligibility_ReturnData_WithCorrectValues()
        {
            //Arrange
            string mdsid = "000000995";
            string productCode = "123";
            IList<ViewOmni> ret = new List<ViewOmni>();
            ret.Add(new ViewOmni{WebAppUrl = "http://www.google.com", WebSoctUrl = "http://www.google.com" });
            //Act
            _oracleManagerMock.Setup(x => x.ExecuteViewOmni(mdsid, productCode)).Returns(ret);
            var result = _sut.CheckEligibility(mdsid, productCode);

            //Assert
            Assert.AreEqual(ret, result);
        }

       

        #endregion
    }
}
