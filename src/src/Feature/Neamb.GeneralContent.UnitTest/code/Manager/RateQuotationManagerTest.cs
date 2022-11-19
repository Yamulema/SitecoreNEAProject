using System;
using System.Collections.Generic;
using Moq;
using Neambc.Neamb.Foundation.MBCData.Managers;
using NUnit.Framework;
using Neambc.Neamb.Feature.GeneralContent.Enums;
using Neambc.Neamb.Feature.GeneralContent.Managers;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Cache.Managers;

namespace Neambc.Neamb.Feature.GeneralContent.UnitTest.Manager
{
    [TestFixture]
    public class RateQuotationManagerTest
    {
        
        #region Fields
        private  Mock<IOracleDatabase> _oracleManagerMock;
        private  Mock<ICacheManager> _cacheManagerMock;

        #endregion

        #region Pre/Post Actions
        [SetUp]
        public void SetUp() {
            // before each test
            _oracleManagerMock = new Mock<IOracleDatabase>();
            _cacheManagerMock = new Mock<ICacheManager>();
        }

        [TearDown]
        public void TearDown() {
            // after each test
        }
        #endregion

        #region GetPlanQuotes

        [Test]
        public void GetPlanQuotes_Should_Return_List_Plan()
        {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);
            List<Plan> PlanData = new List<Plan> {new Plan() {
                Name = "A",
                Info = "../-/media/Files/NEAMB/Landing Pages/NEAMB_RetireeHealth_Detail_A.pdf",
                Quotes =  new List<Quote> { new Quote() {
                        Age = "65",
                        Price = "113.00"
                    }
                }
            },
                new Plan() {
                    Name = "B",
                    Info = "../-/media/Files/NEAMB/Landing Pages/NEAMB_RetireeHealth_Detail_B.pdf",
                    Quotes =  new List<Quote> { new Quote() {
                            Age = "65",
                            Price = "113.00"
                        }
                    }
                },
                new Plan() {
                    Name = "C",
                    Info = "../-/media/Files/NEAMB/Landing Pages/NEAMB_RetireeHealth_Detail_C.pdf",
                    Quotes =  new List<Quote> { new Quote() {
                            Age = "65",
                            Price = "113.00"
                        }
                    }
                }
            };
            IEnumerable<String> RatesData = new List<string>() { "113.00" };
            int[] ages = new int[] { 65 };
            _cacheManagerMock.Setup(x => x.ExistInCache("RateQuotation:FL_32007_65")).Returns(false);
            foreach (var option in Configuration.PlanOptions)
            {
                _oracleManagerMock.Setup(x => x.SelectRates("FL", "32007", ages, option)).Returns(RatesData);
            }


            //Act
            var result = rateQuotationManager.GetPlanQuotes("FL", "32007", "65");

