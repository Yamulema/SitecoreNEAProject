using Sitecore.Data;

namespace Neambc.Seiumb.Feature.GeneralContent
{
    public struct Templates
    {
        public struct SmallAccordion
        {
            public static readonly ID ID = new ID("{807C333A-322C-4E23-9607-E4E2C6806B13}");
            public struct Fields
            {
                public static readonly ID Header = new ID("{BE259F86-A61F-4E00-9311-C3B3592E33C9}");
                public static readonly ID Subhead = new ID("{9B034914-7888-447E-8985-F9666C1BC31C}");
            }
        }
        public struct SmallAccordionItem
        {
            public static readonly ID ID = new ID("{64B91C7E-D47D-4789-AA2A-9F805E8D18FA}");
            public struct Fields
            {
                public static readonly ID Header = new ID("{751DA790-22A8-4CEE-8F77-599A14567A6A}");
                public static readonly ID Subhead = new ID("{1A67FF8D-AFA8-4316-B394-105A3EEE5934}");
            }
        }
        public struct Product
        {
            public static readonly ID ID = new ID("{D1889EB8-BE95-4E99-B8E9-3A0AEB8F4800}");
            public struct Fields
            {
                public static readonly ID PageTitle = new ID("{F71F7747-F88D-499B-AC69-D3A6DC9B0A88}");
            }
        }
        public struct DefaultChat
        {
            public struct Fields
            {
                public static readonly ID SnippetItem = new ID("{F29A91F8-E868-4B5D-9681-C058DCB3EAAB}");
            }
        }

        public struct ChatSnippetItem
        {
            public struct Fields
            {
                public static readonly ID Snippet = new ID("{3DBBC60D-5F36-4357-84F2-3C6B2C36E986}");
            }
        }

        public struct FourStepsCarousel
        {
            public static readonly ID ID = new ID("{17197E5D-9D30-4AD0-9064-406424F7A21A}");
            public struct Fields
            {
                public static readonly ID HeaderText = new ID("{956D3F25-92DE-4AFF-81A6-660827F51AA3}");
                public static readonly ID BottomText = new ID("{69A4F025-08AB-4AE5-9BD3-45655D39CC5A}");
                public static readonly ID Step1 = new ID("{79C2AA69-593A-4451-B921-A3274A725097}");
                public static readonly ID Step2 = new ID("{4D6AA53A-0D10-439C-9ABB-F499207351C6}");
                public static readonly ID Step3 = new ID("{6BCF3BDB-BA4D-4BE3-95D9-8EEB6F009AE4}");
                public static readonly ID Step4 = new ID("{D434D885-9470-458C-8EBA-FC28C4D0CE75}");
            }
        }
    }
}