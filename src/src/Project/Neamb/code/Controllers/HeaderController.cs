using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Controllers;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Project.Web.Models;
using Sitecore.Mvc.Presentation;
using Neambc.Neamb.Project.Web.Repositories;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Services.ActionReminder;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Enums;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Extensions;
using Neambc.Neamb.Feature.Rakuten.Repositories;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Foundation.Product.Pipelines;


namespace Neambc.Neamb.Project.Web.Controllers
{
    public class HeaderController : ProductBaseController
    {
        private readonly IWebRepository _webRepository;
        private readonly IEligibilityCompIntroLife _eligibilityCompIntroLife;
        private readonly IAmazonS3Repository _amazonS3Repository;
        private readonly string _cacheKeyGroup = "AvatarImage";
        private readonly IGtmService _gtmService;
        private readonly IActionReminderService _actionReminderService;
        private readonly IPageTypeService _pageTypeService;
        private readonly IRegistrationManager _registrationManager;
        private readonly IStoreImportRepository _storeImportRepository;
        private readonly ICookieManager _cookieManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;


        public HeaderController(IWebRepository webRepository, ISessionAuthenticationManager sessionAuthenticationManager, ISessionManager sessionManager,
            ICacheManager cacheManager,
            IEligibilityCompIntroLife eligibilityCompIntroLife, IGlobalConfigurationManager globalConfigurationManager,
            IAmazonS3Repository amazonS3Repository, IGtmService gtmService,
            IActionReminderService actionReminderService,
            IPageTypeService pageTypeService,
            IRegistrationManager registrationManager,
            IStoreImportRepository storeImportRepository,
            ICookieManager cookieManager,
            IAuthenticationAccountManager authenticationAccountManager,
            IAccountServiceProxy serviceManager, IOracleDatabase oracleManager,
            IPipelineService pipelineService,
            IEligibilityManager eligibilityManager, IProductGtmManager productGtmManager,
            IComingSoonRepository comingSoonRepository, ICtaActionRepository ctaActionRepository,
            IProductUtilityManager productUtilityManager
        )
        {
            _webRepository = webRepository;
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _sessionManager = sessionManager;
            _cacheManager = cacheManager;
            _eligibilityCompIntroLife = eligibilityCompIntroLife;
            _globalConfigurationManager = globalConfigurationManager;
            _amazonS3Repository = amazonS3Repository;
            _gtmService = gtmService;
            _actionReminderService = actionReminderService;
            _pageTypeService = pageTypeService;
            _registrationManager = registrationManager;
            _storeImportRepository = storeImportRepository;
            _cookieManager = cookieManager;
            _authenticationAccountManager = authenticationAccountManager;
            base._serviceManager = serviceManager;
            base._oracleManager = oracleManager;
            base._pipelineService = pipelineService;
            base._sessionAuthenticationManager = sessionAuthenticationManager;
            base.pageId = PageContext.Current.Item.ID.ToString();
            base._cacheManager = cacheManager;
            base._sessionManager = sessionManager;
            base._commingSoonItemId = ID.Null;
            base._eligibilityItemId = Templates.ProductOfferCard.Fields.Eligibility;
            base._productCodeDroplinkItemId = Templates.ProductOfferCard.Fields.ProductCodeDroplink;
            base._primaryCtaTypeItemId = Templates.ProductOfferCard.Fields.Type;
            base._primaryCtaLinkItemId = Templates.ProductOfferCard.Fields.Link;
            base._secondaryCtaTypeItemId = ID.Null;
            base._secondaryCtaLinkItemId = ID.Null;
            base._eligibilityManager = eligibilityManager;
            base._componentType = ComponentTypeEnum.OfferLinkHeader;
            base._anonymousItemId = ID.Null;
            base._productGtmManager = productGtmManager;
            base._reminderCtaId = ID.Null;
            base._globalConfigurationManager = globalConfigurationManager;
            base._comingSoonRepository = comingSoonRepository;
            base._ctaActionRepository = ctaActionRepository;
            base._primaryPostDataItemId = Templates.ProductOfferCard.Fields.PostData;
            base._secondaryPostDataItemId = ID.Null;
            base._goalPrimaryItemId = Templates.ProductOfferCard.Fields.Goal;
            base._goalSecondaryItemId = ID.Null;
            base._requiresLogin = Templates.OfferLinkItem.Fields.RequiresLogin;
            base._productUtilityManager = productUtilityManager;
        }

