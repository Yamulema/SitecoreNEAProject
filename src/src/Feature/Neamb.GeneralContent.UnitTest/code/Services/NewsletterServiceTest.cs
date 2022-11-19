using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Neambc.Neamb.Feature.GeneralContent.Services;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.FakeDb.Sites;

namespace Neambc.Neamb.Feature.GeneralContent.UnitTest.Services
{
    [TestFixture]
    public class NewsletterServiceTest
    {
        #region Fields
        private NewsletterService _sut;
        private Db _db;
        private Mock<ISessionAuthenticationManager> _sessionManager;
        private Mock<ISubscriptionsManager> _subscriptionsManager;
        private Mock<ISearchUserNameService> _searchUserNameService;
        private FakeSiteContext _context;
        #endregion

        [SetUp]
        public void SetUp()
        {
            _sessionManager = new Mock<ISessionAuthenticationManager>();
            _subscriptionsManager = new Mock<ISubscriptionsManager>();
            _searchUserNameService = new Mock<ISearchUserNameService>();
            _db = new Db();
            _sut = new NewsletterService(_sessionManager.Object, _subscriptionsManager.Object, _searchUserNameService.Object);
            _context = new FakeSiteContext(new Sitecore.Collections.StringDictionary()
            {
                {"enableWebEdit", "true"},
                {"masterDatabase", "master"}
            });
        }
        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }
        #region GetNewsletter
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetNewsletter_Should_Return_AnonymousModel_When_User_Is_Anonymous()
        {
            ID idItem1 = new ID("{5a5c6410-6b51-4a3a-a1e4-bd1f35db8db9}");

            ID idField = new ID("{5a5c6410-6b51-4a3a-a1e4-bd1f35db8db8}");
            ID idFieldNewsletter = new ID("{55DE5632-19E9-4C6F-B983-4770542AB84F}");
            //Arrange
            var contextItem = new DbItem("WizardPage", new ID()) {
                Fields = {
                    new DbField("Link", idField) {
                        Value = "{5a5c6410-6b51-4a3a-a1e4-bd1f35db8db9}"
                    },
                }
            };

            var item = new DbItem("Newsletter", idItem1)
            {
                Fields = {
                    new DbField("Link", idFieldNewsletter) {
                        Value = ""
                    },
                }
            };
            _db.Add(contextItem);
            _db.Add(item);
            var isSubscribed = false;
            using (new Sitecore.Sites.SiteContextSwitcher(_context)) {
                _sessionManager.Setup(x => x.GetAccountMembership())
                    .Returns(new AccountMembership {
                        Status = StatusEnum.Cold
                    });
                //Act
                var result = _sut.GetNewsletter(_db.GetItem(contextItem.ID), isSubscribed);

                //Assert
                Assert.IsTrue(result.IsAnonymous);
            }
        }
        #endregion
    }
}
