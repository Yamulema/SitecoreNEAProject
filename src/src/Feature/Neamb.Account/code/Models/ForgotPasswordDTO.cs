using Neambc.Neamb.Foundation.Config.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class ForgotPasswordDTO : IRenderingModel
	{
		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[MaxLength(100, ErrorMessage = ConstantsNeamb.ValidationLength)]
        [EmailCompare(ErrorMessage = "Email Format")]
		[EmailAddress(ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		public string Email { get; set; }
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public List<ErrorStatusEnum> ErrorsEmail { get; set; }
		public bool HasTooltipEmail { get; set; }
		public bool HasGeneralError { get; set; }
		public bool ProcessedSucessfully { get; set; }
		public bool HasErrorEmailNotFound { get; set; }
		public string Emailconfirmation { get; set; }
		public string ReturnUrl { get; set; }
		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			ErrorsEmail = new List<ErrorStatusEnum>();
			HasTooltipEmail = false;
			HasGeneralError = false;
			ProcessedSucessfully = false;
			HasErrorEmailNotFound = false;			
		}
	}
}
