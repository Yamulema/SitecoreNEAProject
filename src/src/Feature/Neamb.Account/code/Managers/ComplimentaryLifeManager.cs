using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Services.ActionReminder;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Log = Sitecore.Diagnostics.Log;

namespace Neambc.Neamb.Feature.Account.Managers
{

    [Service(typeof(IComplimentaryLifeManager))]
    public class ComplimentaryLifeManager : IComplimentaryLifeManager
    {

        #region Properties
        public const string UK_VALUE = "UK";
        #endregion

        #region Fields
        private readonly ISessionAuthenticationManager _sessionManager;
        private readonly IProfileManager _profileManager;
        private readonly IOracleDatabase _oracleDatabase;
        private readonly IEmailValidationManager _emailValidationManager;
        private readonly IExactTargetClient _exactTargetClient;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IGtmService _gtmService;
        private readonly IActionReminderService _actionReminderService;
        private readonly IComplimentaryLifeWizardService _complimentaryLifeWizardService;
        private readonly IAccountRepository _accountRepository;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly ICompIntroLifeService _compIntroLifeService;
        #endregion

        #region Constructors
        public ComplimentaryLifeManager(
            ISessionAuthenticationManager sessionManager,
            IProfileManager profileManager,
            IOracleDatabase oracleManager,
            IEmailValidationManager emailValidationManager,
            IExactTargetClient exactTargetClient,
            ISessionAuthenticationManager sessionAuthenticationManager,
            IGtmService gtmService,
            IActionReminderService actionReminderService,
            IComplimentaryLifeWizardService complimentaryLifeWizardService,
            IAccountRepository accountRepository,
            IGlobalConfigurationManager globalConfigurationManager,
            ICompIntroLifeService compIntroLifeService
        )
        {
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _gtmService = gtmService;
            _actionReminderService = actionReminderService;
            _complimentaryLifeWizardService = complimentaryLifeWizardService;
            _profileManager = profileManager ?? throw new ArgumentNullException(nameof(profileManager));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _oracleDatabase = oracleManager ?? throw new ArgumentNullException(nameof(oracleManager));
            _emailValidationManager = emailValidationManager ?? throw new ArgumentNullException(nameof(emailValidationManager));
            _exactTargetClient = exactTargetClient ?? throw new ArgumentNullException(nameof(exactTargetClient));
            _sessionAuthenticationManager = sessionAuthenticationManager ?? throw new ArgumentNullException(nameof(sessionAuthenticationManager));
            _globalConfigurationManager = globalConfigurationManager;
            _compIntroLifeService = compIntroLifeService;
        }
        #endregion

        #region Private Methods        

        private bool ValidateBeneficiariesData(ComplimentaryLifeDTO model)
        {
            var result = true;
            var accountMembershipDraft = _sessionManager.GetAccountMembershipDraft();
            model.PayoutStatus = ErrorStatusEnum.None;
            if (accountMembershipDraft != null)
            {
                var hasInvalidPayouts = accountMembershipDraft.Beneficiaries.Any(x => x.Share <= 0 || x.Share > 100);
                if (hasInvalidPayouts)
                {
                    model.PayoutStatus |= ErrorStatusEnum.InvalidValue;
                    result = false;
                }

                if (model.PayoutTotal != 100 && model.Beneficiaries.Beneficiaries.Count > 0)
                {
                    model.PayoutStatus |= ErrorStatusEnum.PayoutTotalError;
                    result = false;
                }

                if (model.Beneficiaries.Beneficiaries.Count > 6)
                {
                    model.ErrorStatus |= ErrorStatusEnum.GeneralError;
                    result = false;
                }

                var spouseRelationshipItem = Sitecore.Context.Database.GetItem(Items.SpouseRelationshipItem);
                if (spouseRelationshipItem != null)
                {
                    var spouseCode = spouseRelationshipItem.Fields[Templates.MbcDbField.Fields.MbcDbId]?.Value;

                    if (model.Beneficiaries.Beneficiaries.Count(x => x.Relationship == spouseCode) > 1)
                    {
                        model.ErrorStatus |= ErrorStatusEnum.GeneralError;
                        result = false;
                    }
                }
            }

            return result;
        }
        private bool UpdateComplimentaryLife(ComplimentaryLifeDTO complimentaryLifeDto, AccountMembership accountMembership, AccountMembership accountMembershipDraft,bool isToteBag )
        {
            Log.Info("Starting Updating Complimentary Life Form process.", this);
            //If the Draft Profile saving process was successful the mdsid value is updated in Profile session.
            var mdsId = int.TryParse(accountMembership.Mdsid, out var mdsIdInt)
                ? mdsIdInt
                : throw new ArgumentException($"Can't parse to int the mdsid value of {accountMembership.Mdsid}.");

            Log.Info("GetChildBirthYears", this);
            Log.Info("To ComplimentaryLifeDb object", this);
            var complimentaryLifeDb = new ComplimentaryLifeDb()
            {
                Beneficiaries = accountMembershipDraft.Profile.EditingStatus.HasFlag(EditingStatus.BeneficiariesChanged)
                    ? accountMembershipDraft.Beneficiaries.Select(GetBeneficiaryDb).ToList()
                    : GetBeneficiaries(accountMembership.Mdsid).Select(GetBeneficiaryDb).ToList(),
                IndvID = mdsId,
                Flag = isToteBag?1:new int?(),
                CampCode = GetCampaignCode(Configuration.CompLifeDefaultCampaignCode),
                CellCode = GetCellCode(Configuration.CompLifeDefaultCellCode)
            };

            Log.Info("ComplimentaryLifeDb object done.", this);
            complimentaryLifeDto.WasSaved = _oracleDatabase.InsertComplimentaryLife(complimentaryLifeDb);

            Log.Info($"Finishing Updating Complimentary Life Form process with status.{complimentaryLifeDto.WasSaved.ToString()}", this);
            return complimentaryLifeDto.WasSaved;
        }
        private string GetCellCode(string defaultValue)
        {
            var result = _sessionAuthenticationManager.GetCellCode();
            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }
        private string GetCampaignCode(string defaultValue)
        {
            var result = _sessionAuthenticationManager.GetCampaignCode();
            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }
        
