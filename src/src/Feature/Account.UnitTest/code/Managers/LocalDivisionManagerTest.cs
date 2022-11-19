using System.Collections.Generic;
using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Seiumb.Feature.Account.Manager;
using Neambc.Seiumb.Feature.Account.Models;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Models;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Seiumb.Feature.Account.UnitTest
{
    [TestFixture]
    public class LocalDivisionManagerTest
    {
        #region Fields

        private Mock<ICacheManager> _cacheManagerMock;
        private Mock<ISeiumbProfileManager> _sharedProfileMock;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private ILocalDivisionManager _sut;

        #endregion

        #region Instrumentation

        [SetUp]
        public void SetUp() {
            _cacheManagerMock = new Mock<ICacheManager>();
            _sharedProfileMock = new Mock<ISeiumbProfileManager>();
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
            _sut = new LocalDivisionManager(_cacheManagerMock.Object, _globalConfigurationManagerMock.Object, _sharedProfileMock.Object);
        }

        #endregion
        /// <summary>
        /// Test when there are empty parameters method ExistLocalCodeUser
        /// </summary>
        [Test]
        public void ExistLocalCodeUser_ReturnsFalse() {
            var result =_sut.ExistLocalCodeUser(new List<LocalCodeDto>(), "");
            Assert.AreEqual(result, false);
        }

        /// <summary>
        /// Test ExistLocalCodeUser method to return result according the local code and division specified
        /// </summary>
        [Test]
        public void ExistLocalCodeUser_ReturnsTrue() {
            var localCodes = new List<LocalCodeDto>();
            var localCode = "178";
            var division = "Public";
            localCodes.Add(new LocalCodeDto{Code = localCode , Division = division });
            _sharedProfileMock.Setup(item => item.GetProfile()).Returns(new SeiuProfile {SeiuLocalNumber = localCode });
            var result = _sut.ExistLocalCodeUser(localCodes, division);
            Assert.AreEqual(result, true);
        }

        /// <summary>
        /// Test GetLocalCodesGlobal method to return an empty list
        /// </summary>
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetLocalCodesGlobalReturnEmptyListWithNoItems()
        {
            using (var db = new Db()) {
                var root = new DbItem("NEAMBC", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var global = new DbItem("Global", new ID("{2EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var locals = new DbItem("Locals", new ID("{3EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var localFolder = new DbItem("SEIUMB Local Code Folder", new ID("{4EA33232-AC25-42E5-A550-6C9232F318ED}"));
                root.Add(global);
                global.Add(locals);
                locals.Add(localFolder);
                var result =_sut.GetLocalCodesGlobal();
                Assert.AreEqual(result.Count==0, true);
            }
        }
        /// <summary>
        /// Test GetLocalCodesGlobal to return a non empty list
        /// </summary>
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetLocalCodesGlobalReturnListWithItems()
        {
            var localCode = "178";
            var division = "Public";
            using (var db = new Db())
            {
                var root = new DbItem("NEAMBC", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var global = new DbItem("Global", new ID("{2EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var locals = new DbItem("Locals", new ID("{3EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var localFolder = new DbItem("SEIUMB Local Code Folder", new ID("{4EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var localCode1 = new DbItem("localCode1", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) {
                    Fields = {
                        new DbField("Id", new ID("{4E5AF70D-E2A1-48A8-B230-DA5839BD4FC3}")) { Value = localCode },
                        new DbField("Name", new ID("{A5A6D562-BA94-453A-BCA5-4F888D9D6DDD}")) { Value = "local code 1" },
                        new DbField("Local Division", new ID("{89A284C6-DD26-4562-9910-CE199C236C54}")) { Value = division }
                    }
                };

                root.Add(global);
                global.Add(locals);
                locals.Add(localFolder);
                localFolder.Add(localCode1);
                db.Add(root);
                var result = _sut.GetLocalCodesGlobal();
                Assert.AreEqual(result.Count > 0, true);
            }
        }

        /// <summary>
        /// Test the GetLocalCodesGlobal to return from cache a specific list
        /// </summary>
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetLocalCodesGlobalReturnListWithItemsFromCache()
        {
            var localCodes = new List<LocalCodeDto>();
            var localCode = "178";
            var division = "Public";
            localCodes.Add(new LocalCodeDto { Code = localCode, Division = division });

            using (var db = new Db())
            {
                var root = new DbItem("NEAMBC", new ID("{1EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var global = new DbItem("Global", new ID("{2EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var locals = new DbItem("Locals", new ID("{3EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var localFolder = new DbItem("SEIUMB Local Code Folder", new ID("{4EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var localCode1 = new DbItem("localCode1", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"))
                {
                    Fields = {
                        new DbField("Id", new ID("{4E5AF70D-E2A1-48A8-B230-DA5839BD4FC3}")) { Value = localCode },
                        new DbField("Name", new ID("{A5A6D562-BA94-453A-BCA5-4F888D9D6DDD}")) { Value = "local code 1" },
                        new DbField("Local Division", new ID("{89A284C6-DD26-4562-9910-CE199C236C54}")) { Value = division }
                    }
                };

                root.Add(global);
                global.Add(locals);
                locals.Add(localFolder);
                localFolder.Add(localCode1);
                db.Add(root);
                _cacheManagerMock.Setup(item => item.RetrieveFromCache<IList<LocalCodeDto>>("LocalDivision")).Returns(localCodes);
                var result = _sut.GetLocalCodesGlobal();
                Assert.AreEqual(result.Count, localCodes.Count);
            }
        }
    }
}
