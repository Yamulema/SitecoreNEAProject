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
    public class ZipCodeValidationFormModel : IRenderingModel
    {
        [Required]
		[MaxLength(5, ErrorMessage = "Error Length")]
		public string ZipCode { get; set; }

        public string FullName { get; set; }
		public bool HasErrorZipcode { get; set; }
		public bool HasErrorZipcodeLength { get; set; }


		public List<ZipCodeValidationErrors> Errors { get; set; }

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