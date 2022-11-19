using Neambc.Seiumb.Feature.Forms.Enums;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class PasswordFormModel : IRenderingModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }

        public List<ProfileErrors> Errors { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public bool HasErrorCurrentPassword { get; set; }
        public bool HasErrorCurrentPasswordInvalidCharacters { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
        }
    }
}