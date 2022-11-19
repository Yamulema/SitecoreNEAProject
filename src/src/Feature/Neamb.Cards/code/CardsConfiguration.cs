using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Cards.Repositories.Interfaces;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Cards
{
    [Service(typeof(ICardsConfiguration))]
    public class CardsConfiguration : ICardsConfiguration
    {
        public CardsConfiguration()
        {
            StartSearchId = new ID(Sitecore.Configuration.Settings.GetSetting("StartSearchId"));
            MaxCardCount = int.Parse(Sitecore.Configuration.Settings.GetSetting("MaxCardCount"));
            MaxTabbedHeroItems = int.Parse(Sitecore.Configuration.Settings.GetSetting("MaxTabbedHeroItems"));
            TermsAndConditionsControlId = Sitecore.Configuration.Settings.GetSetting("TermsAndConditionsControlId");
        }
        public ID StartSearchId { get; }
        public int MaxCardCount { get; }
        public int MaxTabbedHeroItems { get; }
        public string TermsAndConditionsControlId { get; }
    }
}