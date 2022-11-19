using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class ResetPasswordDTO : IRenderingModel
	{
		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[MaxLength(100, ErrorMessage = ConstantsNeamb.ValidationLength)]
        [EmailCompare(ErrorMessage = "Email Format")]
		[EmailAddress(ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public bool HasGeneralError { get; set; }
		public bool HasTooltipPassword { get; set; }
		public bool ProcessedSucessfully { get; set; }
		public string ErrorMessageNotFound { get; set; }
		public PasswordDTO PasswordData { get; set; }
		public bool HasTokenValid { get; set; }
		public string Username { get; set; }
		public string Token { get; set; }
        public string RedirectPage { get; set; }


        public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			//ErrorsEmail = new List<ErrorStatusEnum>();
			//HasTooltipEmail = false;
			HasTooltipPassword = false;
			HasGeneralError = false;
			ProcessedSucessfully = false;
			PasswordData= new PasswordDTO();
			PasswordData.Initialize(rendering);
			HasTokenValid = false;
		}
	}
}
