using System.Linq;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Services
{
    [Service(typeof(INewsletterService))]
    public class NewsletterService : INewsletterService
    {
        private readonly ISessionAuthenticationManager _sessionManager;
        private readonly ISubscriptionsManager _subscriptionsManager;
        private readonly ISearchUserNameService _searchUserNameService;

        public NewsletterService(ISessionAuthenticationManager sessionManager, ISubscriptionsManager subscriptionsManager,
            ISearchUserNameService searchUserNameService) {
            _sessionManager = sessionManager;
            _subscriptionsManager = subscriptionsManager;
            _searchUserNameService = searchUserNameService;
        }

        public NewsletterCTADTO GetNewsletter(Item item, bool isSubscribed) {
            var subscriberKey = _sessionManager.GetAccountMembership()?.Mdsid;
            var newsletterId = int.TryParse(Sitecore.Context.Database.GetItem(item[Templates.NewsletterCTA.Fields.Code])[Templates.Newsletters.Fields.Id], out var code) ? code : 0;
            var isPublic = ((CheckboxField)item.Fields[Templates.NewsletterCTA.Fields.PublicNewsletter])?.Checked ?? false;
            var account = _sessionManager.GetAccountMembership();
            isPublic |= account.Status == StatusEnum.WarmCold;
            //Anonymous User.
            if (string.IsNullOrEmpty(subscriberKey) || account.Status == StatusEnum.WarmCold) {

                return new NewsletterCTADTO()
                {
                    Item = item,
                    NewsletterId = newsletterId,
                    IsSubscribed = isSubscribed,
                    SocialShare = new SocialShareModel(item),
                    IsAnonymous = true,
                    IsPublic = isPublic
                };
            } else {
                return new NewsletterCTADTO()
                {
                    Item = item,
                    NewsletterId = newsletterId,
                    IsSubscribed = IsSubscribed(subscriberKey, newsletterId),
                    SocialShare = new SocialShareModel(item),
                    IsAnonymous = false,
                    IsPublic = isPublic
                };
            }
        }
        public bool UpdateSubscription(int newsletterId, string email, SubscriberStatus newStatus) {
            Sitecore.Diagnostics.Log.Debug($"UpdateSubscription newsletterId: {newsletterId} email: {email}");
            var account = _sessionManager.GetAccountMembership();
            Sitecore.Diagnostics.Log.Debug($"account.Username : {account.Username}");
            var subscriberKey = account?.Mdsid;
            Sitecore.Diagnostics.Log.Debug($"subscriberKey: {subscriberKey}");
            Sitecore.Diagnostics.Log.Debug($"account.Status: {account.Status}");
            if (account.Status == StatusEnum.WarmCold) {
                subscriberKey = email;
            }
            else if (string.IsNullOrEmpty(email) && (account.Status == StatusEnum.Cold || account.Status == StatusEnum.Unknown)) {
                Sitecore.Diagnostics.Log.Debug($"email: {email}");
                return false;
            }
            else if (string.IsNullOrEmpty(subscriberKey)) {
                //Check Mdsid for email/
                var memberData = _searchUserNameService.SearchUserName(email);
                if (memberData?.Data != null && memberData.Data.MdsId != 0)
                    subscriberKey = memberData.Data.MdsId.ToString().PadLeft(9, '0');
                else
                    subscriberKey = email;
            } else {
                email = account.Username;
            }
            
            Sitecore.Diagnostics.Log.Debug($"subscriberKey: {subscriberKey} email: {email}");
            return _subscriptionsManager.AddUpdateSubscription(subscriberKey, newsletterId, email, newStatus);
        }
        private bool IsSubscribed(string subscriberKey, int newsletterId)
        {
            return _subscriptionsManager.GetAllSubscriptions(subscriberKey)
                .Any(x=>x.ListID == newsletterId && x.Status == SubscriberStatus.Active);
        }
    }
}