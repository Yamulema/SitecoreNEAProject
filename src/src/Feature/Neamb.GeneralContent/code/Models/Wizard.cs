using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    //Model
    public class Wizard
    {
        public Item Datasource { get; set; }
        //public WizardHeader Header { get; set; }
        public WizardButton StartButton { get; set; }
        public WizardButton SkipButton { get; set; }
        public WizardHeader Header { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsExperienceEditor { get; set; }
		public string GtmAction { get; set; }
	}
}