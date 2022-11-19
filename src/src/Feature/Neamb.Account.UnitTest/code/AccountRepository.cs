using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model.CompIntroLife;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.Links;
using SUT = Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Model.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife;
using Neambc.Neamb.Foundation.MBCData.Services.UpdateUserStatus;
using Neambc.Neamb.Foundation.MBCData.Services.DeleteUser;
using Neambc.Neamb.Foundation.MBCData.Model.DeleteUser;
using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;

namespace Neambc.Neamb.Feature.Account.UnitTest {
    [TestFixture]
    public class AccountRepository {
        #region Fields

        private ICookieManager _cookieManager;
        private ISessionAuthenticationManager sessionAuthenticationManager;
        private IAuthenticationAccountManager authenticationAccountManager;
        private Mock<IAuthenticationAccountManager> authenticationAccountManagerMock;
        private IRegistrationManager registrationManager;
        private Mock<IRegistrationManager> registrationManagerMock;
        private IGlobalConfigurationManager globalConfigurationManager;
        private Mock<IAccountServiceProxy> serviceManagerMock;
        private IExactTargetClient exactTargetManager;
        private IAmazonS3Repository amazonS3Repository;
        private IBase64Service base64Service;
        private ISessionManager sessionManager;
        private Mock<IGtmService> gtmServiceMock;
        private IRegistrationRedirection registrationRedirection;
        private Mock<IRegistrationRedirection> registrationRedirectionMock;
        private Mock<ISearchUserNameService> searchUserNameServiceMock;
        private ISearchUserNameService searchUserNameService;
        private Mock<IUpdateUserStatusService> updateUserStatusServiceMock;
        private Mock<IDeleteUserService> deleteUserServiceMock;
        private IDeleteUserService deleteUserService;
        private Mock<ICompIntroLifeService> compIntroLifeServiceMock;
        private ICompIntroLifeService compIntroLifeService;
        private IOracleDatabase oracleManager;
        private SUT.AccountRepository _sut;

        #endregion

        [SetUp]
        public void SetUp() {
            _cookieManager = new Mock<ICookieManager>().Object;
            sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>().Object;
            authenticationAccountManagerMock = new Mock<IAuthenticationAccountManager>();
            authenticationAccountManager = authenticationAccountManagerMock.Object;
            registrationManagerMock = new Mock<IRegistrationManager>();
            registrationManager = registrationManagerMock.Object;
            globalConfigurationManager = new Mock<IGlobalConfigurationManager>().Object;
            serviceManagerMock = new Mock<IAccountServiceProxy>();
            exactTargetManager = new Mock<IExactTargetClient>().Object;
            amazonS3Repository = new Mock<IAmazonS3Repository>().Object;
            base64Service = new Mock<IBase64Service>().Object;
            sessionManager = new Mock<ISessionManager>().Object;
            gtmServiceMock = new Mock<IGtmService>();
            searchUserNameServiceMock = new Mock<ISearchUserNameService>();
            updateUserStatusServiceMock = new Mock<IUpdateUserStatusService>();
            deleteUserServiceMock = new Mock<IDeleteUserService>();
            oracleManager = new Mock<IOracleDatabase>().Object;
            gtmServiceMock = new Mock<IGtmService>();
            registrationRedirectionMock = new Mock<IRegistrationRedirection>();
            registrationRedirection = registrationRedirectionMock.Object;
            searchUserNameServiceMock = new Mock<ISearchUserNameService>();
            searchUserNameService = searchUserNameServiceMock.Object;
            compIntroLifeServiceMock = new Mock<ICompIntroLifeService>();
            compIntroLifeService = compIntroLifeServiceMock.Object;
            deleteUserService = deleteUserServiceMock.Object;
         
           _sut = new SUT.AccountRepository(
                _cookieManager,
                sessionAuthenticationManager,
                authenticationAccountManager,
                registrationManager,
                globalConfigurationManager,
                exactTargetManager,
                amazonS3Repository,
                base64Service,
                sessionManager,
                registrationRedirection,
                searchUserNameService,
                compIntroLifeService,
                deleteUserService,
                oracleManager
            );
        }

