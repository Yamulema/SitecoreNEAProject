using System.Collections.Generic;
using Neambc.Neamb.Feature.GeneralContent.Enums;
using Neambc.Neamb.Feature.GeneralContent.Models;

namespace Neambc.Neamb.Feature.GeneralContent.Interfaces
{
    public interface IRateQuotationManager
    {
        List<Plan> GetPlanQuotes(string state, string zip, string age);
        QuoteStatus Validate(string state, string zip);
        StateStatus Validate(string state);
    }
}