using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Membership.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neambc.Neamb.Feature.GeneralContent.Repositories
{
    public interface IContactUsRepository
    {
        void SubmitContactUs(ref ContactUsDTO model, ViewDataDictionary viewData);
    }
}