using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.ComponentModel.DataAnnotations;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class ResetPasswordModel : IRenderingModel
    {
        public string UserName { get; set; }
        public Item Item { get; set; }
        public bool Submitted { get; set; }
        public bool IsUsernameValidToken { get; set; }
        public bool IsPasswordReset { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public void Initialize(Rendering rendering)
        {
            Item = rendering.Item;
            Submitted = false;
        }        
    }
}