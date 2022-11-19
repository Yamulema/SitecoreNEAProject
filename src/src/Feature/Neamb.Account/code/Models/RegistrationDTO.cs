using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models {
	public class RegistrationDTO : ProfileDTO, IPasswordDTO, IRenderingModel {
		public string SubmitText {
			get; set;
		}
		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		public string Password {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsConfirmPassword {
			get; set;
		}
		public string RequestedPage {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsPassword {
            get; set;
        }
		public string GtmActionPage { get; set; }
		public bool IsValid
		{
			get; set;
		}
		public bool IsSubmitted
		{
			get; set;
		}

		public new void Initialize(Rendering rendering) {
			base.Initialize(rendering);
			ProcessedSucessfully = false;
			ErrorsConfirmPassword = new List<ErrorStatusEnum>();
			ErrorsPassword = new List<ErrorStatusEnum>();
			IsValid = true;
			IsSubmitted = false;
        }

        public void PostInitialize()
        {
			ErrorsAddress = new List<ErrorStatusEnum>();
			ErrorsBirthDate = new List<ErrorStatusEnum>();
			ErrorsCity = new List<ErrorStatusEnum>();
			ErrorsEmail = new List<ErrorStatusEnum>();
			ErrorsFirstName = new List<ErrorStatusEnum>();
			ErrorsLastName = new List<ErrorStatusEnum>();
			ErrorsPhone = new List<ErrorStatusEnum>();
			ErrorsState = new List<ErrorStatusEnum>();
			ErrorsZip = new List<ErrorStatusEnum>();
			ErrorsCurrentPassword = new List<ErrorStatusEnum>();
			ErrorsConfirmPassword = new List<ErrorStatusEnum>();
			ErrorsPassword = new List<ErrorStatusEnum>();
			HasErrorUsername = false;
			HasErrorPassword = false;
			HasGeneralError = false;
			ProcessedSucessfully = false;
			HasDuplicateAccount = false;
			IsValid = true;
			IsSubmitted = false;
		}
	}
}