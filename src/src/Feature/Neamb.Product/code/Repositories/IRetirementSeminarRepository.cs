using System;
using Neambc.Neamb.Feature.Product.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Repositories
{
    public interface IRetirementSeminarRepository
    {
        void SetPropertiesRetirementSeminar(ref RetirementSeminarDTO retirementSeminarDto, Item renderingItem, Guid renderingId, string componentId);
        RetirementSeminarResponse ExecuteRegistrationRetirementSeminar(RetirementSeminarDTO retirementSeminarDto);
        void SetActionClickAuthentication(ref SweepstakesDTO sweepstakesDto);
    }
}