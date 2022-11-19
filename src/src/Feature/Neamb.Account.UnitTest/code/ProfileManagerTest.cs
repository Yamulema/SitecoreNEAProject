using Moq;
using Neambc.Neamb.Feature.Account.Managers;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Services.ActionReminder;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Services.UpdateUserName;
using Neambc.Neamb.Foundation.MBCData.Services.UpdateUser;
using Neambc.Neamb.Foundation.Membership.Managers;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Feature.Account.UnitTest
{
    [TestFixture]
    public class ProfileManagerTest
    {
        #region Fields
        private Mock<ISessionAuthenticationManager> _sessionAuthenticationManager;
        private Mock<IAccountRepository> _accountRepository;
        private Mock<IAccountServiceProxy> _serviceManager;
        private Mock<IAuthenticationAccountManager> _authenticationAccountManager;
        private Mock<ICacheManager> _cacheManager;
        private Mock<IGtmService> _gtmService;
        private Mock<IActionReminderService> _actionReminderService;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManager;
        private Mock<IUpdateUserNameService> _updateUserNameService;
        private Mock<ISearchUserNameService> _searchUserNameService;
        private Mock<IUpdateUserService> _updateUserService;
        #endregion
        [SetUp]
        public void SetUp()
        {
            _sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
            _accountRepository = new Mock<IAccountRepository>();
            _serviceManager = new Mock<IAccountServiceProxy>();
            _authenticationAccountManager = new Mock<IAuthenticationAccountManager>();
            _cacheManager = new Mock<ICacheManager>();
            _gtmService = new Mock<IGtmService>();
            _actionReminderService = new Mock<IActionReminderService>();
            _globalConfigurationManager = new Mock<IGlobalConfigurationManager>();
            _updateUserNameService = new Mock<IUpdateUserNameService>();
            _searchUserNameService = new Mock<ISearchUserNameService>();
            _updateUserService = new Mock<IUpdateUserService>();
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void Return_ActionText_ProfileUpdated()
        {
            string expectedValue = "dataLayerPush({ 'event':'account','accountSection':'profile & password','accountAction':'update profile','ctaText':'Save'});";
            _gtmService.Setup(x => x.GetGtmEvent(It.IsAny<object>())).Returns(expectedValue);
            var profileManager = new ProfileManager(_sessionAuthenticationManager.Object,
                _accountRepository.Object,
                _serviceManager.Object,
                _authenticationAccountManager.Object,
                _cacheManager.Object,
                _gtmService.Object,
                _actionReminderService.Object, _globalConfigurationManager.Object, _updateUserNameService.Object, _searchUserNameService.Object, _updateUserService.Object);
            using (var db = new Db {
                new DbItem("profile", new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")) {
                    Fields = {
                        new DbField("ProfileSubmit", new ID("{40170FDB-87BD-444A-AD70-1C6648D5E84D}")) {
                            Value = "Send"
                        },
                        new DbField("PasswordSubmit", new ID("{C2429042-1CAC-48FB-AD8B-7DF616CB8A7E}")) {
                            Value = "Send"
                        },
                    }
                }
            })
            {
                Sitecore.Data.Items.Item itemTest = db.GetItem(new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}"));
                var result = profileManager.GetGtmAction("0", itemTest);
                Assert.AreEqual(result, expectedValue);
            }
        }
    }
}
