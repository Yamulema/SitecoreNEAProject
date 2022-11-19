using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Interfaces;

namespace Neambc.Neamb.Feature.Account
{
    public class BeneficiaryValidator
    {
        public ErrorStatusEnum CheckPayoutErrors(BeneficiaryDTO beneficiary, ViewDataDictionary viewData)
        {
            if (beneficiary == null)
            {
                throw new ArgumentNullException(nameof(beneficiary));
            }
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }
            return GetPayoutErrorStatus(beneficiary, viewData);   
        }

        public ErrorStatusEnum CheckFirstNameErrors(BeneficiaryDTO beneficiary, ViewDataDictionary viewData)
        {
            var ret = ErrorStatusEnum.None;
            if (beneficiary == null)
            {
                throw new ArgumentNullException(nameof(beneficiary));
            }
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }
            if (beneficiary.Type == BeneficiaryType.NamedIndividual)
            {
                ret |= (beneficiary.FirstNameErrorStatus = ValidationFieldHelper.GetErrorStatus(
                    nameof(beneficiary.FirstName), viewData, true, true, true
                ));
            }
            return ret;
        }

        public ErrorStatusEnum CheckMiddleInitialErrors(BeneficiaryDTO beneficiary, ViewDataDictionary viewData)
        {
            var ret = ErrorStatusEnum.None;
            if (beneficiary == null)
            {
                throw new ArgumentNullException(nameof(beneficiary));
            }
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }
            if (beneficiary.Type == BeneficiaryType.NamedIndividual)
            {
                ret |= (beneficiary.MiddleInitialErrorStatus = ValidationFieldHelper.GetErrorStatus(
                    nameof(beneficiary.MiddleInitial), viewData, true, true, true
                ));
            }
            return ret;
        }

        public ErrorStatusEnum CheckLastNameErrors(BeneficiaryDTO beneficiary, ViewDataDictionary viewData)
        {
            var ret = ErrorStatusEnum.None;
            if (beneficiary == null)
            {
                throw new ArgumentNullException(nameof(beneficiary));
            }
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }
            if (beneficiary.Type == BeneficiaryType.NamedIndividual)
            {
                ret |= (beneficiary.LastNameErrorStatus = ValidationFieldHelper.GetErrorStatus(
                    nameof(beneficiary.LastName), viewData, true, true, true
                ));
            }
            return ret;
        }

        public ErrorStatusEnum CheckOtherEntityErrors(BeneficiaryDTO beneficiary, ViewDataDictionary viewData)
        {
            var ret = ErrorStatusEnum.None;
            if (beneficiary == null)
            {
                throw new ArgumentNullException(nameof(beneficiary));
            }
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }
            ret |= (beneficiary.OtherEntityNameErrorStatus = ValidationFieldHelper.GetErrorStatus(
                    nameof(beneficiary.OtherEntityName), viewData, true, true, true
                ));
            return ret;
        }

        public ErrorStatusEnum CheckEmailErrors(BeneficiaryDTO beneficiary, ViewDataDictionary viewData, IEmailValidationManager emailValidator, IAccountRepository repository)
        {
            var ret = ErrorStatusEnum.None;
            if (beneficiary == null)
            {
                throw new ArgumentNullException(nameof(beneficiary));
            }
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }
            if (emailValidator == null)
            {
                throw new ArgumentNullException(nameof(emailValidator));
            }

            ret |= (beneficiary.EmailErrorStatus =
                ValidationFieldHelper.GetErrorStatus(nameof(beneficiary.Email), viewData, true, true, true));

            if (!string.IsNullOrEmpty(beneficiary.Email))
            {
                bool hasDomainErrors = repository.HasEmailDomainValidationErrors(beneficiary.Email);
                if (hasDomainErrors) ret |= beneficiary.EmailErrorStatus |= ErrorStatusEnum.InvalidValue;
            }
            return ret;
        }

        public ErrorStatusEnum GetPayoutErrorStatus(BeneficiaryDTO beneficiary, ViewDataDictionary viewData)
        {
            var ret = ValidationFieldHelper.GetErrorStatus(
                nameof(beneficiary.PayoutPercentage),
                viewData, true, true, true);

            if (beneficiary.PayoutPercentage <= 0 || beneficiary.PayoutPercentage > 100)
            {
                ret |= ErrorStatusEnum.InvalidValue;
            }

            //Checks if is a 3 digit number.
            if (beneficiary.PayoutPercentage >= 1000)
            {
                ret |= ErrorStatusEnum.Length;
            }
            return ret;
        }
    }

}