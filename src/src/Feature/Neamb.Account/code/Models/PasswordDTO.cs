using Neambc.Neamb.Foundation.Config.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Foundation.Configuration.Extensions;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class PasswordDTO : IPasswordDTO,IRenderingModel
	{
		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		public string Password { get; set; }

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		public string ConfirmPassword { get; set; }
		public List<ErrorStatusEnum> ErrorsPassword { get; set; }
		public List<ErrorStatusEnum> ErrorsConfirmPassword { get; set; }
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }

		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			ErrorsPassword = new List<ErrorStatusEnum>();
			ErrorsConfirmPassword = new List<ErrorStatusEnum>();
		}
	}
}
