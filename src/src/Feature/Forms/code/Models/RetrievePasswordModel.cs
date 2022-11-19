using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.ComponentModel.DataAnnotations;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class RetrievePasswordModel : IRenderingModel
    {
        [Required]
		[MaxLength(100, ErrorMessage = "Error Length")]
		public string UserName { get; set; }
        public bool HasErrorEmail { get; set; }
        public bool SendEmail { get; set; }

		public Item Item { get; set; }
        public bool Submitted { get; set; }
        public bool IsUsernameValid { get; set; }
        public string ItemId { get; set; }
        public void Initialize(Rendering rendering)
        {
            Item = rendering.Item;
            Submitted = false;
			SendEmail = false;
		}
    }
}