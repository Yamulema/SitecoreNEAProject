using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Neambc.Neamb.Feature.GeneralContent.Services
{
    [Service(typeof(IWizardService))]
    public class WizardService : IWizardService
    {
        private readonly ISessionService _sessionService;
        private readonly ISessionAuthenticationManager _sessionManager;
        private readonly IWizardEventHandler _wizardEventHandler;
        public WizardService(ISessionService sessionService, ISessionAuthenticationManager sessionManager, IWizardEventHandler wizardEventHandler) {
            _sessionService = sessionService;
            _sessionManager = sessionManager;
            _wizardEventHandler = wizardEventHandler;
        }
        public Wizard GetWizard(Item datasource, HttpRequestBase request) {
            _sessionService.Set(Configuration.ReturnUrlArg, request.QueryString[Configuration.ReturnUrlArg]);
            var steps = (MultilistField)datasource.Fields[Templates.Wizard.Fields.Steps];
            var startButtonPath = steps.Count > 0
                ? LinkManager.GetItemUrl(steps.GetItems()[0])
                : string.Empty;

            return new Wizard()
            {
                Datasource = datasource,
                StartButton = new WizardButton()
                {
                    Label = datasource.Fields[Templates.Wizard.Fields.StartButtonText].Value,
                    Target = startButtonPath
                },
                SkipButton = new WizardButton()
                {
                    Label = datasource.Fields[Templates.Wizard.Fields.SkipButtonText].Value,
                    Target = _sessionService.Get(Configuration.ReturnUrlArg)?.ToString()
                },
                Header = GetHeader(datasource),
                IsAnonymous = IsAnonymous(),
                IsExperienceEditor = Sitecore.Context.PageMode.IsExperienceEditor
            };
        }
        private bool IsAnonymous() {
            return _sessionManager.GetAccountMembership()?.Status != StatusEnum.Hot;
        }
        private WizardHeader GetHeader(Item datasource)
        {
            return new WizardHeader()
            {
                LogoUrl = datasource.ImageUrl(Templates.Wizard.Fields.Logo)
            };
        }
        public Step GetStep(Item datasource) {
            return new Step()
            {
                Header = IsInnerStep(datasource)
                    ? GetNavigationHeader(datasource.Parent)
                    : GetNavigationHeader(datasource),
                IsAnonymous = IsAnonymous(),
                IsExperienceEditor = Sitecore.Context.PageMode.IsExperienceEditor,
                Datasource = IsInnerStep(datasource) 
                    ? datasource.Parent.Parent //Jumps two levels if step is a inner step.
                    : datasource.Parent,
                IsInnerStep = IsInnerStep(datasource)
            };
        }
        private WizardHeader GetNavigationHeader(Item datasource)
        {
            var parent = datasource.Parent;
            var steps = (MultilistField)parent.Fields[Templates.Wizard.Fields.Steps];
            var currentStep = Array.IndexOf(steps.TargetIDs, datasource.ID);
            return new WizardHeader()
            {
                LogoUrl = parent.ImageUrl(Templates.Wizard.Fields.Logo),
                Back = GetBackButton(parent, steps, currentStep),
                Next = GetNextButton(parent, steps, currentStep),
                End = GetEndButton(parent, steps, currentStep),
                StepText = GetStepText(parent, steps, currentStep)
            };
        }
        private string GetStepText(Item parent, MultilistField steps, int currentStep) {
            return string.Format(parent.Fields[Templates.Wizard.Fields.StepText].Value, currentStep + 1, steps.Count);
        }
        private WizardButton GetEndButton(Item datasource, MultilistField steps, int currentStep)
        {
            // Checks if current step is the last step.
            if (currentStep < steps.Count - 1)
            {
                return null;
            }
            return new WizardButton()
            {
                Label = datasource.Fields[Templates.Wizard.Fields.End].Value,
                Target = _sessionService.Get(Configuration.ReturnUrlArg)?.ToString(),
                OnClickEvent = _wizardEventHandler.GetOnClickEndEvent()
            };
        }

        private WizardButton GetBackButton(Item datasource, MultilistField steps, int currentStep)
        {
            // Checks if current step is not the first step.
            if (currentStep < 1)
            {
                return null;
            }
            return new WizardButton()
            {
                Label = datasource.Fields[Templates.Wizard.Fields.Back].Value,
                Target = LinkManager.GetItemUrl(steps.GetItems()[currentStep - 1])
            };
        }
        private WizardButton GetNextButton(Item datasource, MultilistField steps, int currentStep)
        {
            // Checks if current step is not the last step.
            if (currentStep >= steps.Count - 1)
            {
                return null;
            }
            return new WizardButton()
            {
                Label = datasource.Fields[Templates.Wizard.Fields.Next].Value,
                Target = LinkManager.GetItemUrl(steps.GetItems()[currentStep + 1]),
                OnClickEvent = _wizardEventHandler.GetOnClickNextEvent()
            };
        }
        /// <summary>
        /// Checks if step is inner step.
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        private bool IsInnerStep(Item datasource) {
            return datasource.Parent.TemplateID == Templates.WizardStep.ID;
        }

        public string GetNextStepUrl() {
            var step = GetStep(Sitecore.Context.Item);
            return step?.Header?.Next?.Target;
        }
    }
}