using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Foundation.Configuration.Enums;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class MembershipCard : IRenderingModel
    {
        public Item Item { get; set; }
        public WelcomeStatus Status { get; set; }
        public string Imsid { get; set; }
        public string Mdsid { get; set; }
        public WelcomeErrorStatus ErrorStatus { get; set; }

        public MembershipCard()
        {
        }

        public MembershipCard(Item datasource)
        {
            Item = datasource;
        }
        public void Initialize(Rendering rendering)
        {
            Item = rendering.Item;
        }
    }
}