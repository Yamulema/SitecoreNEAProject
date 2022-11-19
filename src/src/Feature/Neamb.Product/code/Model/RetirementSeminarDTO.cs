using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Model
{
    public class RetirementSeminarDTO
    {
        public string SeminarId { get; set; }
        public string ReminderId { get; set; }
        public string SeminarDateTime { get; set; }
        public string Presenter { get; set; }
        public string Location { get; set; }
        public SweepstakesBaseDTO SweepstakesBase { get; set; }
        public string ContextItem { get; set; }
        public bool AlreadyRegistered { get; set; }
        public bool IsMember { get; set; }
        public bool IsValidSeminary { get; set; }
        public bool IsWarmUser { get; set; }
        public string GtmAction { get; set; }

        public void Initialize(Rendering rendering)
        {
            SweepstakesBase = new SweepstakesBaseDTO();
            SweepstakesBase.Rendering = rendering;
            SweepstakesBase.Item = rendering.Item;
            SweepstakesBase.PageItem = PageContext.Current.Item;
            SweepstakesBase.SocialShare = new GeneralContent.Models.SocialShareModel();
            SweepstakesBase.ShowContactInfo = false;
            SweepstakesBase.ListPartners = new List<string>();
            SweepstakesBase.IsProcessedSuccessfully = false;
            SweepstakesBase.HasErrors = false;
            AlreadyRegistered = false;
            IsMember = false;
            IsValidSeminary = false;
            IsWarmUser = false;
        }
    }
}