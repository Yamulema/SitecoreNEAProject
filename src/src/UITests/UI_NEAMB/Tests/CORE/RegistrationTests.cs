using NUnit.Framework;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Email;
using Oshyn.Framework.UITesting.Email.Page;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Selenium;
using System.Linq;
using UI_NEAMB.DTO;
using UI_NEAMB.PagesTypes.CORE;
using UI_NEAMB.PagesTypes.DTO;

namespace UI_NEAMB.Tests.CORE {

    [TestFixture]
    public class RegistrationTests : TestBaseLarge<RegistrationPage> {
        #region Fields
        private DtoFactory<UserRegistration> _factory;
        //private DtoFactory<UserRegistration2> _factory2;
        private UserRegistration _newUser;
#pragma warning disable 649
        private readonly UserRegistration2 _newUser2;
#pragma warning restore 649
        private IWebDriver _driver;
        private IInbox _inbox;
        private ISettings _settings;
        private IWebDriverFactory _factoryMail;
        private IWebDriverServiceCache _serviceCache;
        #endregion

        #region Instrumentation
        [OneTimeSetUp]
        public void OneTimeSetUp() {
            _factory = new DtoFactory<UserRegistration>();
            //_factory2 = new DtoFactory<UserRegistration2>();
            _newUser = _factory.LoadFromFile(@".\TestData\UserRegistration.xml");
            //_newUser2 = _factory2.LoadFromFile(@".\TestData\UserRegistration2.xml");
            _settings = new Settings();
            _serviceCache = new WebDriverServiceCache(_settings);
            _factoryMail = new WebDriverFactory(_settings, null, _serviceCache);
            _driver = _factoryMail.Build();

        }
        [SetUp]
        public void Setup() {
            Page.DeleteCache();

            _inbox = new Inbox(new InboxPage(_driver, Inbox.GetInternalSettings()));
            _newUser.Mail = _inbox.Address;

        }
        [OneTimeTearDown]
        public void OneTimeTeardown() {
            _driver?.Quit();
            _serviceCache?.KillAllServices();
        }
        #endregion

        #region Tests
        [Test, Category("Navigation")]
        public void RegistrationHasAllControls() {
            Page.AssertHasAllControlsForSections(new[] {
                "Header",
                "FormInputs",
                "FormLabels"
            });
        }
        [Test, Category("Registration")]
        public void RegistrationNewUser() {
            Page.RandomUserRequestsRegistration(_newUser);
            Page.AssertHasAllControlsForSections(new[] {
                "ThankYouModal"
            });
            var email = _inbox.CheckInbox(waitSeconds: 5 * 60)
            .FirstOrDefault(x => x.Subject == "Thank you for registering with NEA Member Benefits");
            Assert.IsNotNull(email);
            var linkText = email.Body.Contains("Your registration confirmation!");
        }
        [Test, Category("Registration")]
        public void VerifyEmailAlreadyInUse() {
            Page.RepeatMailRegistration(_newUser);
        }
        [Test, Category("Registration")]
        public void RegistrationProductPage() {
            _newUser = _factory.LoadFromFile(@".\TestData\UserRegistration2.xml");
            Page.RegistrationProductPage(_newUser2);
        }
        #endregion
    }
}
