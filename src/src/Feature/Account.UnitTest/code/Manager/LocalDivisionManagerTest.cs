using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Seiumb.Feature.Account.Models;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using source =Neambc.Seiumb.Feature.Account.Manager;
using NUnit.Framework;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Seiumb.Feature.Account.UnitTest.Manager
{
    [TestFixture]
    public class LocalDivisionManagerTest
    {
        private Mock<Neamb.Foundation.Cache.Managers.ICacheManager> _cacheManagerMock;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private Mock<ISharedProfile> _sharedProfileMock;
        private source.ILocalDivisionManager _sut;

        #region Instrumentation

        [SetUp]
        public void SetUp()
        {
            _cacheManagerMock = new Mock<Neamb.Foundation.Cache.Managers.ICacheManager>();
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
            _sharedProfileMock = new Mock<ISharedProfile>();
            _sut = new source.LocalDivisionManager(_cacheManagerMock.Object, _globalConfigurationManagerMock.Object, _sharedProfileMock.Object);
        }

        [Test]
        public void Verify_ExistLocalCodeUser_Returns_False() {
            _sharedProfileMock.Setup(item => item.GetCustomProperty(UserDataCons.SEIU_LOCAL_NUMBER)).Returns("110");
            var result = _sut.ExistLocalCodeUser(new List<LocalCodeDto>(), "");
            Assert.AreEqual(result, false);
        }

        [Test]
        public void Verify_ExistLocalCodeUser_Returns_True() {
            string localNumber = "110";
            string division = "Public";
            var listLocalCodes = new List<LocalCodeDto>();
            listLocalCodes.Add(new LocalCodeDto{Code = localNumber,Division = division});
            _sharedProfileMock.Setup(item => item.GetCustomProperty(UserDataCons.SEIU_LOCAL_NUMBER)).Returns("110");
            var result = _sut.ExistLocalCodeUser(listLocalCodes, division);
            Assert.AreEqual(result, true);
        }

        [Test]
        public void Verify_GetLocalCodesGlobal_Returns_False() {
            string localNumber = "110";
            string division = "Public";
            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            })
            {
                
            }
            using (var db = new Sitecore.FakeDb.Db()) {
                var root = new DbItem("NEAMBC");
                var global = new DbItem("Global");
                var locals = new DbItem("Locals");
                var localFolder = new DbItem("SEIUMB Local Code Folder");
                var localCodeFirst = new DbItem("LocalItem", new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")) {
                    Name = "LocalItem",
                    Fields = {
                        new DbField("Id", new ID("{4E5AF70D-E2A1-48A8-B230-DA5839BD4FC3}")) {
                            Value = localNumber
                        },
                        new DbField("Name", new ID("{A5A6D562-BA94-453A-BCA5-4F888D9D6DDD}")) {
                            Value = "LocalItem"
                        },
                        new DbField("LocalDivision", new ID("{89A284C6-DD26-4562-9910-CE199C236C54}")) {
                            Value = division
                        },
                    }
                };
                root.Add(global);
                global.Add(locals);
                locals.Add(localFolder);
                localFolder.Add(localCodeFirst);

                _sut.GetLocalCodesGlobal();


            }

            
        }

        #endregion

        }
}
