using System;
using System.Collections.Generic;
using Moq;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model.CreateResetToken;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Model.RetrieveUser;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.CreateResetToken;
using Neambc.Neamb.Foundation.MBCData.Services.Login;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.Membership.Managers;
using SC = Neambc.Seiumb.Foundation.Sitecore;
using Neambc.Neamb.Foundation.Membership.Managers;

namespace Neambc.Neamb.Foundation.Membership.UnitTest.Managers
{
    [TestFixture]
    public class AuthenticationAccountManager
    {

        #region Fields
        private Mock<SUT.ISessionAuthenticationManager> _sessionAuthenticationManagerMock;
        private Mock<IAccountServiceProxy> _serviceManagerMock;
        private Mock<ISessionManager> _sessionManagerMock;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private Mock<IExactTargetClient> _exactTargetClientMock;
        private Mock<IBase64Service> _base64ServiceMock;
        private Mock<SC.IAuthenticationManager> _authenticationManagerMock;
        private Mock<IUrlManager> _urlManagerMock;
        
        private Mock<ILoginUserService> _loginUserServiceMock;
        private Mock<IRetrieveUserManager> _retrieveUserManagerMock;
        private Mock<ICreateResetTokenService> _createResetTokenServiceMock;
        private Mock<ICookieManager> _cookieManagerMock;
        private Mock<IIceTravelDollarsManager> _iceTravelDollarsManagerMock;

        private SUT.ISessionAuthenticationManager _sessionAuthenticationManager;
        private IAccountServiceProxy _serviceManager;
        private ISessionManager _sessionManager;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IExactTargetClient _exactTargetClient;
        private IBase64Service _base64Service;
        private SC.IAuthenticationManager _authenticationManager;
        private IUrlManager _urlManager;
        
        private ILoginUserService _loginUserService;
        private IRetrieveUserManager _retrieveUserManager;
        private ICreateResetTokenService createResetTokenService;
        private ICookieManager _cookieManager;
        private IIceTravelDollarsManager _iceTravelDollarsManager;
        private InstanceFaker _faker;
        private SUT.AuthenticationAccountManager _sut;
        #endregion

        [SetUp]
        public void SetUp()
        {
            _sessionAuthenticationManagerMock = new Mock<SUT.ISessionAuthenticationManager>();
            _serviceManagerMock = new Mock<IAccountServiceProxy>();
            _sessionManagerMock = new Mock<ISessionManager>();
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
            _exactTargetClientMock = new Mock<IExactTargetClient>();
            _base64ServiceMock = new Mock<IBase64Service>();
            _authenticationManagerMock = new Mock<SC.IAuthenticationManager>();
            _urlManagerMock = new Mock<IUrlManager>();            
            _loginUserServiceMock = new Mock<ILoginUserService>();
            _retrieveUserManagerMock = new Mock<IRetrieveUserManager>();
            _createResetTokenServiceMock = new Mock<ICreateResetTokenService>();
            _cookieManagerMock = new Mock<ICookieManager>();
            _iceTravelDollarsManagerMock = new Mock<IIceTravelDollarsManager>();

            _sessionAuthenticationManager = _sessionAuthenticationManagerMock.Object;
            _serviceManager = _serviceManagerMock.Object;
            _sessionManager = _sessionManagerMock.Object;
            _globalConfigurationManager = _globalConfigurationManagerMock.Object;
            _exactTargetClient = _exactTargetClientMock.Object;
            _base64Service = _base64ServiceMock.Object;
            _authenticationManager = _authenticationManagerMock.Object;
            _urlManager = _urlManagerMock.Object;
            //_contactIdentificationRepository = _contactIdentificationRepositoryMock.Object;
            _loginUserService = _loginUserServiceMock.Object;
            _retrieveUserManager = _retrieveUserManagerMock.Object;
            createResetTokenService = _createResetTokenServiceMock.Object;
            _cookieManager = _cookieManagerMock.Object;
            _iceTravelDollarsManager = _iceTravelDollarsManagerMock.Object;

            _faker = new InstanceFaker();

            _sut = new SUT.AuthenticationAccountManager(
                _sessionAuthenticationManager,
                createResetTokenService,
                _sessionManager,
                _globalConfigurationManager,
                _exactTargetClient,
                _base64Service,
                _authenticationManager,
                _urlManager,
                //_contactIdentificationRepository,
                _loginUserService,
                _retrieveUserManager,
                _iceTravelDollarsManager,
                _cookieManager
            );
        }

