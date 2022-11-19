using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class BeneficiaryDTO : IRenderingModel
    {
        private const string REGEXALPHANUMERIC = @"^[-.,' a-zA-Z0-9]{2,}$";
        private const string REGEXALPHANUMERICOneCharacter = @"^[-.,' a-zA-Z0-9]{1,}$";

        public string Id { get; set; }
        public BeneficiaryType Type { get; set; }

        [Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
        [RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
        [MaxLength(15, ErrorMessage = ConstantsNeamb.ValidationLength)]
        [MinLength(2, ErrorMessage = ConstantsNeamb.ValidationMinLength)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
        [RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
        [MaxLength(40, ErrorMessage = ConstantsNeamb.ValidationLength)]
        public string OtherEntityName { get; set; }
        
        [RegularExpression(REGEXALPHANUMERICOneCharacter, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
        [MaxLength(1, ErrorMessage = ConstantsNeamb.ValidationLength)]
        public string MiddleInitial { get; set; }

        [Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
        [RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
        [MaxLength(30, ErrorMessage = ConstantsNeamb.ValidationLength)]
        [MinLength(2, ErrorMessage = ConstantsNeamb.ValidationMinLength)]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = ConstantsNeamb.ValidationLength)]
        [EmailCompare(ErrorMessage = "Email Format")]
        [EmailAddress(ErrorMessage = "Email Format")]
        public string Email { get; set; }

        public MbcDbOption Relationship { get; set; }
        public int PayoutPercentage { get; set; }
        public int PayoutTotal { get; set; }
        public ErrorStatusEnum FirstNameErrorStatus { get; set; }
        public ErrorStatusEnum MiddleInitialErrorStatus { get; set; }
        public ErrorStatusEnum LastNameErrorStatus { get; set; }
        public ErrorStatusEnum EmailErrorStatus { get; set; }
        public ErrorStatusEnum PayoutPercentageErrorStatus { get; set; }
        public ErrorStatusEnum OtherEntityNameErrorStatus { get; set; }

        public StatusEnum AccountStatus { get; set; }
        
        public string Action { get; set; }
        public string SelectedType { get; set; }
        public BackCTA BackCta { get; set; }

        public string DatasourceId { get; set; }
        public bool WasSaved { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public MembershipType MembershipType { get; set; }
        public string OnClickEvent { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
        }
        public void Initialize(Item item)
        {
            Item = item;
        }

        public BeneficiaryDTO (Rendering rendering)
        {
            Initialize(rendering);
        }
        public BeneficiaryDTO(Item item)
        {
            Item = item;
        }
        public BeneficiaryDTO() 
        {

        }
    }
}