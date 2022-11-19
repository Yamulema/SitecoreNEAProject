using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class PassThroughAuthenticationModel : IRenderingModel
    {
        [Required] public string Password { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public bool HasErrorInvalidCredentials { get; set; }
        public bool HasAlreadyLockedError { get; set; }
        public bool HasLockedError { get; set; }
        public bool HasErrorInvalidUser { get; set; }
        public bool HasErrorPassword { get; set; }
        public bool HasErrorTimeout { get; set; }
        public bool IsValid { get; set; }
        public string Mdsid { get; set; }
        public string RedirectUrl { get; set; }
        public string PasswordResetUrl { get; set; }
        public string ProductCode { get; set; }
        public string QueryString { get; set; }
        public string UserName { get; set; }
        public bool HasErrorEligible { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;

            HasErrorInvalidCredentials = false;
            HasAlreadyLockedError = false;
            HasLockedError = false;
            HasErrorTimeout = false;
            HasErrorEligible = false;
            IsValid = false;
            HasErrorInvalidUser = false;

        }
    }

}