using System;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Repositories
{
    public interface ISweepstakesRepository {
        void SetPropertiesSweepstake(ref SweepstakesDTO sweepstakesDto, Item renderingItem);
        bool SendEmail(Item contextItem, AccountMembership accountMembership);
        void SetComponentId(ref SweepstakesDTO sweepstakesDto, Guid renderingId);
        void SetActionClickAuthentication(ref SweepstakesDTO sweepstakesDto);
    }
}