        [Test]
        public void HandleItemRedirectionSuccessRuleWhenNoRedirection() {
            Item resultItem = null;
            registrationRedirectionMock.Setup(x => x.GetItemRedirection()).Returns(resultItem);
            var result = _sut.HandleItemRedirectionSuccessRule(null);
            Assert.AreEqual(result, String.Empty);
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]

        public void HandleItemRedirectionSuccessRuleWhenRedirection() {
            const string requestPage = "http:www.neamb.local/productamerican";
            RegistrationDTO model = new RegistrationDTO {
                RequestedPage = requestPage
            };
              string resultUrl = "";
            using (var db = new Db {
                new DbItem("ItemRedirection", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}")) {
                    {
                        "Title", "Testing"
                    }
                }
            }) {
                var itemRedirection = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}"));
                registrationRedirectionMock.Setup(x => x.GetItemRedirection()).Returns(itemRedirection);
                resultUrl = _sut.HandleItemRedirectionSuccessRule(model);
            }
            Assert.AreNotEqual(resultUrl, String.Empty);
            StringAssert.EndsWith(requestPage, resultUrl);
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void HandleItemRedirectionSuccessRuleWhenRedirectionNoRequestedPage() {
            RegistrationDTO model = new RegistrationDTO();

            string resultUrl = "";
            using (var db = new Db {
                new DbItem("ItemRedirection", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}")) {
                    {
                        "Title", "Testing"
                    }
                }
            }) {
                var itemRedirection = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}"));
                registrationRedirectionMock.Setup(x => x.GetItemRedirection()).Returns(itemRedirection);
                resultUrl = _sut.HandleItemRedirectionSuccessRule(model);
                Assert.AreEqual(resultUrl, LinkManager.GetItemUrl(itemRedirection));
            }
            Assert.AreNotEqual(resultUrl, String.Empty);
        }

        [Test]
        public void SetGtmActionRegistrationWhenModelHasError() {
            RegistrationEventResultEnum inputResult = RegistrationEventResultEnum.Failed;
            RegistrationEventResultEnum capturedArg1 = RegistrationEventResultEnum.None;
            RegistrationDTO model = new RegistrationDTO {
                IsValid = false
            };
            registrationManagerMock.Setup(x => x.GetGtmActionRegistration(It.IsAny<RegistrationEventResultEnum>()))
                .Callback((object x) => {
                    capturedArg1 = (RegistrationEventResultEnum) x;
                })
                .Returns("");
            _sut.SetGtmActionRegistration(model);
            Assert.AreEqual(inputResult.GetDescription(), capturedArg1.GetDescription());
        }

        [Test]
        public void SetGtmActionRegistrationWhenModelHasSuccess() {
            RegistrationEventResultEnum inputResult = RegistrationEventResultEnum.Success;
            RegistrationEventResultEnum capturedArg1 = RegistrationEventResultEnum.None;
            RegistrationDTO model = new RegistrationDTO {
                IsValid = true,
                HasGeneralError = false,
                HasDuplicateAccount = false
            };
            registrationManagerMock.Setup(x => x.GetGtmActionRegistration(It.IsAny<RegistrationEventResultEnum>()))
                .Callback((object x) => {
                    capturedArg1 = (RegistrationEventResultEnum) x;
                })
                .Returns("");
            _sut.SetGtmActionRegistration(model);
            Assert.AreEqual(inputResult.GetDescription(), capturedArg1.GetDescription());
        }

        [Test]
        public void ValidateUserNameWhenNullParameter() {
            SearchUserNameResponse request = null;
            Assert.Throws<ArgumentNullException>(() => _sut.ValidateUserName(ref request, null));
        }

        [Test]
        public void ValidateUserNameWhenUserIsAvailable() {
            const string userName = "mpancho@oshyn.com";
            SearchUserNameResponse resultService = new SearchUserNameResponse {
                Data = new SearchUserNameModel {
                    Registered = true,
                    WebUserId = ""
                },
                Error = null,
                Success = true
            };
            SearchUserNameResponse resultServiceInput = new SearchUserNameResponse();
            RegistrationDTO model = new RegistrationDTO {
                Email = userName
            };
        }
        [Test]
        public void ValidateUserNameWhenUserIsNotAvailable() {
            const string userName = "mpancho@oshyn.com";
            SearchUserNameResponse resultService = new SearchUserNameResponse {
                Data = new SearchUserNameModel
                {
                    Registered = false,
                    WebUserId = ""
                },
                Error = null,
                Success = true
            };
            SearchUserNameResponse resultServiceInput = new SearchUserNameResponse();
            RegistrationDTO model = new RegistrationDTO {
                Email = userName
            };

        }


        [Test]
        public void CallRegisterUserWhenReturnOk() {
            const string email = "mpancho@oshyn.com";
            const string firstName = "Rosa";
            const string lastName = "Esp";
            const string birthdate = "08151966";
            const string address = "251 Monroe Blvd.";
            const string city = "Charleston";
            const string zip = "29401";
            const string phone = "8035559987";
            const string password = "secret12";
            const bool expectedValue = true;
            RegistrationDTO model = new RegistrationDTO
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthdate,
                Address = address,
                City = city,
                Zip = zip,
                Phone = phone,
                Password = password,
            };
            AccountMembership accountMembership = new AccountMembership();
            RegisterUserResponse registerUserResponse = new RegisterUserResponse {
                Success = true,
                Data = new RegisterUserModel {
                    Registered = expectedValue,
                    WebuserId = "123"
                }
            };

            registrationManagerMock.Setup(x => x.RegisterAccount(It.IsAny<AccountMembership>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(registerUserResponse);
            var result = _sut.CallRegisterUser(model, accountMembership);
            Assert.IsTrue(result);
            
            Assert.AreEqual(model.Email, accountMembership.Username);
            Assert.AreEqual(model.FirstName, accountMembership.Profile.FirstName);
            Assert.AreEqual(model.LastName, accountMembership.Profile.LastName);
            Assert.AreEqual(model.BirthDate, accountMembership.Profile.DateOfBirth);
            Assert.AreEqual(model.Address, accountMembership.Profile.StreetAddress);
            Assert.AreEqual(model.City, accountMembership.Profile.City);
            Assert.AreEqual(model.Zip, accountMembership.Profile.ZipCode);
            Assert.AreEqual(model.Phone, accountMembership.Profile.Phone);
        }

        [Test]
        public void CallRegisterUserWhenReturnNotRegistered()
        {
            const string email = "mpancho@oshyn.com";
            const string firstName = "Rosa";
            const string lastName = "Esp";
            const string birthdate = "08151966";
            const string address = "251 Monroe Blvd.";
            const string city = "Charleston";
            const string zip = "29401";
            const string phone = "8035559987";
            const string password = "secret12";
            const bool expectedValue = false;
            RegistrationDTO model = new RegistrationDTO
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthdate,
                Address = address,
                City = city,
                Zip = zip,
                Phone = phone,
                Password = password,
            };
            AccountMembership accountMembership = new AccountMembership();
            RegisterUserResponse registerUserResponse = new RegisterUserResponse
            {
                Success = true,
                Data = new RegisterUserModel
                {
                    Registered = expectedValue,
                    WebuserId = "123"
                }
            };

            registrationManagerMock.Setup(x => x.RegisterAccount(It.IsAny<AccountMembership>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(registerUserResponse);
            var result = _sut.CallRegisterUser(model, accountMembership);
            Assert.IsFalse(result);

            Assert.AreEqual(model.Email, accountMembership.Username);
            Assert.AreEqual(model.FirstName, accountMembership.Profile.FirstName);
            Assert.AreEqual(model.LastName, accountMembership.Profile.LastName);
            Assert.AreEqual(model.BirthDate, accountMembership.Profile.DateOfBirth);
            Assert.AreEqual(model.Address, accountMembership.Profile.StreetAddress);
            Assert.AreEqual(model.City, accountMembership.Profile.City);
            Assert.AreEqual(model.Zip, accountMembership.Profile.ZipCode);
            Assert.AreEqual(model.Phone, accountMembership.Profile.Phone);
        }

        [Test]
        public void CallRegisterUserWhenReturnError() {
            const string email = "mpancho@oshyn.com";
            const string firstName = "Rosa";
            const string lastName = "Esp";
            const string birthdate = "08151966";
            const string address = "251 Monroe Blvd.";
            const string city = "Charleston";
            const string zip = "29401";
            const string phone = "8035559987";
            const string password = "secret12";

            RegistrationDTO model = new RegistrationDTO
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthdate,
                Address = address,
                City = city,
                Zip = zip,
                Phone = phone,
                Password = password,
            };
            AccountMembership accountMembership = new AccountMembership();
            RegisterUserResponse registerUserResponse = new RegisterUserResponse
            {
                Success = false,
               Error = new RestError { Code=401, Messages = new []{ "authentication failed" } }
            };
            registrationManagerMock.Setup(x => x.RegisterAccount(It.IsAny<AccountMembership>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(registerUserResponse);
            var result = _sut.CallRegisterUser(model, accountMembership);
            Assert.IsFalse(result);
            Assert.AreEqual(model.Email, accountMembership.Username);
            Assert.AreEqual(model.FirstName, accountMembership.Profile.FirstName);
            Assert.AreEqual(model.LastName, accountMembership.Profile.LastName);
            Assert.AreEqual(model.BirthDate, accountMembership.Profile.DateOfBirth);
            Assert.AreEqual(model.Address, accountMembership.Profile.StreetAddress);
            Assert.AreEqual(model.City, accountMembership.Profile.City);
            Assert.AreEqual(model.Zip, accountMembership.Profile.ZipCode);
            Assert.AreEqual(model.Phone, accountMembership.Profile.Phone);
        }
        [Test]
        public void AuthenticationAfterRegistrationWhenReturnError() {
            AccountMembership accountMembership = new AccountMembership {
                Username = "mpancho@oshyn.com"
            };
            RegistrationDTO registrationDto = new RegistrationDTO {
                Password = "secret12"
            };
            var result = _sut.AuthenticationAfterRegistration(accountMembership, registrationDto);
            Assert.AreEqual(result, RegistrationStatus.Error);
            Assert.IsTrue(registrationDto.HasGeneralError);
        }

        [Test]
        public void AuthenticationAfterRegistrationWhenReturnOk() {
            const string email = "mpancho@oshyn.com";
            const string password = "secret12";
            SetProfileData(out var profile);

            AccountMembership accountMembership = new AccountMembership {
                Username = email,
                Status = StatusEnum.Hot,
                Mdsid = "994",
                Profile = profile
            };
            RegistrationDTO registrationDto = new RegistrationDTO {
                Password = password
            };
            LoginResponse loginUserDto = new LoginResponse {
                Success = true,
                Data = new LoginRestModel {
                    LoggedIn = true,
                    MdsIdAsString = "000000999",
                    MdsId = 999
                }
            };

            compIntroLifeServiceMock.Setup(x => x.GetCompIntroEligibility(It.IsAny<string>()))
                .Returns(new CompIntroLifeEligibilityModel {
                    IntroEligible = true,
                    CompEligible = true
                });

            authenticationAccountManagerMock.Setup(x => x.AuthenticateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AccountMembership>(), It.IsAny<string>()))
                .Returns(loginUserDto);
            var result = _sut.AuthenticationAfterRegistration(accountMembership, registrationDto);
            Assert.AreEqual(result, RegistrationStatus.Success);
            Assert.IsTrue(registrationDto.ProcessedSucessfully);
        }

        [Test]
        public void AuthenticationAfterRegistrationWhenUserStatusUnknown() {
            const string email = "mpancho@oshyn.com";
            const string password = "secret12";
            SetProfileData(out var profile);

            AccountMembership accountMembership = new AccountMembership {
                Username = email,
                Status = StatusEnum.Unknown,
                Mdsid = "994",
                Profile = profile
            };
            RegistrationDTO registrationDto = new RegistrationDTO {
                Password = password
            };
            LoginResponse loginUserDto = new LoginResponse {
                Success = true,
                Data = new LoginRestModel {
                    LoggedIn = true,
                    MdsIdAsString = "000000999",
                    MdsId = 999
                }
            };

            authenticationAccountManagerMock.Setup(x => x.AuthenticateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AccountMembership>(), It.IsAny<string>()))
                .Returns(loginUserDto);
            var result = _sut.AuthenticationAfterRegistration(accountMembership, registrationDto);
            Assert.AreEqual(result, RegistrationStatus.Error);
            Assert.IsTrue(registrationDto.HasGeneralError);
        }

        [Test]
        public void AuthenticationAfterRegistrationWhenUserDuplicated() {
            const string email = "mpancho@oshyn.com";
            const string password = "secret12";
            SetProfileData(out var profile);

            AccountMembership accountMembership = new AccountMembership {
                Username = email,
                Status = StatusEnum.Unknown,
                Mdsid = "994",
                Profile = profile
            };
            RegistrationDTO registrationDto = new RegistrationDTO {
                Password = password,
                Email = email
            };
            LoginResponse loginUserDto = new LoginResponse {
                Success = true,
                Data = new LoginRestModel {
                    LoggedIn = true,
                    MdsIdAsString = "000000999",
                    MdsId = 999,
                    RegistrationCount = 2
                }
            };

            authenticationAccountManagerMock.Setup(x => x.AuthenticateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AccountMembership>(), It.IsAny<string>()))
                .Returns(loginUserDto);
            var result = _sut.AuthenticationAfterRegistration(accountMembership, registrationDto);
            Assert.AreEqual(result, RegistrationStatus.Duplicated);
        }

        [Test]
        public void AuthenticationAfterRegistrationWhenErrorService() {
            const string email = "mpancho@oshyn.com";
            const string password = "secret12";
            SetProfileData(out var profile);

            AccountMembership accountMembership = new AccountMembership {
                Username = email,
                Status = StatusEnum.Unknown,
                Mdsid = "994",
                Profile = profile
            };
            RegistrationDTO registrationDto = new RegistrationDTO {
                Password = password,
                Email = email
            };
            LoginResponse loginUserDto = new LoginResponse {
                Success = false,
                Error = new RestError {
                    Code = 12003
                }
            };

            authenticationAccountManagerMock.Setup(x => x.AuthenticateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AccountMembership>(), It.IsAny<string>()))
                .Returns(loginUserDto);
            var result = _sut.AuthenticationAfterRegistration(accountMembership, registrationDto);
            Assert.AreEqual(result, RegistrationStatus.Error);
            Assert.IsTrue(registrationDto.HasGeneralError);
        }

        [Test]
        public void ExecuteRegistrationWithErrors() {
            const bool isValid = false;
            SetUserData(out var model, out var viewDataDictionary);
            _sut.ExecuteRegistration(model,
                viewDataDictionary,
                isValid);
            Assert.IsFalse(model.IsValid);
        }
        [Test]
        public void ExecuteRegistrationWithResultValidateUserNameErrorInvocation() {
            const bool isValid = true;
            SetUserData(out var model, out var viewDataDictionary);
            model.ConfirmPassword = "secret12";
            _sut.ExecuteRegistration(model,
                viewDataDictionary,
                isValid);
            Assert.IsFalse(model.IsValid);
            Assert.IsTrue(model.HasGeneralError);
        }

        [Test]
        public void ExecuteRegistrationWithRegisterUserWebServiceError() {
            SetUserData(out var model, out var viewDataDictionary);
            model.ConfirmPassword = "secret12";
            SearchUserNameResponse resultService = new SearchUserNameResponse {
                Data = new SearchUserNameModel {
                    Registered = false,
                    WebUserId = ""
                },
                Error = null,
                Success = true
            };
        }

        [Test]
        public void ExecuteRegistrationWithUserNotLogged() {
            const bool isValid = true;
            SetUserData(out var model, out var viewDataDictionary);
            model.ConfirmPassword = "secret12";
            RegisterUserResponse registerUserResponse = new RegisterUserResponse
            {
                Success = true,
                Data = new RegisterUserModel
                {
                    Registered = true,
                    WebuserId = "123"
                }
            };
            registrationManagerMock.Setup(x => x.RegisterAccount(It.IsAny<AccountMembership>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(registerUserResponse);

            _sut.ExecuteRegistration(model,
                viewDataDictionary,
                isValid);
            Assert.IsTrue(model.HasGeneralError);
        }

        [Test]
        public void ExecuteRegistrationWithUserAlreadyRegistered () {
            SetUserData(out var model, out var viewDataDictionary);
            model.ConfirmPassword = "secret12";
            const bool isValid = true;
            SearchUserNameResponse resultService = new SearchUserNameResponse
            {
                Data = new SearchUserNameModel
                {
                    Registered = false,
                    WebUserId = ""
                },
                Error = null,
                Success = true
            };
            searchUserNameServiceMock.Setup(x => x.SearchUserName(It.IsAny<string>())).Returns(resultService);

            RegisterUserResponse registerUserResponse = new RegisterUserResponse
            {
                Success = true,
                Data = new RegisterUserModel
                {
                    Registered = true,
                    WebuserId = "123"
                }
            };
            registrationManagerMock.Setup(x => x.RegisterAccount(It.IsAny<AccountMembership>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(registerUserResponse);
            LoginResponse loginUserDto = new LoginResponse
            {
                Success = true,
                Data = new LoginRestModel
                {
                    LoggedIn = true,
                    MdsIdAsString = "000000999",
                    MdsId = 999
                }
            };

            authenticationAccountManagerMock.Setup(x => x.AuthenticateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AccountMembership>(), It.IsAny<string>()))
                .Returns(loginUserDto);

            var result = _sut.ExecuteRegistration(model,
                viewDataDictionary,
                isValid);

            Assert.IsTrue(model.HasGeneralError);
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteRegistrationWithUserDuplicated() {
            const bool isValid = true;
            SetUserData(out var model, out var viewDataDictionary);
            model.ConfirmPassword = "secret12";

            SearchUserNameResponse searchUserNameResponse = new SearchUserNameResponse
            {
                Success = true,
                Data = new SearchUserNameModel
                {
                    Registered = false,
                    WebUserId = "1"
                }
            };

            searchUserNameServiceMock.Setup(x => x.SearchUserName(It.IsAny<string>())).Returns(searchUserNameResponse);
            RegisterUserResponse registerUserResponse = new RegisterUserResponse
            {
                Success = true,
                Data = new RegisterUserModel
                {
                    Registered = true,
                    WebuserId = "123"
                }
            };
            registrationManagerMock.Setup(x => x.RegisterAccount(It.IsAny<AccountMembership>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(registerUserResponse);
            LoginResponse loginUserDto = new LoginResponse
            {
                Success = true,
                Data = new LoginRestModel
                {
                    LoggedIn = true,
                    MdsIdAsString = "000000999",
                    MdsId = 999,
                    RegistrationCount = 2
                }
            };

            authenticationAccountManagerMock.Setup(x => x.AuthenticateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<AccountMembership>(), It.IsAny<string>()))
                .Returns(loginUserDto);
            using (var db = new Db {
                new DbItem("ItemRedirection", new ID("{300BAF41-8DF0-4C83-9741-CF3D61529BF8}")) {
                    {
                        "Title", "Testing"
                    }
                }
            })
            {
                var result = _sut.ExecuteRegistration(model,
                    viewDataDictionary,
                    isValid);
                Assert.IsNotNull(result);
            }
            Assert.IsTrue(model.HasDuplicateAccount);
        }
        private void SetUserData(out RegistrationDTO model, out ViewDataDictionary viewDataDictionary) {
            const string email = "mpancho@oshyn.com";
            const string firstName = "Rosa";
            const string lastName = "Esp";
            const string birthdate = "08151966";
            const string address = "251 Monroe Blvd.";
            const string city = "Charleston";
            const string zip = "29401";
            const string phone = "8035559987";
            const string password = "secret12";


            model = new RegistrationDTO {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthdate,
                Address = address,
                City = city,
                Zip = zip,
                Phone = phone,
                Password = password,
                ErrorsEmail = new List<ErrorStatusEnum>(),
                ErrorsBirthDate = new List<ErrorStatusEnum>(),
                ErrorsPassword = new List<ErrorStatusEnum>(),
                ErrorsConfirmPassword = new List<ErrorStatusEnum>(),
                ErrorsFirstName = new List<ErrorStatusEnum>(),
                ErrorsLastName = new List<ErrorStatusEnum>(),
                ErrorsZip = new List<ErrorStatusEnum>()
            };
            viewDataDictionary = new ViewDataDictionary();

            viewDataDictionary.ModelState.Add(new KeyValuePair<string, ModelState>("FirstName", new ModelState()));
            viewDataDictionary.ModelState.Add(new KeyValuePair<string, ModelState>("LastName", new ModelState()));
            viewDataDictionary.ModelState.Add(new KeyValuePair<string, ModelState>("Zip", new ModelState()));
            viewDataDictionary.ModelState.Add(new KeyValuePair<string, ModelState>("Address", new ModelState()));
            viewDataDictionary.ModelState.Add(new KeyValuePair<string, ModelState>("City", new ModelState()));
            viewDataDictionary.ModelState.Add(new KeyValuePair<string, ModelState>("State", new ModelState()));
            viewDataDictionary.ModelState.Add(new KeyValuePair<string, ModelState>("Phone", new ModelState()));
            viewDataDictionary.ModelState.Add(new KeyValuePair<string, ModelState>("Email", new ModelState()));
        }
        private static void SetProfileData(out Profile profile) {
            profile = new Profile {
                FirstName = "Rosa",
                LastName = "Esp",
                EmailPermissionIndicator = "4",
                NewEnvInd = "",
                NeaMembershipType = "83",
                ComplifesignDate = "12212018",
                IsNeaCurrentMember = true,
                Newmembersegmentindicator = ""
            };
        }

        [Test]
        public void DeleteDuplicateRegistrationEmails()
        {
            const string userName = "nea.sally@gmail.com";
            DeleteUserResponse resultService = new DeleteUserResponse
            {
                Data = new DeleteUserResponseData
                {
                    deleted = true
                },
                Error = null,
                Success = true
            };
            DeleteUserResponse resultServiceInput = new DeleteUserResponse();
            DeleteUserRequest model = new DeleteUserRequest
            {
                username = userName
            };
        }

        [Test]
        public void DeleteDuplicateRegistrationEmails_withError()
        {
            const string userName = "nea.sally";
            DeleteUserResponse resultService = new DeleteUserResponse
            {
                Data = new DeleteUserResponseData
                {
                    deleted = false
                },
                Error = new RestError { Code = 10002, Messages = new[] { "Input validation failed" } },
                Success = false
            };
            DeleteUserResponse resultServiceInput = new DeleteUserResponse();
            DeleteUserRequest model = new DeleteUserRequest
            {
                username = userName
            };
        }
    }
}
