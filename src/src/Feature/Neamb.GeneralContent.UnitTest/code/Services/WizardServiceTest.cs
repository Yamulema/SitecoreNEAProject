using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Moq;
using Moq.Protected;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Feature.GeneralContent.Services;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.FakeDb.Sites;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Neambc.Neamb.Feature.GeneralContent.UnitTest.Services
{
    [TestFixture]
    public class WizardServiceTest
    {
        #region Fields
        private WizardService _sut;
        private Db _db;
        private Mock<HttpRequestBase> _httpRequest;
        private DbItem _wizardPage;
        private Mock<ISessionService> _sessionService;
        private Mock<ISessionAuthenticationManager> _sessionManager;
        private Mock<IWizardEventHandler> _wizardEventHandler;
        private FakeSiteContext _context;
        #endregion

        [SetUp]
        public void SetUp()
        {
            _sessionService = new Mock<ISessionService>();
            _sessionManager = new Mock<ISessionAuthenticationManager>();
            _wizardEventHandler = new Mock<IWizardEventHandler>();
            _db = new Db();
            _sut = new WizardService(_sessionService.Object, _sessionManager.Object, _wizardEventHandler.Object);
            _httpRequest = new Mock<HttpRequestBase>();
            _wizardPage = new DbItem("WizardPage", new ID());
            _httpRequest.Setup(x => x.QueryString).Returns(new NameValueCollection());
            _sessionManager.Setup(x => x.GetAccountMembership()).Returns(new AccountMembership()
            {
                Status = StatusEnum.Hot
            });
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

        #region GetWizard
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetWizard_Should_Return_EmptyStartButtonTarget_When_NoStepsAreSelected()
        {
            //Arrange
            _wizardPage.Add(Templates.Wizard.Fields.Steps, string.Empty);
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(_wizardPage.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetWizard(currentItem, _httpRequest.Object);

                //Assert
                Assert.True(string.IsNullOrEmpty(result.StartButton.Target));
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetWizard_Should_Return_SameDatasource()
        {
            //Arrange
            _wizardPage.Add(Templates.Wizard.Fields.Steps, string.Empty);
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(_wizardPage.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetWizard(currentItem, _httpRequest.Object);

                //Assert
                Assert.AreEqual(_wizardPage.ID, result.Datasource.ID);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetWizard_Should_Return_ButtonLabels()
        {
            //Arrange
            _wizardPage.Add(Templates.Wizard.Fields.StartButtonText, Randomizer.CreateRandomizer().GetString(4));
            _wizardPage.Add(Templates.Wizard.Fields.SkipButtonText, Randomizer.CreateRandomizer().GetString(4));
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(_wizardPage.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetWizard(currentItem, _httpRequest.Object);

                //Assert
                Assert.AreEqual(_wizardPage.Fields[Templates.Wizard.Fields.StartButtonText].Value, result.StartButton.Label);
                Assert.AreEqual(_wizardPage.Fields[Templates.Wizard.Fields.SkipButtonText].Value, result.SkipButton.Label);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetWizard_Should_Return_NonEmptyStartButtonTarget_When_StepsAreSelected()
        {
            //Arrange
            var stepA = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepB = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepC = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            _wizardPage.Add(Templates.Wizard.Fields.Steps, $"{stepA.ID}|{stepC.ID}");
            _wizardPage.Children.Add(stepA);
            _wizardPage.Children.Add(stepB);
            _wizardPage.Children.Add(stepC);

            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(_wizardPage.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetWizard(currentItem, _httpRequest.Object);

                //Assert
                Assert.False(string.IsNullOrEmpty(result.StartButton.Target));
                Assert.AreEqual(result.StartButton.Target, LinkManager.GetItemUrl(_db.GetItem(stepA.ID)));
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetWizard_Should_Return_EmptySkipButtonTarget_When_NoQueryStringIsPassed()
        {
            //Arrange
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(_wizardPage.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetWizard(currentItem, _httpRequest.Object);

                //Assert
                Assert.True(string.IsNullOrEmpty(result.SkipButton.Target));
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetWizard_Should_Return_NonEmptySkipButtonTarget_When_QueryStringIsPassed()
        {
            //Arrange
            var randomUrl = $"domain.com/{Randomizer.CreateRandomizer().GetString(4)}";
            _sessionService.Setup(x => x.Get(Configuration.ReturnUrlArg)).Returns(randomUrl);
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(_wizardPage.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetWizard(currentItem, _httpRequest.Object);

                //Assert
                Assert.False(string.IsNullOrEmpty(result.SkipButton.Target));
                Assert.AreEqual(result.SkipButton.Target, randomUrl);
            }
        }
        #endregion

        #region GetStep
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetStep_Should_Return_EmptyLogoUrl_When_LogoIsNotDefined()
        {
            //Arrange
            var stepA = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            _wizardPage.Add(Templates.Wizard.Fields.Steps, $"{stepA.ID}");
            _wizardPage.Children.Add(stepA);
            _wizardPage.Add(Templates.Wizard.Fields.Logo, string.Empty);
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(stepA.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetStep(currentItem);

                //Assert
                Assert.True(string.IsNullOrEmpty(result.Header.LogoUrl));
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetStep_Should_Return_NullNextButton_When_IsLastStep()
        {
            //Arrange
            var stepA = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepB = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepC = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            _wizardPage.Add(Templates.Wizard.Fields.Steps, $"{stepA.ID}|{stepB.ID}|{stepC.ID}");
            _wizardPage.Children.Add(stepA);
            _wizardPage.Children.Add(stepB);
            _wizardPage.Children.Add(stepC);
            _wizardPage.Add(Templates.Wizard.Fields.Next, Randomizer.CreateRandomizer().GetString(4));
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(stepC.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetStep(currentItem);

                //Assert
                Assert.AreEqual(result.Header.Next, null);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetStep_Should_Return_NullBackButton_When_IsFirstStep()
        {
            //Arrange
            var stepA = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepB = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepC = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            _wizardPage.Add(Templates.Wizard.Fields.Steps, $"{stepA.ID}|{stepB.ID}|{stepC.ID}");
            _wizardPage.Children.Add(stepA);
            _wizardPage.Children.Add(stepB);
            _wizardPage.Children.Add(stepC);
            _wizardPage.Add(Templates.Wizard.Fields.Back, Randomizer.CreateRandomizer().GetString(4));
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(stepA.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetStep(currentItem);

                //Assert
                Assert.AreEqual(result.Header.Back, null);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetStep_Should_Return_NullEndButton_When_IsNotLastStep()
        {
            //Arrange
            var stepA = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepB = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepC = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            _wizardPage.Add(Templates.Wizard.Fields.Steps, $"{stepA.ID}|{stepB.ID}|{stepC.ID}");
            _wizardPage.Children.Add(stepA);
            _wizardPage.Children.Add(stepB);
            _wizardPage.Children.Add(stepC);
            _wizardPage.Add(Templates.Wizard.Fields.End, Randomizer.CreateRandomizer().GetString(4));
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(stepB.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetStep(currentItem);

                //Assert
                Assert.AreEqual(result.Header.End, null);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetStep_Should_Return_NotNullNextButton_When_IsNotLastStep()
        {
            //Arrange
            var stepA = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepB = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepC = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            _wizardPage.Add(Templates.Wizard.Fields.Steps, $"{stepA.ID}|{stepB.ID}|{stepC.ID}");
            _wizardPage.Children.Add(stepA);
            _wizardPage.Children.Add(stepB);
            _wizardPage.Children.Add(stepC);
            _wizardPage.Add(Templates.Wizard.Fields.Next, Randomizer.CreateRandomizer().GetString(4));
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(stepB.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetStep(currentItem);

                //Assert
                Assert.AreNotEqual(result.Header.Next, null);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetStep_Should_Return_NotNullBackButton_When_IsNotFirstStep()
        {
            //Arrange
            var stepA = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepB = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepC = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            _wizardPage.Add(Templates.Wizard.Fields.Steps, $"{stepA.ID}|{stepB.ID}|{stepC.ID}");
            _wizardPage.Children.Add(stepA);
            _wizardPage.Children.Add(stepB);
            _wizardPage.Children.Add(stepC);
            _wizardPage.Add(Templates.Wizard.Fields.Next, Randomizer.CreateRandomizer().GetString(4));
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(stepB.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetStep(currentItem);

                //Assert
                Assert.AreNotEqual(result.Header.Back, null);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetStep_Should_Return_QueryUrl_When_IsLastStep()
        {
            //Arrange
            var randomUrl = $"domain.com/{Randomizer.CreateRandomizer().GetString(4)}";
            _sessionService.Setup(x => x.Get(Configuration.ReturnUrlArg)).Returns(randomUrl);
            var stepA = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepB = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            var stepC = new DbItem($"Step {Randomizer.CreateRandomizer().GetString(4)}", new ID(Guid.NewGuid()));
            _wizardPage.Add(Templates.Wizard.Fields.Steps, $"{stepA.ID}|{stepB.ID}|{stepC.ID}");
            _wizardPage.Children.Add(stepA);
            _wizardPage.Children.Add(stepB);
            _wizardPage.Children.Add(stepC);
            _wizardPage.Add(Templates.Wizard.Fields.Next, Randomizer.CreateRandomizer().GetString(4));
            _db.Add(_wizardPage);
            var currentItem = _db.GetItem(stepC.ID);
            using (new Sitecore.Sites.SiteContextSwitcher(_context))
            {
                //Act
                var result = _sut.GetStep(currentItem);

                //Assert
                Assert.AreNotEqual(result.Header.End, null);
                Assert.AreEqual(result.Header.End.Target, randomUrl);
            }
        }
        #endregion
    }
}