        #region AuthenticateAccount
        [Test]
        public void AuthenticateAccount_RetrievesAccountWhenResponseOK()
        {
            var loginUser = new LoginResponse()
            {
                Data = new LoginRestModel { LoggedIn = true, MdsId = 999, MdsIdAsString = "000000999" }
            };
            _loginUserServiceMock.Setup(x => x.LoginUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(loginUser);

            var result = _sut.AuthenticateAccount(string.Empty, string.Empty, null, string.Empty);
            Assert.AreSame(loginUser, result);
        }
        [Test]
        public void AuthenticateAccount_AllowsEmptyResponse()
        {
            var loginUser = _faker.Create<LoginResponse>();

            _loginUserServiceMock.Setup(x => x.LoginUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(loginUser);

            var result = _sut.AuthenticateAccount(string.Empty, string.Empty, null, string.Empty);
            Assert.AreSame(loginUser, result);
        }
        #endregion

        #region RetrieveAccount
        [Test]
        public void RetrieveAccount_CreatesProfileHotWhenUserRegistered()
        {
            var retrievedUser = _faker.Create<RetrieveUserModel>();
            retrievedUser.NeaMembershipTypeName = MembershipType.EarlyEnrollProfessionalFt.GetDescription();
            retrievedUser.Registered = true;
            var profile = new Profile();

            _retrieveUserManagerMock
                .Setup(x => x.RetrieveUserNeamb(It.IsAny<string>()))
                .Returns(retrievedUser);

            _retrieveUserManagerMock
                .Setup(x => x.ToProfileModel(It.IsAny<RetrieveUserModel>()))
                .Returns(profile);

            var am = new AccountMembership { Profile = null };
            var mdsId = "123";
            _sut.RetrieveAccount(am, mdsId);
            Assert.IsNotNull(am.Profile);
            Assert.AreEqual(am.Status, StatusEnum.Hot);
        }
        [Test]
        public void RetrieveAccount_CreatesProfileWhenResponseOK() {
            var retrievedUser = new RetrieveUserModel {
                FirstName = "Test First Name",
                LastName = null,
                StreetAddress = "Street Address",
                City = "City",
                NeaCurrentMember = true,
                Dob = "07-11-1980"
            };
            retrievedUser.NeaMembershipTypeName = MembershipType.EarlyEnrollProfessionalFt.GetDescription();
            retrievedUser.Registered = false;
            var profile = new Profile {
                FirstName = "Test First Name",
                LastName = string.Empty,
                StreetAddress = "Street Address",
                City = "City",
                DateOfBirth = "26081993",
                IsNeaCurrentMember = true
            };

            _retrieveUserManagerMock
                .Setup(x => x.RetrieveUserNeamb(It.IsAny<string>()))
                .Returns(retrievedUser);

            _retrieveUserManagerMock
                .Setup(x => x.ToProfileModel(retrievedUser))
                .Returns(profile);

            var am = new AccountMembership { Profile = null };
            var mdsId = "123";
            _sut.RetrieveAccount(am, mdsId);

            Assert.AreEqual(am.Status, StatusEnum.WarmCold);
            var p = am.Profile;
            Assert.AreEqual("Test First Name", p.FirstName, "FirstName");
            Assert.IsEmpty(p.LastName, "Testing receiving null property");
            Assert.AreEqual("Street Address", p.StreetAddress, "StreetAddress");
            Assert.AreEqual("City", p.City, "City");
            Assert.AreEqual("26081993", p.DateOfBirth, "DateOfBirth");
            Assert.AreEqual(retrievedUser.Email, p.Email, "Email");
            Assert.AreEqual(retrievedUser.EmailPermissionIndicator, p.EmailPermissionIndicator, "EmailPermissionIndicator");
            Assert.AreEqual(retrievedUser.IaId, p.IAId, "IAId");
            Assert.AreEqual(retrievedUser.MembershipCategoryCode, p.MembershipCategoryCode, "MembershipCategoryCode");
            Assert.AreEqual(true, p.IsNeaCurrentMember, "IsNeaCurrentMember");
            Assert.AreEqual(retrievedUser.NeaMembershipType, p.NeaMembershipType, "NeaMembershipType");
            Assert.AreEqual(retrievedUser.Phone, p.Phone, "Phone");
            //Assert.AreEqual(retrievedUser.Registered, p.Registered);
            //Assert.AreEqual(retrievedUser.SeaName, p.SeaName, "SeaName");
            //-- Assert.AreEqual(retrievedUser.SeaNumber, p.SeaNumber, "SeaNumber");
            //Assert.AreEqual(retrievedUser.Seiucurrentmember, p.SeiuCurrentMemberFlag);
            //Assert.AreEqual(retrievedUser.SeiuLocalName, p.SeiuLocalName, "SeiuLocalName");
            //Assert.AreEqual(retrievedUser.SeiuLocalNumber, int.Parse(p.SeiuLocalNumber), "SeiuLocalNumber");
            //Assert.AreEqual(retrievedUser.StateCode, p.StateCode, "StateCode");
            //Assert.AreEqual(retrievedUser.UnionId, int.Parse(p.UnionId), "UnionId");
            //Assert.AreEqual(retrievedUser.WebUserId, int.Parse(p.Webuserid), "Webuserid");
            //Assert.AreEqual(retrievedUser.ZipCode, p.ZipCode, "ZipCode");
            //Assert.AreEqual(retrievedUser.NewEnvironmentIndicator, p.NewEnvInd, "NewEnvInd");
            //Assert.AreEqual(retrievedUser.CompIntroSignDate, p.ComplifesignDate, "ComplifesignDate");
            //Assert.AreEqual(retrievedUser.GenderCode, p.GenderCode, "GenderCode");
            //Assert.AreEqual(retrievedUser.CompIntroEndDate, p.Introlifeenddate, "Introlifeenddate");
            //Assert.AreEqual(retrievedUser.NewMemberSegmentIndicator, p.Newmembersegmentindicator, "Newmembersegmentindicator");
            //Assert.AreEqual(MembershipType.EarlyEnrollProfessionalFt, p.MembershipType, "MembershipType");
        }
        [Test]
        public void RetrieveAccount_AllowsEmptyResponse()
        {
            RetrieveUserModel retrievedUser;
            retrievedUser = null;
            _retrieveUserManagerMock
                .Setup(x => x.RetrieveUserNeamb(It.IsAny<string>()))
                .Returns(retrievedUser);
            var am = new AccountMembership { Profile = null };
            var mdsId = "123";
            _sut.RetrieveAccount(am, mdsId);
            Assert.IsNull(am.Profile);
        }
        #endregion

        #region LoginSitecoreContext

        // This test fails due to a Sitecore configuration error:
        //  System.InvalidOperationException : Could not find configuration node: authentication
        //
        //[Test]
        //public void LoginSitecoreContext_BuildsVirtualUser() {
        //	var name = "name";
        //	var user = new FakeUser(name, true) {
        //		Profile = new FakeUserProfile()
        //	};
        //	_authenticationManagerMock.Setup(x => x.BuildVirtualUser(name, true))
        //		.Returns(user);
        //	_sut.LoginSitecoreContext(name);
        //	Assert.AreEqual(name, user.Profile.Email);
        //	Assert.IsNotNull(((FakeUserProfile)user.Profile).SavedAtUtc);
        //}
        #endregion

        
        //to review
        [Test]
        public void ProcessErrorAuthentication_AccountColdByDefault()
        {
            var loginUser = new LoginResponse()
            {
                Error = new RestError { Code = 12003 }
            };
            var account = new AccountMembership();
            var username = "";
            var pathReset = "";
            _sut.ProcessErrorAuthentication(loginUser, account, username, pathReset);
            Assert.AreEqual(StatusEnum.Cold, account.Status);
        }
        [Test]
        public void ProcessErrorAuthentication_WhenAccountLocked_Email() {
            const string userName = "Joe";

            var loginUser = new LoginResponse()
            {
                Error = new RestError { Code = 12004 }
            };

            var requestResetTokenResponse = new CreateResetTokenResponse
            {
                Success = true,
                Data= new CreateResetTokenModel { FirstName = userName, NewToken = true, ResetToken = "bmVhLm93ZW5AZ21haWwuY29t&s=c6r8tyqzbLTTHtY5adfsGANBgpCS%2f9y%2fT8hsOoENvCk%3d", ExpiresAt= "11/03/2022 02:11:57 PM EST" }
            };
            var account = new AccountMembership();
            var pathReset = "http://neamb.local/reset-password?id=bmVhLm93ZW5AZ21haWwuY29t&s=paQVc7jk5l7QzYzxI2tCidx2td2izPQyQRCudkHR7h8=";
            _urlManagerMock.Setup(x => x.IsValidUrl(pathReset)).Returns(true);
            _createResetTokenServiceMock.Setup(x => x.CreateResetToken(userName, It.IsAny<int>())).Returns(requestResetTokenResponse);
            _sut.ProcessErrorAuthentication(loginUser, account, userName, pathReset);
            Assert.AreEqual(StatusEnum.LockedNewToken, account.Status);
        }
        [Test]
        public void ProcessErrorAuthentication_WhenAccountLocked_Email_ValidToken()
        {
            const string userName = "Joe";

            var loginUser = new LoginResponse()
            {
                Error = new RestError { Code = 12004 }
            };
            var account = new AccountMembership();
            var requestResetTokenResponse = new CreateResetTokenResponse
            {
                Success = true,
                Data = new CreateResetTokenModel { FirstName = userName, NewToken = false, ResetToken = "bmVhLm93ZW5AZ21haWwuY29t&s=c6r8tyqzbLTTHtY5adfsGANBgpCS%2f9y%2fT8hsOoENvCk%3d" }
            };
            _createResetTokenServiceMock.Setup(x => x.CreateResetToken(userName, It.IsAny<int>())).Returns(requestResetTokenResponse);
            var pathReset = "";
            _sut.ProcessErrorAuthentication(loginUser, account, userName, pathReset);
            Assert.AreEqual(StatusEnum.LockedOldToken, account.Status);
        }
        [Test]
        public void SendExactTargetResetEmailWhenPathResetParameterIsWrong()
        {
            Assert.Throws<ArgumentException>(() => _sut.SendExactTargetResetEmail(
                new ExactTargetResetEmail
                {
                    UserName = "nea.owen@gmail.com",
                    FirstName = "",
                    Token = "",
                    ResetPath = null,
                    CancelPath = null,
                    ExpiresAt = null,
                    ResetPasswordEnum = ResetPasswordEnum.Locked
                }));
        }

        [Test]
        public void SendExactTargetResetEmailWhenPathResetParameterIsNotUrl()
        {
            _urlManagerMock.Setup(x => x.IsValidUrl(It.IsAny<string>())).Returns(false);
            Assert.Throws<ArgumentException>(() => _sut.SendExactTargetResetEmail(
                new ExactTargetResetEmail
                {
                    UserName = "nea.owen@gmail.com",
                    FirstName = "",
                    Token = "",
                    ResetPath = "/reset-password?id=bmVhLm93ZW5AZ21haWwuY29t&s=Na5w3fXVf%2fcDnRSeGyCIK%2b%2bldB3BtUnNmlx%2f01cGOtY%3d",
                    CancelPath = null,
                    ExpiresAt = null,
                    ResetPasswordEnum = ResetPasswordEnum.Locked
                }));
        }

        [Test]
        public void SendExactTargetResetEmailWhenPathCancelParameterIsWrong()
        {
            Assert.Throws<ArgumentException>(() => _sut.SendExactTargetResetEmail(
                new ExactTargetResetEmail
                {
                    UserName = "nea.owen@gmail.com",
                    FirstName = "",
                    Token = "",
                    ResetPath = "http://neamb.local/reset-password?id=bmVhLm93ZW5AZ21haWwuY29t&s=Na5w3fXVf%2fcDnRSeGyCIK%2b%2bldB3BtUnNmlx%2f01cGOtY%3d",
                    CancelPath = null,
                    ExpiresAt = null,
                    ResetPasswordEnum = ResetPasswordEnum.RequestedUser
                }));
        }

        [Test]
        public void SendExactTargetResetEmailWhenInputParameterIsOk()
        {
            string userName = "nea.owen@gmail.com";
            string pathReset = "https://neamb.local/reset-password?id=bmVhLm93ZW5AZ21haWwuY29t&s=Na5w3fXVf%2fcDnRSeGyCIK%2b%2bldB3BtUnNmlx%2f01cGOtY%3d";
            string pathCancel = "https://neamb.local/reset-password?id=bmVhLm93ZW5AZ21haWwuY29t&s=Na5w3fXVf%2fcDnRSeGyCIK%2b%2bldB3BtUnNmlx%2f01cGOtY%3d";
            var requestResetTokenResponse = new CreateResetTokenResponse
            {
                Success = true,
                Data = new CreateResetTokenModel { FirstName = userName, NewToken = false, ResetToken = "bmVhLm93ZW5AZ21haWwuY29t&s=c6r8tyqzbLTTHtY5adfsGANBgpCS%2f9y%2fT8hsOoENvCk%3d", ExpiresAt= "11/03/2022 02:11:57 PM EST" }
            };
            _createResetTokenServiceMock.Setup(x => x.CreateResetToken(userName, It.IsAny<int>())).Returns(requestResetTokenResponse);
            _urlManagerMock.Setup(x => x.IsValidUrl(It.IsAny<string>())).Returns(true);
            _exactTargetClientMock
                .Setup(x => x.SendExactTargetService(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
                .Returns(true);
            var result = _sut.SendExactTargetResetEmail(
                new ExactTargetResetEmail
                {
                    UserName = userName,
                    FirstName = requestResetTokenResponse.Data.FirstName,
                    Token = requestResetTokenResponse.Data.ResetToken,
                    ResetPath = pathReset,
                    CancelPath = pathCancel,
                    ExpiresAt = requestResetTokenResponse.Data.ExpiresAt,
                    ResetPasswordEnum = ResetPasswordEnum.Locked
                });
            Assert.IsTrue(result.ResultExactTarget);
        }
    }
    
}

