namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class SweepstakeGtm
    {
        public string Event { get; set; }
        public string NavType { get; set; }
        public string NavText { get; set; }
		public SweepstakeGtm() {
            Event = "navigation";
            NavType = "embedded link";
        }
    }
}