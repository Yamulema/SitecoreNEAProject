using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Cards.Repositories.Enums;

namespace Neambc.Neamb.Feature.Cards.Repositories.Interfaces
{
    public interface IPageCardDealerFactory
    {
        IPageCardDealer GetCardDealer(PageCardDealerType Type);
    }
}