        private BeneficiaryDb GetBeneficiaryDb(Beneficiary beneficiary)
        {
            return new BeneficiaryDb()
            {
                FirstName = string.IsNullOrEmpty(beneficiary?.FirstName) ? string.Empty : beneficiary.FirstName,
                MiddleName = string.IsNullOrEmpty(beneficiary?.MiddleInitial) ? string.Empty : beneficiary.MiddleInitial.Trim(),
                LastName = string.IsNullOrEmpty(beneficiary?.LastName) ? string.Empty : beneficiary.LastName,
                EntityName = string.IsNullOrEmpty(beneficiary?.OtherEntityName) ? string.Empty : beneficiary.OtherEntityName,
                Percentage = beneficiary?.Share,
                EmailAddress = beneficiary?.Email,
                Type = beneficiary != null ? GetBnryTyp(beneficiary.Type) : string.Empty,
                RelationshipType = int.TryParse(beneficiary?.Relationship, out var relationshipType) ? relationshipType : new int?()
            };
        }
        private bool AddOrUpdate(BeneficiaryDTO beneficiaryDto)
        {
            bool result;
            try
            {
                var accountMembershipDraft = _sessionManager.GetAccountMembershipDraft();

                var beneficiary = new Beneficiary()
                {
                    Id = string.IsNullOrEmpty(beneficiaryDto.Id) ? Guid.NewGuid().ToString() : beneficiaryDto.Id,
                    Email = beneficiaryDto.Email,
                    Type = beneficiaryDto.Type,
                    Share = beneficiaryDto.PayoutPercentage
                };

                switch (beneficiaryDto.Type)
                {
                    case BeneficiaryType.NamedIndividual:
                        beneficiary.FirstName = beneficiaryDto.FirstName;
                        beneficiary.MiddleInitial = beneficiaryDto.MiddleInitial;
                        beneficiary.LastName = beneficiaryDto.LastName;
                        beneficiary.Relationship = beneficiaryDto.Relationship.SelectedValue;
                        beneficiary.DisplayRelationship = GetAllRelationships()[beneficiaryDto.Relationship.SelectedValue];
                        break;
                    case BeneficiaryType.OtherEntity:
                        beneficiary.OtherEntityName = beneficiaryDto.OtherEntityName;
                        beneficiary.Relationship = Configuration.CompLifeOtherEntityDefaultRelationship;
                        beneficiary.DisplayRelationship = Configuration.CompLifeOtherEntityDefaultDisplayRelationship;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                beneficiary.DisplayName = GetDisplayName(beneficiary);
                beneficiary.DisplayShare = string.Format("{0}% Payout", beneficiaryDto.PayoutPercentage);
                var existingBeneficiary = accountMembershipDraft.Beneficiaries.FirstOrDefault(x => x.Id == beneficiary.Id);
                if (existingBeneficiary != null)
                {
                    //This updates beneficiary data.
                    var index = accountMembershipDraft.Beneficiaries.IndexOf(existingBeneficiary);
                    accountMembershipDraft.Beneficiaries[index] = beneficiary;
                }
                else
                {
                    accountMembershipDraft.Beneficiaries.Add(beneficiary);
                }
                accountMembershipDraft.Profile.EditingStatus |= EditingStatus.BeneficiariesChanged;
                _sessionManager.SaveAccountMembershipDraft(accountMembershipDraft);
                result = true;
            }
            catch (NeambDatabaseException e)
            {
                Log.Error($"Error while saving beneficiary {beneficiaryDto.Email} in session", e, this);
                result = false;
            }
            catch (Exception e)
            {
                Log.Fatal($"Error while saving beneficiary {beneficiaryDto.Email} in session", e, this);
                result = false;
            }

            return result;
        }

        private void DisableSpouse(BeneficiaryDTO beneficiaryDto)
        {
            var accountMembershipDraft = _sessionManager.GetAccountMembershipDraft();
            var spouseRelationshipItem = Sitecore.Context.Database.GetItem(Items.SpouseRelationshipItem);
            if (spouseRelationshipItem == null)
            {
                return;
            }

            var spouseCode = spouseRelationshipItem.Fields[Templates.MbcDbField.Fields.MbcDbId]?.Value;
            var spouse = accountMembershipDraft.Beneficiaries.FirstOrDefault(x => x.Relationship.Equals(spouseCode, StringComparison.InvariantCultureIgnoreCase));
            if (spouse == null || spouse.Id == beneficiaryDto.Id)
            {
                return;
            }

            if (spouseCode != null)
            {
                beneficiaryDto.Relationship.Values.Remove(spouseCode);
            }
        }

        public int GetPayoutTotal()
        {
            var result = 0;
            var accountMembershipDraft = _sessionManager.GetAccountMembershipDraft();
            if (accountMembershipDraft != null)
            {
                result = accountMembershipDraft.Beneficiaries.Sum(x => x.Share);
            }

            return result;
        }
        private void SetDraft(bool force = false)
        {
            var accountMembership = _sessionManager.GetAccountMembership();
            var accountMembershipDraft = _sessionManager.GetAccountMembershipDraft();

            // Saves in session a copy of the current AccountMembership if there is none.
            if (accountMembershipDraft?.Mdsid == null || force)
            {
                Log.Info("Loading AccountMembershipDraft from Oracle.", this);
                // Loads beneficiaries in session.
                accountMembershipDraft = (AccountMembership)accountMembership.Clone();
                accountMembershipDraft.Beneficiaries = GetBeneficiaries(accountMembership.Mdsid);
                 _sessionManager.SaveAccountMembershipDraft(accountMembershipDraft);
            }
        }
        private BeneficiaryType GetBeneficiaryType(string bnryTyp)
        {
            switch (bnryTyp)
            {
                case "I":
                    return BeneficiaryType.NamedIndividual;
                case "N":
                    return BeneficiaryType.OtherEntity;
                default:
                    return BeneficiaryType.NamedIndividual;
            }
        }
        private string GetBnryTyp(BeneficiaryType beneficiaryType)
        {
            switch (beneficiaryType)
            {
                case BeneficiaryType.NamedIndividual:
                    return "I";
                case BeneficiaryType.OtherEntity:
                    return "N";
                default:
                    throw new ArgumentOutOfRangeException(nameof(beneficiaryType), beneficiaryType, null);
            }
        }

        private string GetRelationship(string bnryDesgCd)
        {
            var result = string.Empty;

            var relationshipValuesItem = Sitecore.Context.Database.GetItem(Items.RelationshipValues);
            if (relationshipValuesItem != null)
            {
                var categories = GetCategoriesToDictionary(relationshipValuesItem.Axes.GetDescendants());
                result = categories[bnryDesgCd];
            }
            return result;
        }
        private Dictionary<string, string> GetAllRelationships()
        {
            var result = new Dictionary<string, string>();

            var relationshipValuesItem = Sitecore.Context.Database.GetItem(Items.RelationshipValues);
            if (relationshipValuesItem != null)
            {
                result = GetCategoriesToDictionary(relationshipValuesItem.Axes.GetDescendants());
            }
            return result;
        }

        private Dictionary<string, string> GetCategoriesToDictionary(Item[] getItems)
        {
            try
            {
                return getItems.Where(x => x.Template.BaseTemplates.Any(y => y.ID == Templates.MbcDbField.ID))
                    .ToDictionary(x => x.Fields[Templates.MbcDbField.Fields.MbcDbId]?.Value, x => x.Fields[Templates.CategoryItem.Fields.Value]?.Value);
            }
            catch (Exception e)
            {
                Log.Error("Error while converting categories to Dictionary.", e, this);
            }
            return new Dictionary<string, string>();
        }        
        
        private string GetSubhead(Item datasource, string mdsId, AccountMembership accountMembership)
        {
            //Call the webservice to get the user's eligibility in comp and intro life
            var result = _compIntroLifeService.GetCompIntroEligibility(accountMembership.Mdsid);

            if (result.IntroEligible && string.IsNullOrEmpty(accountMembership.Profile?.ComplifesignDate))
                return datasource.Fields[Templates.ComplementaryLifeInsurance.Fields.NewUserHeader]?.Value;
            
            if (result.CompEligible && string.IsNullOrEmpty(accountMembership.Profile?.ComplifesignDate))
                return datasource.Fields[Templates.ComplementaryLifeInsurance.Fields.CompIntroHeader]?.Value;
            
            return datasource.Fields[Templates.ComplementaryLifeInsurance.Fields.Header]?.Value
                .Replace("[]", _oracleDatabase.SelectLastUpdateDate(mdsId));
        }

        
        private static Dictionary<string, string> GetChildYearRange()
        {
            return Enumerable.Range(DateTime.Now.Year - 79, 80)
                .OrderByDescending(x => x)
                .ToDictionary(x => x.ToString(), x => x.ToString());
        }

        private List<Beneficiary> GetBeneficiaries(string mdsid)
        {
            var beneficiaries = _oracleDatabase.SelectBeneficiaries(mdsid);
            return beneficiaries != null
                ? beneficiaries.Select(ToBeneficiary).ToList()
                : new List<Beneficiary>();
        }

        private Beneficiary ToBeneficiary(ViewBeneficiary viewBeneficiary)
        {
            var result = new Beneficiary()
            {
                Id = Guid.NewGuid().ToString(),
                OtherEntityName = viewBeneficiary.EntityName,
                FirstName = viewBeneficiary.FirstName,
                LastName = viewBeneficiary.LastName,
                MiddleInitial = viewBeneficiary.MiddleName.Trim(),
                Share = viewBeneficiary.DesignatedPts,
                Relationship = viewBeneficiary.DesignatedCd.Trim(),
                DisplayShare = $"{viewBeneficiary.DesignatedPts}% Payout",
                DisplayRelationship = GetRelationship(viewBeneficiary.DesignatedCd.Trim().ToLower()),
                Type = GetBeneficiaryType(viewBeneficiary.BeneficiaryType),
                Email = viewBeneficiary.EmailAddress
            };
            result.DisplayName = GetDisplayName(result);

            return result;
        }

        private string GetDisplayName(Beneficiary beneficiary)
        {
            switch (beneficiary.Type)
            {
                case BeneficiaryType.NamedIndividual:
                    return $"{beneficiary.FirstName} {beneficiary.MiddleInitial} {beneficiary.LastName}";
                case BeneficiaryType.OtherEntity:
                    return $"{beneficiary.OtherEntityName}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Public Methods
        public ComplimentaryLifeDTO ComplimentaryLifeModel(Rendering rendering, AccountMembership accountMembership)
        {
            var datasource = rendering.Item;
            if (accountMembership.Status != StatusEnum.Hot)
            {
                // Mockup data for Experience editor.
                if (Sitecore.Context.PageMode.IsExperienceEditor)
                {
                    return new ComplimentaryLifeDTO(rendering)
                    {
                        Beneficiaries = new BeneficiariesDTO()
                        {
                            Add = datasource.Fields[Templates.ComplementaryLifeInsurance.Fields.Add]?.Value,
                            Tooltip = datasource
                                .Fields[Templates.ComplementaryLifeInsurance.Fields.Beneficiaries_Tooltip]?.Value,
                            SelectedBeneficiaryId = Guid.Empty.ToString(),
                            Item = datasource,
                            Beneficiaries = new List<Beneficiary>
                            {
                                Beneficiary.CreateSample()
                            },
                            AddCta = GetBeneficiariesAddCta()
                        },
                        PayoutTotal = GetPayoutTotal(),
                        IsWizardStep = _complimentaryLifeWizardService.IsWizardStep()
                    };
                }
                return new ComplimentaryLifeDTO(rendering);
            }

            SetDraft();
            var accountMembershipDraft = _sessionManager.GetAccountMembershipDraft();
            var isValidDate = DateTime.TryParseExact(accountMembershipDraft.Profile.DateOfBirth, "MMddyyyy", null, DateTimeStyles.None, out var dateOfBirth);
            var dateOfBirthString = (isValidDate)
                ? $"{dateOfBirth:MMMM d, yyyy}"
                : string.Empty;
            var result = new ComplimentaryLifeDTO(rendering)
            {
                AccountStatus = accountMembership.Status,
                MembershipType = accountMembership.Profile.MembershipType,
                EditingStatus = accountMembershipDraft.Profile.EditingStatus,
                AnonymousUser = datasource.Fields[Templates.ComplementaryLifeInsurance.Fields.AnonymousUser]?.Value,

                Beneficiaries = new BeneficiariesDTO()
                {
                    Add = datasource.Fields[Templates.ComplementaryLifeInsurance.Fields.Add]?.Value,
                    Tooltip = datasource.Fields[Templates.ComplementaryLifeInsurance.Fields.Beneficiaries_Tooltip]?.Value,
                    SelectedBeneficiaryId = Guid.Empty.ToString(),
                    Item = datasource,
                    Beneficiaries = accountMembershipDraft.Beneficiaries,
                    AddCta = GetBeneficiariesAddCta(),
                    IsCtaEnabled = accountMembershipDraft.Beneficiaries.Count < 6,
                    OnClickEvent = _gtmService.GetOnClickEvent(new Foundation.Analytics.Gtm.Account()
                    {
                        Event = "account",
                        AccountSection = "complimentary life insurance",
                        AccountAction = rendering.Item.Fields[Templates.ComplementaryLifeInsurance.Fields.Beneficiaries_Headline]?.Value ?? string.Empty,
                        CtaText = rendering.Item.Fields[Templates.ComplementaryLifeInsurance.Fields.Add]?.Value ?? string.Empty
                    })
                },
                Subhead = GetSubhead(datasource, accountMembershipDraft.Mdsid, accountMembership),
                IsWizardStep = _complimentaryLifeWizardService.IsWizardStep()
            };

            foreach (var beneficiary in result.Beneficiaries.Beneficiaries)
            {
                beneficiary.OnEditClickEvent = _gtmService.GetOnClickEvent(new Foundation.Analytics.Gtm.Account()
                {
                    Event = "account",
                    AccountSection = "complimentary life insurance",
                    AccountAction = rendering.Item.Fields[Templates.ComplementaryLifeInsurance.Fields.Beneficiaries_Headline]?.Value ?? string.Empty,
                    CtaText = "Edit" //This label is not managed by CMS
                });
                beneficiary.OnRemoveClickEvent = _gtmService.GetOnClickEvent(new Foundation.Analytics.Gtm.Account()
                {
                    Event = "account",
                    AccountSection = "complimentary life insurance",
                    AccountAction = rendering.Item.Fields[Templates.ComplementaryLifeInsurance.Fields.Beneficiaries_Headline]?.Value ?? string.Empty,
                    CtaText = "Remove" //This label is not managed by CMS
                });
            }
            result.PayoutTotal = GetPayoutTotal();
            result.GtmAction = _gtmService.GetGtmEvent(new Foundation.Analytics.Gtm.Account()
            {
                Event = "account",
                AccountSection = "complimentary life insurance",
                AccountAction = "Beneficiaries",
                CtaText = "Save" 
            });

            return result;
        }
        private string GetBeneficiariesAddCta()
        {
            return _complimentaryLifeWizardService.IsWizardStep()
                ? _complimentaryLifeWizardService.GetBeneficiariesAddCta()
                : LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.AddBeneficiary));
        }

        public UpdateUserInformationDTO GetUpdateUserInformationModel(
            Rendering rendering, string newcell, string oldcell, string msrName
        )
        {
            SetDraft();
            var datasource = rendering.Item;
            var profileDatasource =
                Sitecore.Context.Database.GetItem(datasource.Fields[Templates.EditUpdateInformation.Fields.Profile]
                    ?.Value);

            var accountMembership = _sessionManager.GetAccountMembership();

            if (accountMembership.Status != StatusEnum.Hot)
            {
                return new UpdateUserInformationDTO(rendering)
                {
                    AccountStatus = accountMembership.Status,
                    DatasourceId = datasource.ToString(),
                    BackCta = new BackCTA()
                    {
                        FriendlyUrl = ((LinkField)datasource.Fields[Templates.EditUpdateInformation.Fields.Back])?.GetFriendlyUrl(),
                        Text = ((LinkField)datasource.Fields[Templates.EditUpdateInformation.Fields.Back])?.Text
                    },
                    ProfileDto = new ProfileDTO(profileDatasource)
                };
            }

            var result = new UpdateUserInformationDTO(rendering)
            {
                AccountStatus = accountMembership.Status,
                DatasourceId = datasource.ToString(),
                BackCta = new BackCTA()
                {
                    FriendlyUrl = ((LinkField)datasource.Fields[Templates.EditUpdateInformation.Fields.Back])?.GetFriendlyUrl(),
                    Text = ((LinkField)datasource.Fields[Templates.EditUpdateInformation.Fields.Back])?.Text
                },
                ProfileDto = _profileManager.GetProfileDto(profileDatasource, newcell, oldcell, msrName, true),
                OnClickEvent = _gtmService.GetOnClickEvent(new Foundation.Analytics.Gtm.Account()
                {
                    Event = "account",
                    AccountSection = "complimentary life insurance",
                    AccountAction = datasource.Fields[Templates.EditUpdateInformation.Fields.Title]?.Value ?? string.Empty,
                    CtaText = datasource.Fields[Templates.EditUpdateInformation.Fields.Submit]?.Value ?? string.Empty
                })
            };

            return result;
        }

        public BeneficiaryDTO GetAddBeneficiaryModel(Item datasource)
        {
            var item = datasource ?? throw new ArgumentNullException(nameof(datasource));
            SetDraft();

            var accountMembership = _sessionManager.GetAccountMembership();

            var result = new BeneficiaryDTO(item)
            {
                Item = item,
                DatasourceId = item.ID.Guid.ToString(),
                Relationship = new MbcDbOption(
                    item,
                    Templates.Beneficiary.Fields.RelationshipLabel,
                    Templates.Beneficiary.Fields.RelationshipValues
                ),
                Type = BeneficiaryType.NamedIndividual,
                FirstName = string.Empty,
                Id = string.Empty,
                BackCta = GetBackCta(datasource),
                AccountStatus = accountMembership.Status,
                MembershipType = accountMembership.Profile.MembershipType,
                PayoutTotal = GetPayoutTotal(),
                OnClickEvent = _gtmService.GetOnClickEvent(new Foundation.Analytics.Gtm.Account()
                {
                    Event = "account",
                    AccountSection = "complimentary life insurance",
                    AccountAction = datasource.Fields[Templates.Beneficiary.Fields.Title]?.Value ?? string.Empty,
                    CtaText = datasource.Fields[Templates.Beneficiary.Fields.Save]?.Value ?? string.Empty
                })
            };

            //Removes spouse option from collection if needed.
            DisableSpouse(result);
            return result;
        }

        private BackCTA GetBackCta(Item datasource)
        {
            return _complimentaryLifeWizardService.IsWizardStep()
                ? new BackCTA()
                {
                    FriendlyUrl = _complimentaryLifeWizardService.GetParentStepUrl(),
                    Text = ((LinkField)datasource.Fields[Templates.Beneficiary.Fields.Back])?.Text
                }
                : new BackCTA()
                {
                    FriendlyUrl = ((LinkField)datasource.Fields[Templates.Beneficiary.Fields.Back])?.GetFriendlyUrl(),
                    Text = ((LinkField)datasource.Fields[Templates.Beneficiary.Fields.Back])?.Text
                };
        }
        public BeneficiaryDTO SaveBeneficiaryViaModel(AccountMembership accountMembership, Item item)
        {
            return (accountMembership.Status != StatusEnum.Hot) ?
                GetAddBeneficiaryModel(item)
                : null;
        }

        /// <summary>
        /// Save any beneficiary when passed status is "hot", return null otherwise
        /// Check beneficiary.WasSaved and all "ErrorStatus" properties for saving confirmation/validation
        /// </summary>
        /// <param name="beneficiaryDto"></param>
        /// <param name="status"></param>
        /// <param name="item"></param>
        /// <param name="viewData"></param>
        /// <returns></returns>
        public BeneficiaryDTO SaveBeneficiaryViaAccount(
                BeneficiaryDTO beneficiaryDto,
                StatusEnum status,
                Item item,
                ViewDataDictionary viewData
            )
        {
            var beneficiary = beneficiaryDto ?? throw new ArgumentNullException(nameof(beneficiaryDto));
            var i = item ?? throw new ArgumentNullException(nameof(item));
            var viewD = viewData ?? throw new ArgumentNullException(nameof(viewData));

            BeneficiaryDTO ret = null;
            if (status == StatusEnum.Hot)
            {
                beneficiaryDto.FillBeneficiaryFromDataSource(i);
                beneficiaryDto.FirstNameErrorStatus = new BeneficiaryValidator().CheckFirstNameErrors(beneficiary, viewD);
                beneficiaryDto.MiddleInitialErrorStatus = new BeneficiaryValidator().CheckMiddleInitialErrors(beneficiary, viewD);
                beneficiaryDto.LastNameErrorStatus = new BeneficiaryValidator().CheckLastNameErrors(beneficiary, viewD);
                beneficiaryDto.EmailErrorStatus = new BeneficiaryValidator().CheckEmailErrors(beneficiary, viewD, _emailValidationManager, _accountRepository);
                beneficiaryDto.PayoutPercentageErrorStatus = new BeneficiaryValidator().CheckPayoutErrors(beneficiary, viewD);
                
                if (beneficiaryDto.Type == BeneficiaryType.OtherEntity)
                {
                    beneficiaryDto.OtherEntityNameErrorStatus = new BeneficiaryValidator().CheckOtherEntityErrors(beneficiary, viewD);
                }

                beneficiaryDto.WasSaved = !beneficiaryDto.HasFieldErrors();
                if (!beneficiaryDto.HasFieldErrors())
                {
                    beneficiaryDto.WasSaved = AddOrUpdate(beneficiaryDto);
                }
                ret = beneficiaryDto;
                ret.PayoutTotal = GetPayoutTotal();
            }
            return ret;
        }
        [Obsolete("Use new version: SaveBeneficiary(BeneficiaryDTO beneficiaryDto, ViewDataDictionary viewData, Item item)")]
        public BeneficiaryDTO SaveBeneficiary(BeneficiaryDTO beneficiaryDto, ViewDataDictionary viewData)
        {
            return SaveBeneficiary(beneficiaryDto, viewData, Sitecore.Context.Item);
        }
        public BeneficiaryDTO SaveBeneficiary(BeneficiaryDTO beneficiaryDto, ViewDataDictionary viewData, Item item)
        {
            var accountMembership = _sessionManager.GetAccountMembership();
            if (accountMembership.Status == StatusEnum.Hot)
            {
                return SaveBeneficiaryViaAccount(beneficiaryDto, accountMembership.Status, item, viewData);
            }
            else
            {
                return SaveBeneficiaryViaModel(accountMembership, item);
            }
        }

        public EditBeneficiaryDTO EditBeneficiary(BeneficiaryDTO beneficiaryDto, ViewDataDictionary viewData)
        {
            var accountMembership = _sessionManager.GetAccountMembership();
            if (accountMembership.Status == StatusEnum.Hot)
            {
                var item = Sitecore.Context.Database.GetItem(new ID(beneficiaryDto.DatasourceId));
                var processedBeneficiary = SaveBeneficiary(beneficiaryDto, viewData, Sitecore.Context.Item);

                //Maps parent class into child.
                var result = new EditBeneficiaryDTO(processedBeneficiary)
                {
                    AccountStatus = accountMembership.Status,
                    BackCta = GetBackCta(item),
                };

                return result;
            }
            return new EditBeneficiaryDTO(Sitecore.Context.Item)
            {
                AccountStatus = accountMembership.Status
            };
        }
        public ComplimentaryLifeDTO Save(Rendering rendering, bool isToteBag)
        {
            var accountMembership = _sessionManager.GetAccountMembership();
            var model = ComplimentaryLifeModel(rendering, accountMembership);
            model.WasSaved = false;

            var datasource = rendering.Item;
            
            model.PayoutTotal = GetPayoutTotal();


            // Cancel attempt to Save if user is not in Hot state.
            if (accountMembership != null)
            {
                if (accountMembership.Status != StatusEnum.Hot)
                {
                    return model;
                }
            }

            var accountMembershipDraft = _sessionManager.GetAccountMembershipDraft();

            // Validate Beneficiaries data.
            var isValid = ValidateBeneficiariesData(model);
            if (!isValid)
            {
                return model;
            }

            // Returns if there are no additional changes.
            if (!accountMembershipDraft.Profile.EditingStatus.HasFlag(EditingStatus.BeneficiariesChanged))
            {
                // Reloads AccountMembershipDraft from DB 
                SetDraft(true);
                model = ComplimentaryLifeModel(rendering, accountMembership);
                model.WasSaved = true;
                accountMembershipDraft.Profile.EditingStatus |= EditingStatus.Saved;
                _sessionManager.SaveAccountMembershipDraft(accountMembershipDraft);
                return model;
            }

            var wasSaved = UpdateComplimentaryLife(model, accountMembership, accountMembershipDraft,isToteBag);
            Log.Debug($"Starting email complimentary ");
            if (wasSaved)
            {
                Log.Debug($"Starting email complimentary enter ");
                // Send Email via ExactTarget
                var wasEmailSent = _exactTargetClient.TriggeredSend(new ExactTargetEmail()
                {
                    CustomerKey = Configuration.CompLifeEmailCustomerKey,
                    SubscriberKey = accountMembershipDraft.Mdsid,
                    EmailTo = accountMembershipDraft.Profile.Email,
                    Attributes = new Dictionary<string, string>()
                    {
                        { "FIRST_NAME", accountMembershipDraft.Profile.FirstName },
                        { "LAST_NAME", accountMembershipDraft.Profile.LastName },
                        { "CELL_CODE", isToteBag?_globalConfigurationManager.CompLifeEmailCellCodeToteBag:GetCellCode(Configuration.CompLifeEmailDefaultCellCode) },
                        { "CAMPAIGN_CD", Configuration.CompLifeEmailDefaultCampaignCode },
                        { "INDIVIDUAL_ID", accountMembershipDraft.Mdsid },
                        { "COMP_OR_INTRO", "Complimentary" },
                    }
                });
                Log.Debug($"Starting email complimentary end {wasEmailSent} ");

                if (!wasEmailSent)
                {
                    Log.Warn($"Error while sending Complimentary Life email to {accountMembershipDraft.Profile.Email}", this);
                }

                // Reloads AccountMembershipDraft from DB 
                SetDraft(true);
                model = ComplimentaryLifeModel(rendering, accountMembership);
                model.WasSaved = true;
                accountMembershipDraft.Profile.EditingStatus |= EditingStatus.Saved;
                _sessionManager.SaveAccountMembershipDraft(accountMembershipDraft);
                _actionReminderService.SetVisited(PageType.Complife, accountMembership.Username);
                return model;
            }
            else
            {
                model.ErrorStatus |= ErrorStatusEnum.GeneralError;
                return model;
            }
        }

        public bool Remove(string beneficiaryId)
        {
            var result = false;
            try
            {
                var accountMembershipDraft = _sessionManager.GetAccountMembershipDraft();
                var existingBeneficiary = accountMembershipDraft.Beneficiaries.FirstOrDefault(x => x.Id == beneficiaryId);
                if (existingBeneficiary != null)
                {
                    accountMembershipDraft.Beneficiaries.Remove(existingBeneficiary);
                    accountMembershipDraft.Profile.EditingStatus |= EditingStatus.BeneficiariesChanged;
                    _sessionManager.SaveAccountMembershipDraft(accountMembershipDraft);
                    result = true;
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error while removing beneficiary {beneficiaryId} in draft profile session.", e, this);
                result = false;
            }

            return result;
        }

        public EditBeneficiaryDTO GetEditBeneficiaryModel(Rendering rendering, string beneficiaryId)
        {
            SetDraft();

            var item = rendering.Item;
            var accountMembership = _sessionManager.GetAccountMembership();
            if (accountMembership.Status == StatusEnum.Hot)
            {
                var accountMembershipDraft = _sessionManager.GetAccountMembershipDraft();
                var beneficiary = accountMembershipDraft.Beneficiaries.FirstOrDefault(x => x.Id == beneficiaryId);

                var registrationLink = (LinkField)item.Fields[Templates.Beneficiary.Fields.Back];
                if (registrationLink?.TargetItem != null)
                {
                    LinkManager.GetItemUrl(registrationLink.TargetItem);
                }

                if (beneficiary != null)
                {
                    var result = new EditBeneficiaryDTO(rendering)
                    {
                        Item = item,
                        DatasourceId = item.ID.Guid.ToString(),
                        Relationship = new MbcDbOption(
                            item, Templates.Beneficiary.Fields.RelationshipLabel,
                            Templates.Beneficiary.Fields.RelationshipValues, beneficiary.Relationship.Trim()),
                        Type = beneficiary.Type,
                        FirstName = beneficiary.FirstName,
                        LastName = beneficiary.LastName,
                        MiddleInitial = beneficiary.MiddleInitial,
                        OtherEntityName = beneficiary.OtherEntityName,
                        Email = beneficiary.Email,
                        Id = beneficiaryId,
                        PayoutPercentage = beneficiary.Share,
                        PayoutTotal = GetPayoutTotal(),
                        BackCta = GetBackCta(item),
                        AccountStatus = _sessionManager.GetAccountMembership().Status,
                        MembershipType = accountMembership.Profile.MembershipType,
                        OnClickEvent = _gtmService.GetOnClickEvent(new Foundation.Analytics.Gtm.Account()
                        {
                            Event = "account",
                            AccountSection = "complimentary life insurance",
                            AccountAction = rendering.Item.Fields[Templates.Beneficiary.Fields.Title]?.Value ?? string.Empty,
                            CtaText = rendering.Item.Fields[Templates.Beneficiary.Fields.Save]?.Value ?? string.Empty
                        })
                    };
                    DisableSpouse(result);
                    return result;
                }
                Log.Warn($"Beneficiary with Id:{beneficiaryId} not found in profile draft.", this);
                return null;
            }
            return new EditBeneficiaryDTO(item)
            {
                Item = item,
                DatasourceId = item.ID.Guid.ToString(),
                Relationship = new MbcDbOption(
                    item, Templates.Beneficiary.Fields.RelationshipLabel,
                    Templates.Beneficiary.Fields.RelationshipValues),
                Type = BeneficiaryType.NamedIndividual,
                FirstName = string.Empty,
                Id = string.Empty,
                BackCta = GetBackCta(item),
                AccountStatus = accountMembership.Status
            };
        }
        #endregion
    }
}