        public ActionResult Header()
        {
            var pathDuplicateRegistration =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID));

            if (IsDuplicateRegistration(pathDuplicateRegistration))
            {
                return Redirect(pathDuplicateRegistration);
            }
            var model = GetHeaderModel();
            return View("/Views/Neamb.Web/Renderings/Header.cshtml", model);
        }

        public ActionResult EmptyHeader()
        {
            var model = GetHeaderModel();
            return View("/Views/Neamb.Web/Renderings/EmptyHeader.cshtml", model);
        }

        public ActionResult HiddenHeader()
        {
            var pathDuplicateRegistration =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID));

            if (IsDuplicateRegistration(pathDuplicateRegistration))
            {
                return Redirect(pathDuplicateRegistration);
            }
            var model = GetHeaderModel();
            return View("/Views/Neamb.Web/Renderings/HiddenHeader.cshtml", model);
        }

        public ActionResult HeaderNewDesign()
        {
            var pathDuplicateRegistration =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID));

            if (IsDuplicateRegistration(pathDuplicateRegistration))
            {
                return Redirect(pathDuplicateRegistration);
            }
            var model = GetHeaderModel();
            return View("/Views/Neamb.Web/Renderings/HeaderNewDesign.cshtml", model);
        }

        private HeaderDTO GetHeaderModel()
        {
            //logout parameter received from zip code verification page where it is required to logout the user
            var logoutParameter = Request.QueryString[ConstantsNeamb.LogoutParameter];
            if (!string.IsNullOrEmpty(logoutParameter))
            {
                _authenticationAccountManager.LogoutUser();
                _cookieManager.RemoveWarmUser();
            }

            var hasNotifications = false;
            var pathZip =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ZipCodeVerificationPage.ID));
            var pathMemberVerification =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Configuration.MemberVerificationPageId));
            var pathRegistration =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.RegistrationPage.ID));


            var refParameter = Request.QueryString[ConstantsNeamb.Ref];
            if (!string.IsNullOrEmpty(refParameter))
            {
                _authenticationAccountManager.RemoveSessionOffersMenu(refParameter.PadLeft(9, '0'));
                _sessionAuthenticationManager.RemoveZipCodeValidationSuccess();
                _webRepository.SetWarmStatus(refParameter);
            }
            else
            {
                //Verify the existence of the cookie and the session without user authenticated
                _webRepository.ProcessCookieWarm();
            }

            SetUtmCampaign();
            SetUtmSource();
            SetUtmMedium();
            SetSeminarId();
            SetUtmTerm();
            SetGclid();
            SetSob();
            

            var accountMembershipDraft = _sessionAuthenticationManager.GetAccountMembershipDraft();

            if (accountMembershipDraft?.Mdsid != null)
            {
                if (accountMembershipDraft.Profile.EditingStatus != EditingStatus.None)
                {
                    var isProfileEditingPage = Sitecore.Context.Item.Template.BaseTemplates.Any(x =>
                        x.ID == new ID(Configuration.ProfileEditingPageTemplateId));
                    // Remove accountMembershipDraft.
                    if (!isProfileEditingPage)
                    {
                        _sessionAuthenticationManager.RemoveAccountMembershipDraft();
                    }
                }
            }

            var model = new HeaderDTO();
            model.Initialize(RenderingContext.Current.Rendering);
            model.Menus = GetMenus(model.Item);

            var headerData = _sessionAuthenticationManager.GetAccountMembership();

            VerifyIceTravellDollarCookie(headerData.Mdsid, headerData.Status);

            model.Status = headerData.Status;
            if (headerData.Status != StatusEnum.Unknown)
            {
                model.Name = headerData.Profile.FirstName;
                model.Mdsid = headerData.Mdsid;
                model.PersonaCode = headerData.Profile.MembershipCategoryCode;
            }
            model.UserIdentifier = GetUserIdentifier(headerData);
            model.IsLoginPage = PageContext.Current.Item.ID == Templates.LoginPage.ID;

            Item[] links = model.SiteSettings != null ? ((Sitecore.Data.Fields.MultilistField)model.SiteSettings.Fields[Templates.AccountMenu.Fields.Links]
                ).GetItems() : new Item[0];
            var linkName = string.Empty;

            if (headerData.Status == StatusEnum.Hot)
            {
                var pageType = _pageTypeService.GetPageType(PageContext.Current.Item);
                if (pageType == PageType.Subscription)
                {
                    _actionReminderService.SetVisited(pageType, headerData.Username);
                }
                //ProcessPagesHelper.ExecuteProcessPageVisited(PageContext.Current.Item, Templates.ProfilePassword.ID,
                //	Templates.SettingSubscription.ID, Templates.CompLife.ID, _cacheManager, headerData.Username);
            }

            if (headerData.Status == StatusEnum.Hot || headerData.Status == StatusEnum.WarmCold ||
                headerData.Status == StatusEnum.WarmHot)
            {
                //Key for session check of intro life user logged
                var keyEligibilityResult = $"{ConstantsNeamb.EligibilityCompIntroLife}{headerData.Mdsid}";
                var keyEligibilityResultIntroLife = $"{ConstantsNeamb.HasIntroLifeEligibility}{headerData.Mdsid}";
                string descriptionEligibility;

                //Retrieve the session value for checking the intro life eligibility
                var valueEligibilityResultFromSession = _sessionManager.RetrieveFromSession<string>(keyEligibilityResult);
                var valueHasIntroLifeFromSession = _sessionManager.RetrieveFromSession<bool>(keyEligibilityResultIntroLife);
                EligibilityResultEnum eligibilityResult;
                bool hasIntroLifeEligibility = false;
                if (string.IsNullOrEmpty(valueEligibilityResultFromSession))
                {
                    //Value from session is null retrieve the value from service
                    Log.Debug($"Starting CompIntroLifeCheck webservice calling {headerData.Mdsid}");
                    var resultEligibility = _eligibilityCompIntroLife.GetResultCompIntroLifeEligibility(headerData.Mdsid);
                    hasIntroLifeEligibility = resultEligibility.IntroEligible;
                    eligibilityResult = _eligibilityCompIntroLife.GetEligibilityResult(resultEligibility);
                    Log.Debug($"Finishing CompIntroLifeCheck webservice calling {headerData.Mdsid}");
                    //Save in session
                    descriptionEligibility = eligibilityResult.GetDescription();
                    _sessionManager.StoreInSession(keyEligibilityResult, descriptionEligibility);
                    _sessionManager.StoreInSession(keyEligibilityResultIntroLife, hasIntroLifeEligibility);
                }
                else
                {
                    //Retrieve from session the eligibility intro life value
                    eligibilityResult = EnumExtensions.FromDescription<EligibilityResultEnum>(valueEligibilityResultFromSession);
                    hasIntroLifeEligibility = valueHasIntroLifeFromSession;
                }

                List<string> offerIdList = new List<string>();
                var keyOffersList = $"{ConstantsNeamb.OfferHeaderListIds}{headerData.Mdsid}";

                foreach (var linkItem in links)
                {
                    var isCompLife = linkItem.IsDerived(Templates.CompLife.ID);
                    var isOfferLink = linkItem.IsDerived(Templates.OfferLink.ID);
                    if (isOfferLink)
                    {
                        offerIdList.Add(linkItem.ID.ToString());
                        var linkItemToBeAdded = new LinkHeaderItem
                        {
                            LinkName = !string.IsNullOrEmpty(linkItem.DisplayName)
                                ? linkItem.DisplayName
                                : linkItem.Name,
                            IsOffer = true,
                            LinkOffers = new List<HeaderOfferItemDto>()
                        };

                        var keyOffersHeader = $"{ConstantsNeamb.OfferHeader}{headerData.Mdsid}{linkItem.ID.ToString()}";
                        List<HeaderOfferItemDto> valueOffersHeaderFromSession = _sessionManager.RetrieveFromSession<List<HeaderOfferItemDto>>(keyOffersHeader);
                        if (valueOffersHeaderFromSession == null || valueOffersHeaderFromSession.Count == 0)
                        {
                            var offerChildItems = linkItem.GetChildren();
                            foreach (Item offerChild in offerChildItems)
                            {
                                var multiRowOfferItemDto = new MultiRowOfferItemDto();
                                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                                ProcessEligibilityActions(multiRowOfferItemDto, offerChild, accountMembership);
                                var headerOfferItemDto = new HeaderOfferItemDto
                                {
                                    ActionClickPrimary = multiRowOfferItemDto.ActionClickPrimary,
                                    ActionClickSecondary = multiRowOfferItemDto.ActionClickSecondary,
                                    ActionPrimary = multiRowOfferItemDto.ActionPrimary,
                                    ActionPrimaryDescription = multiRowOfferItemDto.ActionPrimaryDescription,
                                    ActionPrimaryTargetBlank = multiRowOfferItemDto.ActionPrimaryTargetBlank,
                                    ActionSecondary = multiRowOfferItemDto.ActionSecondary,
                                    ActionSecondaryDescription = multiRowOfferItemDto.ActionSecondaryDescription,
                                    ActionSecondaryTargetBlank = multiRowOfferItemDto.ActionSecondaryTargetBlank,
                                    ComponentId = multiRowOfferItemDto.ComponentId,
                                    HasCheckEligibility = multiRowOfferItemDto.HasCheckEligibility,
                                    HasErrorLink = multiRowOfferItemDto.HasErrorLink,
                                    ResultCheckEligibility = multiRowOfferItemDto.ResultCheckEligibility,
                                    UserStatus = multiRowOfferItemDto.UserStatus
                                };

                                linkItemToBeAdded.LinkOffers.Add(headerOfferItemDto);
                            }
                            _sessionManager.StoreInSession(keyOffersHeader, linkItemToBeAdded.LinkOffers);
                        }
                        else
                        {
                            linkItemToBeAdded.LinkOffers = valueOffersHeaderFromSession;
                        }
                        bool isPreviewMode = Sitecore.Context.PageMode.IsExperienceEditor || Sitecore.Context.PageMode.IsPreview;

                        var countOfferToBeDisplayed = linkItemToBeAdded.LinkOffers.Count(item =>
                        !item.HasCheckEligibility ||
                            (item.HasCheckEligibility &&
                                item.ResultCheckEligibility &&
                                (item.UserStatus != StatusEnum.Cold || item.UserStatus != StatusEnum.Unknown)) ||
                            (item.HasCheckEligibility && (item.UserStatus == StatusEnum.Cold || item.UserStatus == StatusEnum.Unknown)));

                        if (isPreviewMode || countOfferToBeDisplayed > 0)
                        {

                            model.LinkPages.Add(linkItemToBeAdded);
                        }
                    }
                    else if (!isCompLife || (isCompLife && eligibilityResult == EligibilityResultEnum.Eligible))
                    {
                        if (isCompLife && hasIntroLifeEligibility)
                        {
                            var complifePage = Sitecore.Context.Database.GetItem(Templates.ComplimentaryLifePage.ID);
                            linkName = complifePage[Templates.CompLife.Fields.IntroLifeTitle];
                        }
                        else
                        {
                            linkName = !string.IsNullOrEmpty(linkItem.DisplayName)
                                ? linkItem.DisplayName
                                : linkItem.Name;
                        }

                        var navigation = new Navigation
                        {
                            Event = "navigation",
                            NavType = "account",
                            NavText = linkName
                        };
                        var clickAction = _gtmService.GetOnClickEvent(navigation);
                        if (headerData.Status == StatusEnum.WarmCold)
                        {
                            model.LinkPages.Add(new LinkHeaderItem
                            {
                                LinkName = linkName,
                                LinkUrl = LinkManager.GetItemUrl(linkItem),
                                ClickAction = clickAction,
                                HasNotification =
                                    linkItem.IsDerived(Templates.ProfilePassword.ID) ||
                                    linkItem.IsDerived(Templates.SettingSubscription.ID) ||
                                    linkItem.IsDerived(Templates.CompLife.ID)
                                        ? true
                                        : false
                            });
                        }
                        else
                        {
                            var pageType = _pageTypeService.GetPageType(linkItem);
                            var hasNotification = !_actionReminderService.WasVisited(pageType, headerData.Username) && pageType != PageType.NotHandled;
                            hasNotifications |= hasNotification;
                            model.LinkPages.Add(new LinkHeaderItem
                            {
                                LinkName = linkName,
                                LinkUrl = LinkManager.GetItemUrl(linkItem),
                                ClickAction = clickAction,
                                HasNotification = hasNotification
                            });
                        }
                    }
                }//end for
                if (offerIdList != null && offerIdList.Count > 0)
                {
                    _sessionManager.StoreInSession(keyOffersList, offerIdList);
                }
            }

            if (headerData.Status == StatusEnum.WarmCold)
            {
                model.HasNotificationAvatar = true;
            }
            else
            {
                model.HasNotificationAvatar = hasNotifications;
            }

            model.StateLogo = _webRepository.GetLogoImage(headerData.Profile.SeaNumber);
            //Get the avatar image
            if (headerData.Status == StatusEnum.Hot || headerData.Status == StatusEnum.WarmCold ||
                headerData.Status == StatusEnum.WarmHot)
            {
                var keyImageAvatar = $"{_cacheKeyGroup}:{headerData.Profile.Webuserid}";
                byte[] retrievedFile = null;

                if (_cacheManager.ExistInCache(keyImageAvatar))
                {
                    retrievedFile = _cacheManager.RetrieveFromCache<byte[]>(keyImageAvatar);
                }
                else
                {
                    BaseRequestS3 baseRequest = new BaseRequestS3
                    {
                        BucketName = _globalConfigurationManager.BucketNameAvatarImages,
                        Key = headerData.Profile.Webuserid,
                        IsEncrypted = true
                    };
                    retrievedFile =
                        _amazonS3Repository.GetObjectS3<byte[]>(baseRequest);
                }

                if (retrievedFile != null && retrievedFile.Length > 0)
                {
                    _cacheManager.StoreInCache<byte[]>(keyImageAvatar, retrievedFile, DateTime.Now.AddDays(3));

                    //Convert byte arry to base64string

                    var imreBase64Data = Convert.ToBase64String(retrievedFile);
                    //Build the image url

                    model.UserImageUrl = $"data:image/jpg;base64,{imreBase64Data}";
                }
            }
            model.GtmAction = _registrationManager.ExecuteGtmActionRegistrationRedirection(model.PageItem.ID.ToString());
            model.OnLoadEvent = GetOnLoadEvent(Sitecore.Context.Item);

            var isFirsLoadHomePage = _sessionManager.RetrieveFromSession<bool?>("IsFirsLoadHomePage");
            model.DisplaLoginPopup = !isFirsLoadHomePage.HasValue;

            if (!isFirsLoadHomePage.HasValue)
            {
                _sessionManager.StoreInSession<bool>("IsFirsLoadHomePage", true);
                model.DisplaLoginPopup = true;
            }

            return model;
        }

        /// <summary>
        /// Verify if the Ice Travell Dollar cookie exists. If not, the value is requested and then the cookie is created
        /// </summary>
        /// <param name="mdsid">User Id</param>
        private void VerifyIceTravellDollarCookie(string mdsid, StatusEnum headerStatus)
        {
            if (headerStatus == StatusEnum.WarmHot || headerStatus == StatusEnum.Hot)
            {
                _authenticationAccountManager.IceTravelDollarCookie(mdsid);
            }
            else
            {
                _authenticationAccountManager.RemoveIceTravellDollarCookie();
            }
        }

        private bool IsDuplicateRegistration(string pathDuplicateRegistration)
        {
            var duplicateRegistrationEmail = _sessionAuthenticationManager.GetDuplicateRegistrationEmail();

            if (!string.IsNullOrEmpty(duplicateRegistrationEmail) &&
                !pathDuplicateRegistration.Equals(Request.Url.AbsolutePath)) return true;
            return false;
        }

        private UserIdentifier GetUserIdentifier(AccountMembership accountMembership)
        {
            var userMdsid = "none";
            var userPersonaCode = "none";
            var leaCode = "none";
            var seaCode = "none";
            var leaName = "none";
            var seaName = "none";
            if (!string.IsNullOrEmpty(accountMembership?.Mdsid) && int.TryParse(accountMembership.Mdsid, out var mdsid) && mdsid != 0)
            {
                userMdsid = accountMembership.Mdsid;
            }
            if (!string.IsNullOrEmpty(accountMembership?.Profile?.MembershipCategoryCode))
            {
                userPersonaCode = accountMembership.Profile.MembershipCategoryCode;
            }
            if (!string.IsNullOrEmpty(accountMembership?.Profile?.SeaNumber))
            {
                seaCode = accountMembership.Profile.SeaNumber;
            }
            if (!string.IsNullOrEmpty(accountMembership?.Profile?.LeaNumber))
            {
                leaCode = accountMembership.Profile.LeaNumber;
            }
            if (!string.IsNullOrEmpty(accountMembership?.Profile?.SeaName))
            {
                seaName = accountMembership.Profile.SeaName;
            }
            if (!string.IsNullOrEmpty(accountMembership?.Profile?.LeaName))
            {
                leaName = accountMembership.Profile.LeaName;
            }
            return new UserIdentifier()
            {
                UserMdsid = userMdsid,
                UserPersonaCode = userPersonaCode,
                LeaCode = leaCode,
                SeaCode = seaCode,
                LeaName = leaName,
                SeaName = seaName
            };
        }

        private string GetOnLoadEvent(Item item)
        {
            if (item == null)
            {
                return string.Empty;
            }
            var genre = item.Fields[Templates.GenreAttribute.Fields.Genre]?.Value;

            if (string.IsNullOrEmpty(genre))
            {
                return string.Empty;
            }
            return _gtmService.GetGtmEvent(new Content()
            {
                ContentType = genre
            });
        }

        private void SetSeminarId()
        {
            var parameter = Request.QueryString[ConstantsNeamb.SeminarId];
            if (!string.IsNullOrEmpty(parameter))
            {
                _sessionManager.StoreInSession<string>(ConstantsNeamb.SessionSeminaryId, parameter);
            }
        }

        private void SetUtmSource()
        {
            var parameter = Request.QueryString[ConstantsNeamb.UtmSource];
            if (!string.IsNullOrEmpty(parameter))
            {
                if (parameter.Length > 8) parameter = parameter.Substring(0, 8);
                _sessionAuthenticationManager.SaveCellCode(parameter);
            }
        }

        private void SetUtmMedium()
        {
            var parameter = Request.QueryString[ConstantsNeamb.UtmMedium];
            if (!string.IsNullOrEmpty(parameter))
            {
                _sessionAuthenticationManager.SaveMedium(parameter);
            }
        }

        private void SetUtmCampaign()
        {
            var parameter = Request.QueryString[ConstantsNeamb.UtmCampaign];
            if (!string.IsNullOrEmpty(parameter))
            {
                parameter = parameter.Split('?')[0];
                if (parameter.Length > 8) parameter = parameter.Substring(0, 8);
                _sessionAuthenticationManager.SaveCampaignCode(parameter);
            }
            else
            {
                var isWelcomePage = Sitecore.Context.Item.Template.BaseTemplates.Any(x =>
                    x.ID == Templates.MemberWelcome.ID);
                if (isWelcomePage)
                {
                    var campaignCode = Sitecore.Context.Item.Fields[Templates.MemberWelcome.Fields.CampaignCode]?.Value;
                    if (!string.IsNullOrEmpty(campaignCode))
                    {
                        _sessionAuthenticationManager.SaveCampaignCode(campaignCode);
                    }
                }
            }
        }

        private void SetUtmTerm()
        {
            var parameter = Request.QueryString[ConstantsNeamb.UtmTerm];
            if (!string.IsNullOrEmpty(parameter))
            {
                _sessionAuthenticationManager.SaveUtmTerm(parameter);
            }
        }

        private void SetGclid()
        {
            var parameter = Request.QueryString[ConstantsNeamb.Gclid];
            if (!string.IsNullOrEmpty(parameter))
            {
                _sessionAuthenticationManager.SaveGclid(parameter);
            }
        }

        private void SetSob()
        {
            var parameter = Request.QueryString[ConstantsNeamb.Sob];
            if (!string.IsNullOrEmpty(parameter))
            {
                _sessionAuthenticationManager.SaveSob(parameter);
            }
        }

        private IEnumerable<Menu> GetMenus(Item datasource)
        {
            var result = new List<Item>();
            try
            {
                LookupField navigationField = datasource.Fields[Templates.SiteSettings.Fields.HeaderNavigation];
                var navigationFieldTarget = navigationField.TargetItem;
                var menus = navigationFieldTarget.Children.ToArray();
                result.AddRange(menus);
            }
            catch (Exception ex)
            {
                Log.Warn("Header Menu retrieval", ex, this);
            }
            return result.Select(GetMenu);
        }

        private Menu GetMenu(Item item)
        {
            var submenu = item.Fields[Templates.NavigationItem.Fields.Submenu];
            var menuClass = "dropdown mega-dropdown";
            var urlItem = string.Empty;
            var classAnchor = "dropdown-toggle";
            var toggle = "dropdown";
            var hasSubMenu = !submenu.Value.IsEmpty();
            if (!hasSubMenu)
            {
                menuClass = "dropdown mega-dropdown no-icon";
                var itemsMenuLink = ((MultilistField)item.Fields[Templates.NavigationItem.Fields.MenuItemLink]).GetItems();
                if (itemsMenuLink != null && itemsMenuLink.Length > 0)
                {
                    var firstItem = itemsMenuLink.FirstOrDefault();
                    urlItem = firstItem != null
                        ? LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(firstItem.ID))
                        : string.Empty;
                    classAnchor = string.Empty;
                    toggle = string.Empty;
                }
            }
            return new Menu()
            {
                MenuClass = menuClass,
                ClassAnchor = classAnchor,
                HasSubMenu = hasSubMenu,
                Toggle = toggle,
                UrlItem = urlItem,
                ClickAction = _gtmService.GetOnClickEvent(new Navigation
                {
                    Event = "navigation",
                    NavType = "top nav",
                    NavText = item.Fields[Templates.NavigationItem.Fields.MenuItem]?.Value
                }),
                Item = item
            };
        }
    }
}