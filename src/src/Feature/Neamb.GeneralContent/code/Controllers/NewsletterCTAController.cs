using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Subscription = Neambc.Neamb.Foundation.Analytics.Gtm.Subscription;


namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class NewsletterCTAController : BaseController
    {
        private readonly ISessionAuthenticationManager _sessionManager;
        private readonly INewsletterService _newsletterService;
        private readonly IGtmService _gtmService;
        public NewsletterCTAController(ISessionAuthenticationManager sessionManager, INewsletterService newsletterService, IGtmService gtmService)
        {
            _sessionManager = sessionManager;
            _newsletterService = newsletterService;
            _gtmService = gtmService;
        }
        public ActionResult NewsletterCta()
        {
            var subscribeResult = Request.QueryString[Configuration.SubscribeResult];
            bool.TryParse(subscribeResult, out var isSubscribed);
            var datasource = RenderingContext.Current.Rendering.Item ?? PageContext.Current.Item;
            var model = _newsletterService.GetNewsletter(datasource, isSubscribed);
            model.Initialize(RenderingContext.Current.Rendering);

            CheckboxField Publicnewsletter = model.Item.Fields[Templates.NewsletterCTA.Fields.PublicNewsletter];
            var accActionItem = model.Item.Fields[Templates.NewsletterCTA.Fields.Code]?.Value;
            var accItem = Sitecore.Context.Database.Items.GetItem(accActionItem);
            var NewsletterName = accItem.Name;

            var status = _sessionManager.GetAccountMembership().Status;
          
            if ( Publicnewsletter.Checked || status == StatusEnum.Hot) { 

                model.OnSubscribeEvent = _gtmService.GetGtmEvent(new Subscription()
                {
                    Event = "account",
                    AccountSection = "settings & subscriptions",
                    AccountAction = NewsletterName,
                    CtaText = model.Item.Fields[Templates.NewsletterCTA.Fields.Subscribe]?.Value
                });
            model.OnUnSubscribeEvent = _gtmService.GetGtmEvent(new Subscription()
            {
                Event = "account",
                AccountSection = "settings & subscriptions",
                AccountAction = NewsletterName,
                CtaText = model.Item.Fields[Templates.NewsletterCTA.Fields.Unsubscribe]?.Value
            });
        }else {
         model.OnSubscribeEvent= "";
         model.OnUnSubscribeEvent= "";
    }

            return View("/Views/Neamb.GeneralContent/Renderings/NewsletterCta.cshtml", model);
        }
        public ActionResult Subscribe(int newsletterId, string newsLetterItemId, string email, bool subscribe, string redirectId)
        {
            var contextItem = Sitecore.Context.Database.GetItem(newsLetterItemId);
            var redirectItem = Sitecore.Context.Database.GetItem(redirectId);
            if (contextItem == null || redirectItem == null)
            {
                return Redirect("/");
            }
            var status = _sessionManager.GetAccountMembership().Status;
            CheckboxField publicNewsletter = contextItem.Fields[Templates.NewsletterCTA.Fields.PublicNewsletter];
            if (publicNewsletter.Checked || status == StatusEnum.Hot)
            {
                var newStatus = subscribe ? SubscriberStatus.Active : SubscriberStatus.Unsubscribed;
                var result = _newsletterService.UpdateSubscription(newsletterId, email, newStatus);

                return Redirect($"{LinkManager.GetItemUrl(redirectItem)}?{Configuration.SubscribeResult}={result.ToString()}");
            }
            else
            {
                return Redirect(Configuration.LoginPagePath);
            }
        }
    }
}