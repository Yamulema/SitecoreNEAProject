using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class SocialShareModel
    {
        public bool ShowSocialShare { get; set; }
        public string AddThisHtml { get; set; }
        public string AddThisJSContent { get; set; }

        public SocialShareModel()
        {

        }

        public SocialShareModel(Item item)
        {
            var siteSettings = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);
            if (siteSettings != null) {
                ShowSocialShare = ((CheckboxField)item.Fields[Templates.Shareable.Fields.ShowSocialShare]).Checked;
                AddThisHtml = siteSettings.Fields[Templates.SiteSettings.Fields.InlineButtons].Value;
                AddThisJSContent = siteSettings.Fields[Templates.SiteSettings.Fields.AddThisCodeSnippet].Value;
            } 
        }
    }
}