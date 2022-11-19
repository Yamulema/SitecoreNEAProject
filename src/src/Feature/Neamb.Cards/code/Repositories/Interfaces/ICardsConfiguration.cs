using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Cards.Repositories.Interfaces
{
    public interface ICardsConfiguration
    {
        ID StartSearchId { get; }
        int MaxCardCount { get; }
        int MaxTabbedHeroItems { get; }
        string TermsAndConditionsControlId { get; }
    }
}
