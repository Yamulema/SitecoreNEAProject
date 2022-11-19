using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class EditBeneficiaryDTO : BeneficiaryDTO
    {
        public EditBeneficiaryDTO(Rendering rendering)
        {
            Initialize(rendering);
        }
        public EditBeneficiaryDTO(Item datasource)
        {
            Initialize(datasource);
        }

        public EditBeneficiaryDTO()
        {

        }

        public EditBeneficiaryDTO(BeneficiaryDTO beneficiaryDto)
        {
            DatasourceId = beneficiaryDto.DatasourceId;
            Email = beneficiaryDto.Email;
            EmailErrorStatus = beneficiaryDto.EmailErrorStatus;
            FirstName = beneficiaryDto.FirstName;
            FirstNameErrorStatus = beneficiaryDto.FirstNameErrorStatus;
            Id = beneficiaryDto.Id;
            Item = beneficiaryDto.Item;
            LastName = beneficiaryDto.LastName;
            LastNameErrorStatus = beneficiaryDto.LastNameErrorStatus;
            MiddleInitial = beneficiaryDto.MiddleInitial;
            Relationship = beneficiaryDto.Relationship;
            MiddleInitialErrorStatus = beneficiaryDto.MiddleInitialErrorStatus;
            OtherEntityName = beneficiaryDto.OtherEntityName;
            OtherEntityNameErrorStatus = beneficiaryDto.OtherEntityNameErrorStatus;
            PayoutPercentage = beneficiaryDto.PayoutPercentage;
            PayoutPercentageErrorStatus = beneficiaryDto.PayoutPercentageErrorStatus;
            SelectedType = beneficiaryDto.SelectedType;
            Type = beneficiaryDto.Type;
            WasSaved = beneficiaryDto.WasSaved;
        }
    }
}