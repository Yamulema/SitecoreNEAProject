using Neambc.Neamb.Foundation.Config.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Foundation.Configuration.Extensions;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class ForgotUsernameDTO : IRenderingModel, IUsernameBasicDTO, IDateBirthDTO
	{
		private const string REGEXALPHANUMERIC = @"^[-.,' a-zA-Z0-9]{2,}$";

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(15, ErrorMessage = ConstantsNeamb.ValidationLength)]
		public string FirstName { get; set; }

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(30, ErrorMessage = ConstantsNeamb.ValidationLength)]
		public string LastName { get; set; }

		public string BirthDate { get; set; }
		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(@"[0-9]+", ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(5, ErrorMessage = ConstantsNeamb.ValidationLength)]
		public string Zip { get; set; }

		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public bool HasTooltipFirstName { get; set; }
		public bool HasTooltipLastName { get; set; }
		public bool HasTooltipBirthDate { get; set; }
		public bool HasTooltipZip { get; set; }
		public List<ErrorStatusEnum> ErrorsFirstName { get; set; }
		public List<ErrorStatusEnum> ErrorsLastName { get; set; }
		public List<ErrorStatusEnum> ErrorsBirthDate { get; set; }
		public List<ErrorStatusEnum> ErrorsZip { get; set; }
		public bool HasGeneralError { get; set; }
		public bool ProcessedSucessfully { get; set; }
		public string Month { get; set; }
		public string Day { get; set; }
		public string Year { get; set; }
		public string Username { get; set; }
		public bool HasErrorUserName { get; set; }
		public string PathResetPassword { get; set; }
		public string TextResetPassword { get; set; }
		public string Zipconfirmation { get; set; }

		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			HasTooltipFirstName = false;
			HasTooltipLastName = false;
			HasTooltipBirthDate = false;
			HasTooltipZip = false;
			ErrorsBirthDate = new List<ErrorStatusEnum>();
			ErrorsFirstName = new List<ErrorStatusEnum>();
			ErrorsLastName = new List<ErrorStatusEnum>();
			ErrorsZip = new List<ErrorStatusEnum>();
			HasGeneralError = false;
			ProcessedSucessfully = false;
			HasErrorUserName = false;
		}
	}
}
