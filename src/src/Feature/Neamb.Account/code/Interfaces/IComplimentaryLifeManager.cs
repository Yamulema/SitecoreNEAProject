using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Interfaces
{
    public interface IComplimentaryLifeManager
    {
        ComplimentaryLifeDTO ComplimentaryLifeModel(Rendering rendering, AccountMembership accountMembership);
        BeneficiaryDTO GetAddBeneficiaryModel(Item datasource);
        BeneficiaryDTO SaveBeneficiary(BeneficiaryDTO beneficiaryDto, ViewDataDictionary viewData);
        EditBeneficiaryDTO EditBeneficiary(BeneficiaryDTO beneficiaryDto, ViewDataDictionary viewData);
        EditBeneficiaryDTO GetEditBeneficiaryModel(Rendering rendering, string beneficiaryId);

        bool Remove(string beneficiaryId);
        ComplimentaryLifeDTO Save(Rendering rendering, bool isToteBag);
    }
}