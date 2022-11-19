using Sitecore.Data;

namespace Neambc.Seiumb.Feature.Cards
{
    public class Templates
    {
        public struct Cards
        {
            public struct CardItem
            {
                public static readonly ID ID = new ID("{4F9366CB-F874-4DAA-8479-658961186C6C}");

                public struct Fields
                {
                    public static readonly ID TitleBarSkin = new ID("{6B36075A-654D-45D5-9973-1E6D1AC4C642}");
                    public static readonly ID Title = new ID("{63056AC1-9C1A-4191-9844-505C806CCA22}");
                    public static readonly ID Icon = new ID("{23E1DD2C-C44F-4203-9F34-EED0042AA5D0}");
                    public static readonly ID Image = new ID("{93DC6670-B05C-476E-84D4-AD82CE568389}");
                    public static readonly ID Body = new ID("{D0853B60-C18B-4FAC-9FF8-808326C049E2}");
                }
            }
        }
    }
}