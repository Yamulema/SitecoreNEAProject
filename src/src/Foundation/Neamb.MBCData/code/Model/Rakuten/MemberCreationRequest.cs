namespace Neambc.Neamb.Foundation.MBCData.Model.Rakuten
{
    public class MemberCreationRequest
    {
        public string EmailAddress { get; set; }
        public string MemberSource { get; set; }
        public string BonusId { get; set; }
        public bool TriggerpasswordEmail { get; set; }
    }
}