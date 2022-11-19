using Neambc.Neamb.Foundation.Config.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Foundation.Configuration.Extensions;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class ZipCodeVerificationDTO : IRenderingModel
	{
		public string FullName { get; set; }

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(@"[0-9]+", ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(5, ErrorMessage = ConstantsNeamb.ValidationLength)]
		public string ZipCode { get; set; }

		public List<ErrorStatusEnum> ErrorsZipCode { get; set; }
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public Item PageItem { get; set; }
		public int Attempts { get; set; }
		public string ButtonText { get; set; }
		public bool HasTooltip { get; set; }

		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			ErrorsZipCode = new List<ErrorStatusEnum>();
			Attempts = 0;
		}
	}
}