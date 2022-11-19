using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Foundation.Configuration.Enums;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class MembershipCardLogin : IRenderingModel
    {
        public Item Item { get; set; }
        public WelcomeStatus Status { get; set; }
        public string Imsid { get; set; }
        public string Password { get; set; }
        public string Salutation { get; set; }
        public string Instructions { get; set; }
        public string NotYou { get; set; }
        public WelcomeErrorStatus ErrorStatus { get; set; }
        public WelcomeAction Action { get; set; }
        public bool WasProcessed { get; set; }
        public int Attempts { get; set; }
        public string NotYouLinkUrl { get; set; }
        public MembershipCardLogin()
        {
        }

        public MembershipCardLogin(Item datasource)
        {
            Item = datasource;
        }
        public void Initialize(Rendering rendering)
        {
            Item = rendering.Item;
        }
    }
}