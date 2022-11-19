using Neambc.Neamb.Foundation.Config.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Neamb.Foundation.MBCData.Model.Login;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class ProfileBasicDTO: IDateBirthDTO
	{
        private const string REGEXALPHANUMERIC = @"^[-.,' a-zA-Z0-9]{2,}$";

        [Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(15, ErrorMessage = ConstantsNeamb.ValidationLength)]
		[MinLength(2, ErrorMessage = ConstantsNeamb.ValidationMinLength)]
		public string FirstName { get; set; }

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(30, ErrorMessage = ConstantsNeamb.ValidationLength)]
		[MinLength(2, ErrorMessage = ConstantsNeamb.ValidationMinLength)]
		public string LastName { get; set; }

		public string BirthDate { get; set; }

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[MaxLength(100, ErrorMessage = ConstantsNeamb.ValidationLength)]
		[EmailAddress(ErrorMessage = "Email Format")]
        [EmailCompare(ErrorMessage = "Email Format")]
        public string Email { get; set; }
		public bool HasGeneralError { get; set; }

		public List<ErrorStatusEnum> ErrorsFirstName { get; set; }
		public List<ErrorStatusEnum> ErrorsLastName { get; set; }
		public List<ErrorStatusEnum> ErrorsBirthDate { get; set; }
		public List<ErrorStatusEnum> ErrorsEmail { get; set; }
		public string Month { get; set; }
		public string Day { get; set; }
		public string Year { get; set; }
		public bool HasTooltipFirstName { get; set; }
		public bool HasTooltipLastName { get; set; }
		public bool HasTooltipBirthDate { get; set; }
		public bool HasTooltipEmail { get; set; }
		public bool HasTooltipPassword { get; set; }
		public bool ProcessedSucessfully { get; set; }
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public string ErrorMessageEmailInUse { get; set; }
		public string Webuserid { get; set; }
		public bool HasDuplicateAccount { get; set; }
		public List<LoginRegistration> Registrations { get; set; }
		public string NewMdsid { get; set; }
	}
}
