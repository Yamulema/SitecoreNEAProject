namespace Neambc.Neamb.Foundation.MBCData.Model.CompIntroLife
{
    public class CompIntroLifeEligibilityModel
    {
        public bool CompEligible { get; set; }
        public bool IntroEligible { get; set; }
        public bool CurrentMember { get; set; }
        public bool? CompMembershipTypeMatch { get; set; }
        public bool? IntroMembershipTypeMatch { get; set; }
        public bool? IntroNewMemberSegmentMatch { get; set; }
        public bool? IntroEffectiveDateValid { get; set; }
        public bool? InformationReviewNeeded { get; set; }
        public string SignDate { get; set; }
    }
}