using Neambc.Seiumb.Feature.Forms.Enums;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Forms.Models
{    
    public class UsernameFormModel : IRenderingModel
    {
        public string CurrentUsername { get; set; }

        [Required]
		[MaxLength(100, ErrorMessage = "Error Length")]
		public string NewUsername { get; set; }

		[Required]
		[MaxLength(100, ErrorMessage = "Error Length")]
		public string ConfirmNewUsername { get; set; }
		public bool HasErrorNewUsernameLength { get; set; }
		public bool HasErrorNewUsername { get; set; }
        public bool HasErrorConfirmNewUsernameLength { get; set; }
        public List<ProfileErrors> Errors { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;

        }
    }
}