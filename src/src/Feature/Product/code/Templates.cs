using Sitecore.Data;

namespace Neambc.Seiumb.Feature.Product
{
    public struct Templates
    {
        public struct ProductDetail
        {
            public static readonly ID ID = new ID("{5661F4B3-A9E7-40F5-9F77-2E93AA70DEF9}");

            public struct Fields
            {
                public static readonly ID ProductDescription = new ID("{965B2932-7D14-4605-AFBE-6D1033958BF0}");
                public static readonly ID PartnerImage = new ID("{20B1F590-18AF-4657-97A6-29F85595A857}");
                public static readonly ID Sidebar = new ID("{BF31300B-5D22-4212-8171-83E85D5D2B79}");
                public static readonly ID Share = new ID("{5A2F309B-3FCA-4005-9690-DED126305E64}");
                public static readonly ID Disclaimer = new ID("{375F7C5F-E5AC-4DC3-990F-72FF5C746FF3}");
                public static readonly ID ImageListing = new ID("{9B30869F-186B-43A0-82C6-8340BEA71FBC}");
                public static readonly ID HeadlineListing = new ID("{4E6A5D79-F1DD-40B5-9ADF-DD4B4B6AFAC3}");
                public static readonly ID SummaryListing = new ID("{A50409D3-C6B3-4DE3-B7AD-3D6ACDAC88CC}");
                public static readonly ID MoreLinkListing = new ID("{89B0FB04-597A-48E6-A8CF-5037D3CADA85}");
            }
        }

        public struct ProductLite
        {
            public static readonly ID ID = new ID("{9E06E7AF-296B-441E-AFCD-FEEDAA995348}");

            public struct Fields
            {
                public static readonly ID ProgramCode = new ID("{708593F0-EF5F-4CF1-8919-3F2A9A085DB9}");
                public static readonly ID Eligibility = new ID("{609D56AF-AD4D-4DCF-A690-7F6C8F94D35E}");
                public static readonly ID GaEvent = new ID("{21877B5A-1C7A-48D8-B136-22343C158709}");
                public static readonly ID PreLoginMessage = new ID("{62FAA9BB-88F4-4BCB-B0BD-B923E1F379D9}");
                public static readonly ID NotEligibleMessage = new ID("{B86F4672-C893-43C0-99D5-3986D223E7AE}");
                public static readonly ID ProductImage = new ID("{0BD75CE8-249E-48D5-96A0-A4269936B430}");
                public static readonly ID ProductHeadline = new ID("{66E1662D-6485-4D32-8FCB-ADD4770B8F0C}");
                public static readonly ID ProductSubHeadline = new ID("{B96733B7-5086-4AF4-8686-EE2BF58CC4A9}");
                public static readonly ID PromoDescription = new ID("{5E340330-767F-415D-9614-18F1C98DDFDD}");
                public static readonly ID RegistrationButton = new ID("{10CC799C-9710-4EE0-AE7C-E476825B7EA3}");
                public static readonly ID DesktopLoginButtonText = new ID("{912CDB5F-0FA7-4ABE-B8EC-E5B05561E06B}");
                public static readonly ID MobileLoginButton = new ID("{796BE7AB-0366-4FB9-AAD1-A586C62D19AE}");
                public static readonly ID CTA1Link = new ID("{A2B8DB91-6760-41D4-9DC5-E4EDA11D865F}");
                public static readonly ID CTA1Type = new ID("{3F511131-B0A6-4B73-9B1D-066CF71A7F59}");
                public static readonly ID CTA1PostData = new ID("{E2CAA321-296F-4B2E-9140-7FE8118C0117}");
                public static readonly ID CTA1Goal = new ID("{9EC825C3-30E3-45C8-95A6-315E67627A04}");
                public static readonly ID CTA2Link = new ID("{051BDC03-F2BB-4E22-8A55-5243538CDA9A}");
                public static readonly ID CTA2Type = new ID("{DFFFF6E4-6CCE-4E3D-9FA7-F94C3D8DC2FC}");
                public static readonly ID CTA2PostData = new ID("{FD502E5E-93BF-4816-B85F-48E4848951BC}");
                public static readonly ID CTA2Goal = new ID("{2231C533-E09F-49CA-ACF3-25CAAAEB242D}");
                public static readonly ID CTA3Link = new ID("{BE5B6E97-47A2-4923-A740-88099873CBCD}");
                public static readonly ID CTA3Type = new ID("{6F60EC74-E534-4FFD-82E6-FEC50502CFE8}");
                public static readonly ID CTA3PostData = new ID("{C56E4785-0372-4666-A38A-67A6E96979E8}");
                public static readonly ID CTA3Goal = new ID("{87F1357D-5FE3-47ED-84C5-95C60D4920FA}");
            }
        }

        public struct NameValueItem
        {
            public static readonly ID ID = new ID("{71BE9887-E50B-426E-A7EF-18F27844BA0C}");

            public struct Fields
            {
                public static readonly ID ItemName = new ID("{73494340-4626-4D41-A5E7-F0EC2664C0B0}");
                public static readonly ID Value = new ID("{6E092B84-F2CE-477A-AF26-DB4C0BF85AD0}");
            }
        }

        public struct ProductList
        {
            public static readonly ID ID = new ID("{921D6E7E-8B5B-4D7E-9462-F458E97657F4}");

            public struct Fields
            {
                public static readonly ID ProductItemList = new ID("{893A012A-F580-4D7E-A79E-A205FEE54047}");
            }
        }

        public struct ProgramCode
        {
            public static readonly ID ID = new ID("{4E79CDB4-366E-42F2-8B7F-C6FB0888798B}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{3FA7E02B-5291-4E38-83A3-B315251B80E2}");
                public static readonly ID ProgramCode = new ID("{432257EC-50FA-4100-AD61-002C641EA8A7}");
            }
        }

        public struct Navigable
        {
            public struct Fields
            {
                public static readonly ID ShortTitle = new ID("{A137093C-DE37-4C2D-A519-2A5D29B3147B}");
            }
        }

        public struct TwoColumnListPageType
        {
            public static readonly ID ID = new ID("{AD0853BA-639C-44BA-8EF9-EF4AE713B631}");
        }
	    public struct Home
	    {
		    public static readonly ID ID = new ID("{72051F7F-87F7-4279-B455-5A7EACA902F5}");
	    }
	}
}