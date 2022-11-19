using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class SocialShareModel
    {
        public bool ShowSocialShare { get; set; }
        public string AddThisHtml { get; set; }
        public string AddThisJSContent { get; set; }

        public SocialShareModel()
        {

        }

        public SocialShareModel(Item item, ID templateId = null)
        {
            var siteSettings = Sitecore.Context.Database.GetItem(Configuration.SiteSettingsId);
            if (siteSettings != null)
            {
                AddThisHtml = siteSettings.Fields[Templates.SiteSettings.Fields.InlineButtons].Value;
                AddThisJSContent = siteSettings.Fields[Templates.SiteSettings.Fields.AddThisCodeSnippet].Value;
                if (templateId == (ID)null)
                {
                    ShowSocialShare = ((CheckboxField)item.Fields[Templates.Shareable.Fields.ShowSocialShare]).Checked;
                }
                else
                {
                    ShowSocialShare = ((CheckboxField)item.Fields[templateId]).Checked;
                }
            }
        }
    }
}