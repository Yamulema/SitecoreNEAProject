using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Configuration.Services.ActionReminder
{
    public interface IPageTypeService {
        PageType GetPageType(Item pageItem);
    }
}