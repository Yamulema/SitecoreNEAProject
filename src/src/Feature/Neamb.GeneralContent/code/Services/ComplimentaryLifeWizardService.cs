using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using Neambc.Neamb.Feature.GeneralContent.Extensions;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.GeneralContent.Services
{
    [Service(typeof(IComplimentaryLifeWizardService))]
    public class ComplimentaryLifeWizardService : IComplimentaryLifeWizardService {
        private readonly IEligibilityCompIntroLife _eligibilityCompIntroLife;
        public ComplimentaryLifeWizardService(IEligibilityCompIntroLife eligibilityCompIntroLife)
        {
            _eligibilityCompIntroLife = eligibilityCompIntroLife;
        }
        public string GetBeneficiariesAddCta()
        {
            var currentItem = Sitecore.Context.Item;
            var children = currentItem.GetChildren();
            var targetItem = children
                .FirstOrDefault(x => x.HasRendering(Renderings.AddBeneficiaryFA005));
            return targetItem != null
                ? LinkManager.GetItemUrl(targetItem)
                : string.Empty;
        }
        public string GetParentStepUrl()
        {
            var currentItem = Sitecore.Context.Item;
            var targetItem = currentItem.Parent;
            return targetItem != null
                ? LinkManager.GetItemUrl(targetItem)
                : string.Empty;
        }
        public bool IsWizardStep()
        {
            return Sitecore.Context.Item.TemplateID == Templates.WizardStep.ID;
        }
        public bool IsWizardInnerStep()
        {
            return Sitecore.Context.Item.Parent.TemplateID == Templates.WizardStep.ID;
        }
        public string GetBeneficiariesEditCta()
        {
            var currentItem = Sitecore.Context.Item;
            var children = currentItem.GetChildren();
            var targetItem = children
                .FirstOrDefault(x => x.HasRendering(Renderings.EditBeneficiaryFA005));
            return targetItem != null
                ? LinkManager.GetItemUrl(targetItem)
                : string.Empty;
        }
        public string GetOnClickNextEvent() {
            return GetSubmitEvent();
        }
        public string GetOnClickEndEvent() {
            return GetSubmitEvent();
        }
        private string GetSubmitEvent() {
            var isElegible = _eligibilityCompIntroLife.IsCurrentSessionEligible();
            if(isElegible == Foundation.Eligibility.Model.EligibilityResultEnum.Eligible)
            {
                return $"document.forms[\"registration\"].submit()";
            }
            return string.Empty;
        }
    }
}