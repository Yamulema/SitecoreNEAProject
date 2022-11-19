using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class DuplicateRegistrationFormModel : IRenderingModel
    {
        public string CurrentEmail { get; set; }
        public List<EmailDuplicate> EmailList { get; set; }


        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
		public string RedirectAction { get; set; }
		public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;

        }
    }

    public class EmailDuplicate
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
    }
}