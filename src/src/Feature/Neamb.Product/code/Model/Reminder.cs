using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Model
{
    public class Reminder
    {
        public string Id { get; set; }
        public bool Enabled { get; set; }
        public bool Notified { get; set; }
        public Item Datasource { get; set; }
        public string ComponentIdAuthentication { get; set; }
        public bool HasResultAuthentication { get; set; }
    }
}