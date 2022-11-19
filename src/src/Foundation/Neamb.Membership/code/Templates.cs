using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Membership
{
    public struct Templates
    {

        public struct StatesGlobal
        {
            public static readonly ID ID = new ID("{1C0CF0BA-D674-418E-A807-72EF1BA9359C}");
        }

        public struct NameValueItem
        {
            public static readonly ID ID = new ID("{D1402B59-E079-4856-9DFB-551B6C87B7AE}");

            public struct Fields
            {
                public static readonly ID ItemValue = new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}");
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

        public struct LoginPage
        {
            public static readonly ID ID = new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}");
        }

        public struct MarketplacePage
        {
            public static readonly ID ID = new ID("{8AB0DA6D-A30C-4E23-BA51-B76E0D1123FD}");
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
        public struct DuplicateRegistrationPage
        {
            public static readonly ID ID = new ID("{300BAF41-8DF0-4C83-9741-CF3D61529BF8}");
        }

        public struct HomePage
        {
            public static readonly ID ID = new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}");
        }

        public struct ZipCodeVerificationPage
        {
            public static readonly ID ID = new ID("{F53DDE81-AE70-47C4-9AF5-DCD87D0D7A82}");
        }

        public struct RegistrationPage
        {
            public static readonly ID ID = new ID("{016CB02B-98DA-403E-B75F-538BF642DFE8}");
        }

        public struct ForgotPasswordPage
        {
            public static readonly ID ID = new ID("{25FBED04-5D2E-44B1-8C05-492A9246D2D8}");
        }

        public struct ResetPage
        {
            public static readonly ID ID = new ID("{FCD0B7D8-BCA1-42ED-A8E0-22C101D22FCD}");
        }

        public struct ResetPageDisavow
        {
            public static readonly ID ID = new ID("{71B872ED-2C1F-498A-8414-1BE313C448FD}");
        }

        public struct ForgotEmailPage
        {
            public static readonly ID ID = new ID("{7DC69553-4813-4956-AC32-F8B4C65AE686}");
        }
    }
}