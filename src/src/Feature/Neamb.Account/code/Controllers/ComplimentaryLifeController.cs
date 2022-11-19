using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers {
	public class ComplimentaryLifeController : SitecoreController {
		private readonly IComplimentaryLifeManager _complimentaryLifeManager;
		private readonly IEligibilityCompIntroLife _eligibilityCompIntroLife;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IComplimentaryLifeWizardService _complimentaryLifeWizardService;
        private readonly IWizardService _wizardService;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly ISessionManager _sessionManager;

        public ComplimentaryLifeController(IComplimentaryLifeManager complimentaryLifeManager, IEligibilityCompIntroLife eligibilityCompIntroLife, ISessionAuthenticationManager sessionAuthenticationManager, IAccountServiceProxy accountServiceProxy,
            IComplimentaryLifeWizardService complimentaryLifeWizardService,
            IWizardService wizardService, IGlobalConfigurationManager globalConfigurationManager,
            ISessionManager sessionManager
        ) {
			_complimentaryLifeManager = complimentaryLifeManager;
			_eligibilityCompIntroLife = eligibilityCompIntroLife;
			_sessionAuthenticationManager = sessionAuthenticationManager;
            _complimentaryLifeWizardService = complimentaryLifeWizardService;
            _wizardService = wizardService;
            _eligibilityCompIntroLife = eligibilityCompIntroLife;
            _globalConfigurationManager = globalConfigurationManager;
            _sessionManager = sessionManager;
        }

		#region Complimentary Life
		public ActionResult ComplimentaryLife() {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            var model = _complimentaryLifeManager.ComplimentaryLifeModel(RenderingContext.Current.Rendering, accountMembership);
            var flagUrlParameter = Request.QueryString[ConstantsNeamb.FlagUrlParameter];
            if (!string.IsNullOrEmpty(flagUrlParameter)) {
                _sessionManager.StoreInSession(ConstantsNeamb.FlagUrlParameterSession, flagUrlParameter);
            } else {
                _sessionManager.Remove(ConstantsNeamb.FlagUrlParameterSession);
            }
            var postbackResult = Request.QueryString[Configuration.ComplimentaryLifeResultParameterName];

			if (bool.TryParse(postbackResult, out var wasSaved)) {
				if (wasSaved) {
					if (model.EditingStatus.HasFlag(EditingStatus.Saved)) {
						model.WasSaved = true;
						var draft = _sessionAuthenticationManager.GetAccountMembershipDraft();
						draft.Profile.EditingStatus &= ~EditingStatus.Saved;
						_sessionAuthenticationManager.SaveAccountMembershipDraft(draft);
					}
				}
			}

			model.PathComplimentary = Request != null && Request.Url != null ? Request.Url.AbsolutePath : "/";
			//Get the eligibility result
			if (accountMembership.Status == StatusEnum.Hot) {
				model.EligibilityResult = _eligibilityCompIntroLife.IsMemberEligible(accountMembership.Mdsid);
				var siteSettings = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);
				model.PhoneNumber = siteSettings[Templates.SiteSettings.Fields.Phone];
				model.SupportEmail = siteSettings[Templates.SiteSettings.Fields.Email];
			}
           
			return View("/Views/Neamb.Account/ComplimentaryLife/ComplimentaryLife.cshtml", model);
		}

		[HttpPost]
		public ActionResult ComplimentaryLife(string percentOK) {
            bool isToteBag = false;
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            var flagUrlParameter = Request.QueryString[ConstantsNeamb.FlagUrlParameter];
            if (!string.IsNullOrEmpty(flagUrlParameter))
            {
                Log.Debug($"Flag {flagUrlParameter}");
                if (flagUrlParameter == "1")
                {
                    Log.Debug($"Flag value 1");
                    isToteBag = true;
                }
            }
            if (_sessionAuthenticationManager.GetCellCode() == _globalConfigurationManager.CompLifeEmailCellCodeToteBag) {
                isToteBag = true;
            }
            var model = _complimentaryLifeManager.Save(RenderingContext.Current.Rendering,isToteBag);
            accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            model.EligibilityResult = _eligibilityCompIntroLife.IsMemberEligible(accountMembership.Mdsid);
            if (model.WasSaved) {
                Log.Info("Successfully finished Updating Complimentary Life form.", this);
                if (_complimentaryLifeWizardService.IsWizardStep()) {
                    if (string.IsNullOrEmpty(_wizardService.GetNextStepUrl())) {
                        return View("/Views/Neamb.Account/ComplimentaryLife/ComplimentaryLife.cshtml", model);
                    } else {
                        return Redirect(_wizardService.GetNextStepUrl());
                    }
                } else {
                    return Redirect($"{GetUrlComplimentaryPage(true)}");
                }
			} else {
				var siteSettings = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);
				model.PhoneNumber = siteSettings[Templates.SiteSettings.Fields.Phone];
				model.SupportEmail = siteSettings[Templates.SiteSettings.Fields.Email];
				Log.Info("Error while Updating Complimentary Life form.", this);
				return View("/Views/Neamb.Account/ComplimentaryLife/ComplimentaryLife.cshtml", model);
			}
		}

		[HttpPost]
		public ActionResult UpdateRemoveBeneficiary(BeneficiariesDTO beneficiariesDto) {
			switch (beneficiariesDto.Action) {
				case "Edit":
                    var url = string.Empty;
                    if (_complimentaryLifeWizardService.IsWizardStep()) {
                        url = string.Format("{0}?id={1}",
                            _complimentaryLifeWizardService.GetBeneficiariesEditCta(),
                            beneficiariesDto.SelectedBeneficiaryId);
                    } else {
                        url = string.Format("{0}?id={1}",
                            LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.EditBeneficiary)),
                            beneficiariesDto.SelectedBeneficiaryId);
                    }
					return Redirect(url);
				case "Remove":
                    _complimentaryLifeManager.Remove(beneficiariesDto.SelectedBeneficiaryId);
                    return Redirect(_complimentaryLifeWizardService.IsWizardStep()
                        ? LinkManager.GetItemUrl(Sitecore.Context.Item) 
                        : GetUrlComplimentaryPage());
                default:
					return Redirect(Request.UrlReferrer.AbsolutePath);
			}
		}
		#endregion

		#region Add Beneficiary
		public ActionResult AddBeneficiary() {
			var model = _complimentaryLifeManager.GetAddBeneficiaryModel(RenderingContext.Current.Rendering.Item);
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
              return View("/Views/Neamb.Account/ComplimentaryLife/AddBeneficiary.cshtml", model);
		}

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult AddBeneficiary(BeneficiaryDTO beneficiaryDto) {
			var model = _complimentaryLifeManager.SaveBeneficiary(beneficiaryDto, ViewData);
			if (model.WasSaved) {
                if (_complimentaryLifeWizardService.IsWizardStep()) {
                    return Redirect(_complimentaryLifeWizardService.GetParentStepUrl());
                }
				return Redirect(GetUrlComplimentaryPage());
			} else {
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                model.AccountStatus = accountMembership.Status;
                return View("/Views/Neamb.Account/ComplimentaryLife/AddBeneficiary.cshtml", model);
			}
		}
		#endregion

		#region Edit Beneficiary
		public ActionResult EditBeneficiary() {
			var beneficiaryId = Request.QueryString[Configuration.EditBeneficiaryIdParameterName];
			var model = _complimentaryLifeManager.GetEditBeneficiaryModel(RenderingContext.Current.Rendering, beneficiaryId);
            if (model == null) {
				Log.Warn($"No model for Id:{beneficiaryId} in profile draft. Redirecting to Complimentary Life Page.", this);
                if (_complimentaryLifeWizardService.IsWizardStep()) {
                    return Redirect(_complimentaryLifeWizardService.GetParentStepUrl());
                }
                return Redirect(GetUrlComplimentaryPage());
            } else {
				return View("/Views/Neamb.Account/ComplimentaryLife/EditBeneficiary.cshtml", model);
			}
		}

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult EditBeneficiary(string beneficiaryId, EditBeneficiaryDTO editBeneficiaryDto) {
			var model = _complimentaryLifeManager.EditBeneficiary(editBeneficiaryDto, ViewData);
			if (model.WasSaved) {
                if (_complimentaryLifeWizardService.IsWizardStep())
                {
                    return Redirect(_complimentaryLifeWizardService.GetParentStepUrl());
                }
                return Redirect(GetUrlComplimentaryPage());
			} else {
				return View("/Views/Neamb.Account/ComplimentaryLife/EditBeneficiary.cshtml", model);
			}
		}
        #endregion

        #region Private methods
        private string GetUrlComplimentaryPage(bool successParameter=false)
        {
            var urlComplimentaryPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.ComplimentaryLife));
            var urlFlagValue = _sessionManager.RetrieveFromSession<string>(ConstantsNeamb.FlagUrlParameterSession);
            if (!string.IsNullOrEmpty(urlFlagValue))
            {
                urlComplimentaryPage = $"{urlComplimentaryPage}?{ConstantsNeamb.FlagUrlParameter}={urlFlagValue}";
            }
            if (successParameter) {
                urlComplimentaryPage = (urlComplimentaryPage.Contains("?"))
                    ? $"{urlComplimentaryPage}&{Configuration.ComplimentaryLifeResultParameterName}={true.ToString()}"
                    : $"{urlComplimentaryPage}?{Configuration.ComplimentaryLifeResultParameterName}={true.ToString()}";
            }
            return urlComplimentaryPage;
        }
        #endregion
    }
}