            //Assert
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].Info, PlanData[i].Info);
                Assert.AreEqual(result[i].Name, PlanData[i].Name);
                for (int j = 0; j < result[i].Quotes.Count; j++)
                {
                    Assert.AreEqual(result[i].Quotes[j].Age, PlanData[i].Quotes[j].Age);
                    Assert.AreEqual(result[i].Quotes[j].Price, PlanData[i].Quotes[j].Price);
                }

            }
        }

        [Test]
        public void GetPlanQuotes_Should_Return_Empty_List_Plan_When_No_Rates()
        {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);
            List<Plan> PlanData = new List<Plan>();
            IEnumerable<String> RatesData = new List<string>();
            int[] ages = new int[] { 65 };
            _cacheManagerMock.Setup(x => x.ExistInCache("RateQuotation:FL_32007_65")).Returns(false);
            foreach (var option in Configuration.PlanOptions)
            {
                _oracleManagerMock.Setup(x => x.SelectRates("FL", "32007", ages, option)).Returns(RatesData);
            }


            //Act
            var result = rateQuotationManager.GetPlanQuotes("FL", "32007", "65");

            //Assert
            Assert.AreEqual(result.ToString(), PlanData.ToString());
        }

        [Test]
        public void GetPlanQuotes_Should_Return_List_When_Chached()  {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);
            IEnumerable<String> RatesData = new List<string>();
            int[] ages = new int[] { 65, 67, 70 };
            _cacheManagerMock.Setup(x => x.ExistInCache("RateQuotation:FL_32007_65")).Returns(true);
            _cacheManagerMock.Setup(x => x.RetrieveFromCache<List<Plan>>("RateQuotation:FL_32007_65")).Returns(new List<Plan>());
            _oracleManagerMock.Setup(x => x.SelectRates("FL", "32007", ages, "A")).Returns(RatesData);

            //Act
            var result = rateQuotationManager.GetPlanQuotes("FL", "32007", "65");

            //Assert
            Assert.AreEqual(result, RatesData);
        }

        #endregion

        #region Validate StateCode ZipCode Age

        [Test]
        public void Validate_Should_Return_QuoteStatusNodata_When_Zip_Is_Null_And_State_Is_Null()
        {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);

            //Act
            var result = rateQuotationManager.Validate("", "");

            //Assert
            Assert.AreEqual(result, QuoteStatus.NoData);
        }
        [Test]
        public void Validate_Should_Return_QuoteStatusNodata_When_Zip_Lenght_Is_Invalid()
        {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);

            //Act
            var result = rateQuotationManager.Validate("FL", "3200");

            //Assert
            Assert.AreEqual(result, QuoteStatus.NoData);
        }
        [Test]
        public void Validate_Should_Return_QuoteStatuOk_When_Age_Is_Valid()
        {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);

            //Act
            var result = rateQuotationManager.Validate("AK", "32007");

            //Assert
            Assert.AreEqual(result, QuoteStatus.Ok);
        }
        [Test]
        public void Validate_Should_Return_QuoteStatusOK_When_Age_Is_Valid()
        {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);

            //Act
            var result = rateQuotationManager.Validate("FL", "32007");

            //Assert
            Assert.AreEqual(result, QuoteStatus.Ok);
        }

        #endregion

        #region Validate StateCode
        [Test]
        public void Validate_Should_Return_StateStatusInvalid_When_State_Is_Null() {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);

            //Act
            var result = rateQuotationManager.Validate("");

            //Assert
            Assert.AreEqual(result, StateStatus.Invalid);
        }

        [Test]
        public void Validate_Should_Return_StateStatusInvalid_When_State_Is_One_Character() {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);

            //Act
            var result = rateQuotationManager.Validate("A");

            //Assert
            Assert.AreEqual(result, StateStatus.Invalid);
        }

        [Test]
        public void Validate_Should_Return_StateStatusSpecifyAge_When_State_Is_Florida() {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);

            //Act
            var result = rateQuotationManager.Validate("FL");

            //Assert
            Assert.AreEqual(result, StateStatus.None);
        }

        [Test]
        public void Validate_Should_Return_StateStatusSpecifyZip_When_State_Is_Missouri() {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);
            _oracleManagerMock.Setup(x => x.SelectZipCodeCount("MO")).Returns(123);
            

            //Act
            var result = rateQuotationManager.Validate("MO");

            //Assert
            Assert.AreEqual(result, StateStatus.SpecifyZip);
        }

        [Test]
        public void Validate_Should_Return_StateStatusSpecifyAge_When_State_Is_Missouri() {
            //Arrange
            var rateQuotationManager = new RateQuotationManager(_oracleManagerMock.Object, _cacheManagerMock.Object);
            _oracleManagerMock.Setup(x => x.SelectZipCodeCount("MO")).Returns(1);
            

            //Act
            var result = rateQuotationManager.Validate("MO");

            //Assert
            Assert.AreEqual(result, StateStatus.SpecifyAge);
        }

        #endregion
    }
}
