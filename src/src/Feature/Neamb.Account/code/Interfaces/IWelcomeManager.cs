using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Account.Interfaces
{
    public interface IWelcomeManager
    {
        MemberWelcome MemberWelcomeModel(Item datasource);
        MemberWelcome MemberWelcomeRegister(MemberWelcome memberWelcome);
        MemberVerification MemberVerificationModel(Item datasource);
        void VerifyZip(ref MemberVerification model, ModelStateDictionary viewDataModelState);
        AuthenticationResultEnum VerifyPassword(ref MemberVerification model, ViewDataDictionary viewData);
        MembershipCard MembershipCardRegister(MembershipCard membershipCard);
        AuthenticationResultEnum VerifyPassword(ref MembershipCardLogin model, ViewDataDictionary viewData);
    }
}