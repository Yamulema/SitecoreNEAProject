using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class SweepstakesDTO
	{
		public string FullName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool HasEligibility { get; set; }
		public bool HasCheckEligibility { get; set; }
		public string SweepstakesId { get; set; }
		public bool HasClassEligibility { get; set; }
		public string Target { get; set; }
		public string ContextItem { get; set; }
        public SweepstakesBaseDTO SweepstakesBase { get; set; }

        public void Initialize(Rendering rendering)
		{
            SweepstakesBase= new SweepstakesBaseDTO();
            SweepstakesBase.Rendering = rendering;
            SweepstakesBase.Item = rendering.Item;
            SweepstakesBase.PageItem = PageContext.Current.Item;
			SweepstakesBase.SocialShare = new GeneralContent.Models.SocialShareModel();
			SweepstakesBase.ShowContactInfo = false;
            SweepstakesBase.ListPartners = new List<string>();
			SweepstakesBase.ListPartnersItems = new List<Item>();
			HasEligibility = false;
			HasCheckEligibility = false;
            SweepstakesBase.IsProcessedSuccessfully = false;
            SweepstakesBase.HasErrors = false;
            
        }
	}
}