using System.Collections.Generic;
using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers
{
    [TestFixture]
    public class SeminarRepositoryTest {
        #region Fields

        private Mock<IOracleDatabase> _oracleDatabaseMock;
        private Mock<ICacheManager> _cacheManagerMock;
        private Mock<ISessionManager> _sessionManagerMock;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private ISeminarRepository _sut;

        #endregion

        #region Instrumentation

        [SetUp]
        public void SetUp() {
            _oracleDatabaseMock = new Mock<IOracleDatabase>();
            _cacheManagerMock = new Mock<ICacheManager>();
            _sessionManagerMock = new Mock<ISessionManager>();
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
            _sut = new SeminarRepository(_oracleDatabaseMock.Object, _cacheManagerMock.Object, _globalConfigurationManagerMock.Object, _sessionManagerMock.Object);
        }

        #endregion

        [Test]
        public void GetSeminaries_ReturnsNull() {
            var result = _sut.GetSeminaries();
            Assert.AreEqual(result, null);
        }

        [Test]
        public void GetSeminaries_ReturnsDataFromCache() {
            List<ViewSeminar> listViewSeminars = new List<ViewSeminar>();
            _cacheManagerMock.Setup(x => x.ExistInCache(ConstantsNeamb.RedisKeySeminaries)).Returns(true);
            _cacheManagerMock.Setup(x => x.RetrieveFromCache<IReadOnlyList<ViewSeminar>>(It.IsAny<string>())).Returns(listViewSeminars);
            var result = _sut.GetSeminaries();
            Assert.AreEqual(result, listViewSeminars);
        }

        [Test]
        public void GetSeminaries_SaveReturnsFromCache() {
            List<ViewSeminar> listViewSeminars = new List<ViewSeminar>();
            _cacheManagerMock.Setup(x => x.ExistInCache(ConstantsNeamb.RedisKeySeminaries)).Returns(false);
            _oracleDatabaseMock.Setup(x => x.ViewAllSeminar()).Returns(listViewSeminars);
            var result = _sut.GetSeminaries();
            Assert.AreEqual(result, listViewSeminars);
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetSeminarId_Empty() {
            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            }) {
                var seminar= _sut.GetSeminarId(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsEmpty(seminar);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetSeminarId_FromCache()
        {
            const string seminarExpected = "1";
            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            }) {
                _sessionManagerMock.Setup(x => x.RetrieveFromSession<string>(ConstantsNeamb.SessionSeminaryId)).Returns(seminarExpected);
                var seminar = _sut.GetSeminarId(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.AreEqual(seminar, seminarExpected);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetSeminarId_FromField()
        {
            const string seminarExpected = "2";

            ID seminarId = new ID("{942C9D25-8D1C-41EA-94D7-1FCC778C195A}");
            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) {
                    Fields = {
                        new DbField("Link", seminarId) {
                            Value = seminarExpected
                        },
                    }
                },
            })
            {
                _sessionManagerMock.Setup(x => x.RetrieveFromSession<string>(ConstantsNeamb.SessionSeminaryId)).Returns("");
                var seminar = _sut.GetSeminarId(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.AreEqual(seminar, seminarExpected);
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetLeaCode_WhenNoSeminar()
        {
            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            })
            {
                var leaCode = _sut.GetLeaCode(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsEmpty(leaCode);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetLeaCode_WhenSeminarExists()
        {
            const string seminarExpected = "2";
            const string leaCodeExpected = "leaCode";

            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            })
            {
                List<ViewSeminar> listViewSeminars = new List<ViewSeminar>();
                listViewSeminars.Add(new ViewSeminar{ SeminarId = seminarExpected, LeaCode = leaCodeExpected});
                _cacheManagerMock.Setup(x => x.ExistInCache(ConstantsNeamb.RedisKeySeminaries)).Returns(true);
                _cacheManagerMock.Setup(x => x.RetrieveFromCache<IReadOnlyList<ViewSeminar>>(It.IsAny<string>())).Returns(listViewSeminars);
                _sessionManagerMock.Setup(x => x.RetrieveFromSession<string>(ConstantsNeamb.SessionSeminaryId)).Returns(seminarExpected);
                
                var leaCode = _sut.GetLeaCode(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.AreEqual(leaCodeExpected,leaCode);
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void CheckIsValidSeminaryId_ReturnFalse()
        {
            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            })
            {
                var isValidSeminary = _sut.IsValidSeminaryId(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsFalse(isValidSeminary);
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void CheckIsValidSeminaryId_ReturnTrue()
        {
            const string seminarExpected = "2";
            
            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            })
            {
                List<ViewSeminar> listViewSeminars = new List<ViewSeminar>();
                listViewSeminars.Add(new ViewSeminar { SeminarId = seminarExpected });
                _cacheManagerMock.Setup(x => x.ExistInCache(ConstantsNeamb.RedisKeySeminaries)).Returns(true);
                _cacheManagerMock.Setup(x => x.RetrieveFromCache<IReadOnlyList<ViewSeminar>>(It.IsAny<string>())).Returns(listViewSeminars);
                _sessionManagerMock.Setup(x => x.RetrieveFromSession<string>(ConstantsNeamb.SessionSeminaryId)).Returns(seminarExpected);
                
                var isValidSeminary = _sut.IsValidSeminaryId(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsTrue(isValidSeminary);
            }
        }
    }
}
