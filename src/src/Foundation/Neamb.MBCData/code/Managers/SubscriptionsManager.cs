using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Model;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Subscription = Neambc.Neamb.Foundation.MBCData.Model.Subscription;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    /// <summary>
    /// Serves as wrapper for ExactTarget Subscriptions API.
    /// </summary>
    [Service(typeof(ISubscriptionsManager))]
    public class SubscriptionsManager : ISubscriptionsManager
    {
        private readonly IExactTargetClient _exactTargetClient;

        public SubscriptionsManager(IExactTargetClient exactTargetClient)
        {
            _exactTargetClient = exactTargetClient;
        }

        public IEnumerable<ListSubscriber> GetAllSubscriptions(string mdsid)
        {
            var apiResults = _exactTargetClient.RetrieveAllSubscriptions(mdsid);
            return apiResults.Select(x => (ListSubscriber)x);
        }

        public bool UpdateSubscription(string mdsid, int listId, SubscriberStatus newStatus)
        {
            return _exactTargetClient.UpdateSubscriberList(mdsid, listId, newStatus);
        }
        public bool AddUpdateSubscription(string mdsid, int listId, string email, SubscriberStatus newStatus) {
            email = email.ToLower();
            mdsid = mdsid.ToLower();

            var newsletters = Sitecore.Context.Database.GetItem(Configuration.NewslettersId).Axes.GetDescendants().ToList();
            newsletters.AddRange(Sitecore.Context.Database.GetItem(Configuration.NewsletterCtaId).Axes.GetDescendants().ToList());

            //TODO: Review if we need to cache the line below.
            
            var newsletter = newsletters
                .Where(x => !string.IsNullOrEmpty(x.Fields[Templates.Newsletters.Fields.Id]?.Value) || !string.IsNullOrEmpty(x.Fields[Templates.NewsletterCTA.Fields.Code]?.Value))
                .ToList()
                .FirstOrDefault(x => x.Fields[Templates.Newsletters.Fields.Id].Value == listId.ToString());

            if (newStatus == SubscriberStatus.Active && newsletter == null)
            {
                Log.Error($"Newsletter with Id {listId} not found in {Configuration.NewslettersId}", this);
                return false;
            }

            var wasListUpdated = _exactTargetClient.AddUpdateSubscriberList(mdsid, listId, email, newStatus);
            if (!wasListUpdated)
            {
                Log.Error($"Operation for AddUpdateSubscriberList failed: mdsid:{mdsid} listId:{listId} email:{email} newStatus:{newStatus}", this);
                return false;
            }

            if (newStatus == SubscriberStatus.Unsubscribed && newsletter == null) {
                return true;
            }
            var wasDataExtensionUpdated = Configuration.EnableDataExtensionRequest
                ? AddUpdateDataExtension(new AddUpdateDataExtensionRequest() {
                    SubscriberKey = mdsid,
                    Email = email,
                    Newsletter = newsletter,
                    NewStatus = newStatus
                })
                : AddUpdateDataExtension(mdsid, newsletter, newStatus);

            if (wasDataExtensionUpdated) {
                Log.Debug($"Newsletter {listId} updated");
            } else {
                Log.Error($"Operation for AddUpdateDataExtension failed: mdsid:{mdsid} newStatus:{newStatus}", this);
            }
            return wasDataExtensionUpdated;
        }
        public Subscriber RetrieveSubscriber(string subscriberKey) {
            return _exactTargetClient.RetrieveSubscriber(subscriberKey).FirstOrDefault();
        }

        //[Obsolete("Use AddUpdateDataExtension with AddUpdateDataExtensionRequest as overload.")]
        //TODO: Remove after MBREQ-340 release to production.
        private bool AddUpdateDataExtension(string mdsid, Item newsletter, SubscriberStatus newStatus)
        {
            var source = "WEBSUB"; //This value is hardcoded for all user requests coming from the website.
            var vendor = newsletter.Fields[Templates.Newsletters.Fields.Vendor]?.Value;

            if (string.IsNullOrEmpty(vendor))
            {
                Log.Warn($"Invalid Newsletter Vendor Id value :{vendor}", this);
                return false;
            }

            var properties = new Dictionary<string, string>
            {
                { "INDIVIDUAL_MASTER_ID", mdsid },
                { "SOURCE", source }
            };

            switch (newStatus)
            {
                case SubscriberStatus.Active:
                    properties.Add("SUB_STATUS", "Y");
                    break;
                case SubscriberStatus.Unsubscribed:
                    properties.Add("SUB_STATUS", "N");
                    properties.Add("UNSUB_DATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, null);
            }

            return _exactTargetClient.AddUpdateDataExtension(vendor, properties);
        }
        private bool AddUpdateDataExtension(AddUpdateDataExtensionRequest request)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = Validator.TryValidateObject(request, new ValidationContext(request, null, null), results, true);
            if (!isValid) {
                var message = string.Join(Environment.NewLine, results.Select(x => x.ErrorMessage));
                Log.Error($"AddUpdateDataExtensionRequest object is not valid: {message}", this);
                return false;
            }

            var source = "WEBSUB"; //This value is hardcoded for all user requests coming from the website.
            var vendor = request.Newsletter.Fields[Templates.Newsletters.Fields.Vendor]?.Value;

            if (string.IsNullOrEmpty(vendor) && request.NewStatus == SubscriberStatus.Active)
            {
                Log.Warn($"Invalid Newsletter Vendor Id value :{vendor}", this);
                return false;
            }
            if (string.IsNullOrEmpty(vendor) && request.NewStatus == SubscriberStatus.Unsubscribed)
            {
                Log.Warn($"Invalid Newsletter Vendor Id value :{vendor}, skipping AddUpdateDataExtension", this);
                return true;
            }

            var properties = new Dictionary<string, string>
            {
                { "SubscriberKey", request.SubscriberKey },
                { "EMAILADDRESS", request.Email },
                { "SOURCE", source }
            };

            switch (request.NewStatus)
            {
                case SubscriberStatus.Active:
                    properties.Add("SUB_STATUS", "Y");
                    break;
                case SubscriberStatus.Unsubscribed:
                    properties.Add("SUB_STATUS", "N");
                    properties.Add("UNSUB_DATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.NewStatus), request.NewStatus, null);
            }

            return _exactTargetClient.AddUpdateDataExtension(vendor, properties);
        }
    }
}