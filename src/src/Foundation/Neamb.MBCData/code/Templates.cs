using Sitecore.Data;

namespace Neambc.Neamb.Foundation.MBCData
{
    public struct Templates
    {
        public struct Newsletters
        {
            public struct Fields
            {
                public static readonly ID Id = new ID("{55DE5632-19E9-4C6F-B983-4770542AB84F}");
                public static readonly ID Vendor = new ID("{965D672E-D77A-4128-BAFA-2C6B0451EBC1}");
                public static readonly ID Headline = new ID("{1E76F3F4-8C45-4622-B347-0E5BB4B0A699}");
                public static readonly ID Description = new ID("{A6EA7686-5F0C-4635-B305-00060B13D1D1}");
                public static readonly ID Subscribe = new ID("{3C8D6C04-DA57-4C5D-B337-BDB3F772AEEC}");
                public static readonly ID Subscribed = new ID("{AF3661AC-FE96-4E44-A99D-89D3FE07FE2D}");
                public static readonly ID Unsubscribe = new ID("{B6E2B48E-4C48-4A83-87BE-1CBB9D7D5E93}");
            }
        }
		public struct RegistrationPage
		{
			public static readonly ID ID = new ID("{016CB02B-98DA-403E-B75F-538BF642DFE8}");
		}
        public struct NewsletterCTA
        {
            public static readonly ID ID = new ID("{6D3C7D4B-46A6-4B48-BC7B-E299B7E723F6}");
            public struct Fields
            {
                public static readonly ID Eyebrow = new ID("{54BEC316-E09E-493B-9F7C-093A034FAE05}");
                public static readonly ID Name = new ID("{008FB0DA-A24B-4BFF-B47F-B0E409F76C42}");
                public static readonly ID Subhead = new ID("{0ABEE25D-5458-4B7D-B2F3-2935ACA6209C}");
                public static readonly ID Placement = new ID("{B2C0B021-1E53-4029-922A-5F7C7478A437}");
                public static readonly ID Image = new ID("{F329B22A-7130-40C5-96E8-ECD8C2B8B163}");
                public static readonly ID Video = new ID("{FAF36F17-4552-4462-AC43-3B53FA95F64C}");
                public static readonly ID Code = new ID("{5A5C6410-6B51-4A3A-A1E4-BD1F35DB8DB8}");
                public static readonly ID Subscribe = new ID("{B383A29F-289D-4F08-8DF5-1429E15D5949}");
                public static readonly ID Subscribed = new ID("{519E265A-F98F-440C-AAD0-DBA70DE76CAE}");
                public static readonly ID Unsubscribe = new ID("{30A9FEAB-B5D4-4DED-9DC7-69FFEF5EAA37}");
                public static readonly ID FinePrint = new ID("{F968319B-88F3-4653-B241-0B614CABD088}");
                public static readonly ID PublicNewsletter = new ID("{251A9AFE-476D-4679-AC74-A8F907D282A3}");
            }
        }
        public struct RetirementSeminarCta
        {
            public static readonly ID ID = new ID("{57AF696E-73B6-41AD-9494-30A39BF48667}");

            public struct Fields
            {
                public static readonly ID Seminar = new ID("{942C9D25-8D1C-41EA-94D7-1FCC778C195A}");
                public static readonly ID RegisteredUserMessage = new ID("{B7F8583B-1988-4F64-A4EB-EEC81726EAED}");
            }
        }

        public struct ProductCTAs
        {
            public static readonly ID ID = new ID("{CDCEBEEF-02EB-4CF2-86A3-41BC0D31F613}");

            public struct Fields
            {
                public static readonly ID Name = new ID("{D4FDCA7E-7BD4-41DF-A4EC-C0C141341670}");
                public static readonly ID ProductCodeDroplink = new ID("{D7125B4C-E4AA-4C56-A7E2-A5BC2369B88B}");
            }
        }
        public struct ProductCode
        {
            public static readonly ID ID = new ID("{699A13C3-F2A0-440F-8D79-737CCAE610D1}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{A7612FAF-7445-4ADB-BEFC-1810C14E5414}");
                public static readonly ID ProductCode = new ID("{B01AD396-BC36-486A-839E-889926842C54}");
            }
        }
    }
}