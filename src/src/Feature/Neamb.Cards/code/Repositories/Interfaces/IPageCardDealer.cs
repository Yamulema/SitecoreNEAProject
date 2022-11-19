using System.Collections.Generic;
using Neambc.Neamb.Feature.Cards.Models;
using Sitecore.Data.Items;
using Sitecore.Web.UI.HtmlControls;

namespace Neambc.Neamb.Feature.Cards.Repositories.Interfaces
{
    public interface IPageCardDealer
    {
        IEnumerable<T> GetPageCards<T>(Item page, Item datasource) where T : PageCard;
    }
}