using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Interfaces
{
    public interface INewsletterService {
        NewsletterCTADTO GetNewsletter(Item item, bool isSubscribed);
        bool UpdateSubscription(
            int newsletterId,
            string email,
            SubscriberStatus newStatus
        );
    }
}