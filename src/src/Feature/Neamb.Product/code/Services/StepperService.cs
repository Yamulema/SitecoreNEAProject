using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Services
{
    [Service(typeof(IStepperService))]
    public class StepperService : IStepperService
    {
        private readonly ISessionAuthenticationManager _sessionManager;
        private readonly ISubscriptionsManager _subscriptionsManager;

        public StepperService(ISubscriptionsManager subscriptionsManager, ISessionAuthenticationManager sessionManager)
        {
            _subscriptionsManager = subscriptionsManager;
            _sessionManager = sessionManager;
        }

        public void Run(Item item)
        {
            Log.Debug("Running StepperService");
            var steps = item.Template.BaseTemplates
                .Where(x => x.BaseTemplates
                    .Any(y => y.ID == Templates.Step.ID));

            var accountMembership = _sessionManager.GetAccountMembership();

            foreach (var step in steps)
            {
                if (step.ID == Templates.NewsletterStep.ID)
                {
                    if (item.Fields[Templates.NewsletterStep.Fields.Enabled].IsChecked())
                    {
                        Log.Debug("Running NewsletterStep");
                        var mdsid = accountMembership?.Mdsid?.PadLeft(9, '0');
                        var listId = int.TryParse(((LookupField)item.Fields[Templates.NewsletterStep.Fields.Newsletter])?
                            .TargetItem?.Fields[Templates.Newsletters.Fields.Id]?.Value, out var _listId)
                            ? _listId
                            : 0;
                        Log.Debug($"NewsletterStep calling AddUpdateSubscription mdsid:{mdsid} listId:{listId} email:{accountMembership?.Profile?.Email} SubscriberStatus:{SubscriberStatus.Active}");
                        var response = _subscriptionsManager.AddUpdateSubscription(mdsid, listId,
                            accountMembership?.Profile?.Email, SubscriberStatus.Active);
                        Log.Debug("NewsletterStep Finished");
                    }
                }
            }
        }
    }
}