using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.ComponentModel.DataAnnotations;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class RetrieveUserNameModel : IRenderingModel
    {
		private const string REGEXALPHANUMERIC = @"^[-.,' a-zA-Z0-9]{2,}$";

		[Required]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = "Special characters not allowed")]
		[MaxLength(15, ErrorMessage = "Error Length")]
		public string FirstName { get; set; }
        
		[Required]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = "Special characters not allowed")]
		[MaxLength(30, ErrorMessage = "Error Length")]
		public string LastName { get; set; }
		
		[RegularExpression(@"[0-9]+", ErrorMessage = "Special characters not allowed")]
		[MaxLength(5, ErrorMessage = "Error Length")]
		public string ZipCode { get; set; }
        
		[Required]
		[BirthDateTimeCompareAttribute(ErrorMessage = "DateRangeError")]
		public string DateOfBirth { get; set; }

        public string UserNameRetrieved { get; set; }
        public bool HasErrorFirstName { get; set; }
        public bool HasErrorFirstNameInvalidCharacters { get; set; }
        public bool HasErrorLastName { get; set; }
		public bool HasErrorZipcode { get; set; }
		public bool HasErrorLastNameInvalidCharacters { get; set; }
		public bool HasErrorFirstNameLength { get; set; }
		public bool HasErrorLastNameLength { get; set; }
		public bool HasErrorZipcodeLength { get; set; }
		public bool HasErrorDateOfBirthAge { get; set; }
		public bool HasErrorBirthDate { get; set; }

		public Item Item { get; set; }
        public bool Submitted { get; set; }
        public void Initialize(Rendering rendering)
        {
            Item = rendering.Item;
            Submitted = false;
        }        
    }
}