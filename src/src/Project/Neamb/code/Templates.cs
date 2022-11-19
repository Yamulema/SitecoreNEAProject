using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Project.Web
{
    public struct Templates
    {
        public struct SiteSettings
        {
            public static readonly ID ID = new ID("{4A236EC7-6029-4230-A16D-688A1CA89832}");

            public struct Fields
            {
                public static readonly ID FooterContent = new ID("{64D1543D-7017-4487-A7C5-1B03D77B4A94}");
                public static readonly ID HeaderLogo = new ID("{F685AF3A-8126-4543-ACDE-05DD51C7E63E}");
                public static readonly ID HeaderCreateAccount = new ID("{5C0454A1-397E-4368-B0D9-56EF1A27BAF9}");
                public static readonly ID HeaderSignIn = new ID("{56D767AD-7C8E-4837-B5E1-9DB1FA136B6E}");
                public static readonly ID HeaderAvatar = new ID("{EC93C27B-A621-4B20-AD31-8559715607C7}");
                public static readonly ID HeaderAvatarWarmHot = new ID("{4AE27660-DB6D-4241-B975-69C71B3CC18F}");
                public static readonly ID HeaderSearchPlaceholder = new ID("{E66B41F4-5634-4EAD-9E85-91ACBB937789}");
                public static readonly ID HeaderGetStartedHeading = new ID("{D9B92F34-F4B8-4E11-B0EF-7B900440AE07}");
                public static readonly ID HeaderGetStartedSubheading = new ID("{AE8E2ECB-6782-459F-B96C-E6EB8F568205}");
                public static readonly ID HeaderNavigation = new ID("{403DAA95-9B98-49B7-94B5-3CBB3AD87251}");
                public static readonly ID CodeSnippet = new ID("{051E5ACF-950A-4733-B339-54AC5701B998}");
                public static readonly ID SearchPage = new ID("{826ECFA8-7526-42A1-BA6B-CDDD30C0A7A3}");
                public static readonly ID JavascriptCodeTop = new ID("{0CBE674D-9D8D-4ADF-A924-0976A0344040}");
                public static readonly ID JavascriptCodeBottom = new ID("{36D71156-18E8-4783-B971-A4C420159D07}");
                public static readonly ID JavascriptCodeTopBody = new ID("{8BB63596-EFCE-47E5-BB6B-1B2ED7ED4809}");
                public static readonly ID CreateAccount = new ID("{5C0454A1-397E-4368-B0D9-56EF1A27BAF9}");
            }
        }

        public struct NavigationItem
        {
            public static readonly ID ID = new ID("{1001C69F-F3AA-4286-A9ED-AE84AE79C5DA}");

            public struct Fields
            {
                public static readonly ID MenuItem = new ID("{42EDF202-8525-4A35-BC8F-542A9EDB10CF}");
                public static readonly ID MenuItemLink = new ID("{7AB8FB18-FA16-4EAF-8DE9-ED7255829204}");
                public static readonly ID Submenu = new ID("{5C81C7F6-DFBA-4231-959F-8FCCFDEC4BD7}");
                public static readonly ID SubmenuMobile = new ID("{F52126F6-A9EC-42FF-AFD5-C0481B94E469}");
            }
        }

        public struct LoginPage
        {
            public static readonly ID ID = new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}");
        }

        public struct DuplicateRegistrationPage
        {
            public static readonly ID ID = new ID("{300BAF41-8DF0-4C83-9741-CF3D61529BF8}");
        }

        public struct RegistrationPage
        {
            public static readonly ID ID = new ID("{016CB02B-98DA-403E-B75F-538BF642DFE8}");
        }

        public struct AccountMenu
        {
            public static readonly ID ID = new ID("{3DFE963F-C723-47CF-8934-91436F43442F}");

            public struct Fields
            {
                public static readonly ID Links = new ID("{B4F7A597-FEDA-4810-A874-B2471B792317}");
                public static readonly ID NotYou = new ID("{EB51339A-1F08-4EC3-919C-6D3C9985F0DE}");
                public static readonly ID SignOut = new ID("{8D287D22-C4D6-496A-B11F-C2A4155C74B0}");
                public static readonly ID SetProfileNotification = new ID("{2E1B495B-C8F1-43AD-9CF4-AEF537618E25}");
                public static readonly ID NotificationLink = new ID("{E130DA48-99A5-47DC-89EF-3E02214E2256}");
                public static readonly ID NotificationDismissal = new ID("{F0888EAF-CE9E-475E-A98F-A23C5871AEE1}");
            }
        }

        public struct Metadata
        {
            public static readonly ID ID = new ID("{1A2CE642-F13F-4BEB-AFC4-FE43DFB06623}");

            public struct Fields
            {
                public static readonly ID MetaTitle = new ID("{A3258719-614E-4F9E-BCE6-DFF1E9E66226}");
                public static readonly ID MetaDescription = new ID("{CFD2D139-2D6C-42F1-9C60-F5FF6ADFA31F}");
                public static readonly ID MetaKeywords = new ID("{522E8D3F-98DE-43BC-B131-F55713775BDD}");
                public static readonly ID DisableIndexing = new ID("{BF61C3CA-10EB-4B01-B831-983D17CED656}");
                public static readonly ID DoNotFollowLinks = new ID("{D6F12F08-575F-44F3-8BF5-37EEBB48BA23}");
            }
        }

        public struct CategoryItem
        {
            public static readonly ID ID = new ID("{D1402B59-E079-4856-9DFB-551B6C87B7AE}");

            public struct Fields
            {
                public static readonly ID Value = new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}");
            }
        }

        public struct OpenGraph
        {
            public static readonly ID ID = new ID("{6DFFAC57-6610-4785-A1DD-039FDFB7E139}");

            public struct Fields
            {
                public static readonly ID Type = new ID("{8E450537-57D3-437E-903B-D6559CC24FCD}");
                public static readonly ID Title = new ID("{CDC52512-7A8F-46ED-910F-0E53E16AD050}");
                public static readonly ID Description = new ID("{DC932696-9F6F-4B6D-93AD-DF2B1C440012}");
                public static readonly ID Thumbnail = new ID("{F9ABAC2E-9411-4DF6-A2B8-B223801504E0}");
            }
        }

        public struct PageInfo
        {
            public static readonly ID ID = new ID("{367B8E27-D435-49A7-BA34-5D8F44FC1EB8}");

            public struct Fields
            {
                public static readonly ID PageTitle = new ID("{F71F7747-F88D-499B-AC69-D3A6DC9B0A88}");
                public static readonly ID ShortDescription = new ID("{FEC5D746-A317-48EA-BD97-D81D7FF151D6}");
                public static readonly ID Thumbnail = new ID("{A552581C-7279-4485-9BCD-32CF536565F8}");
            }
        }

        public struct ProfilePassword
        {
            public static readonly ID ID = new ID("{8DFF374B-52A5-48A9-BE20-412D6C173856}");
        }

        public struct SettingSubscription
        {
            public static readonly ID ID = new ID("{1131EBD3-3291-40D7-894B-8ABBC1224945}");
        }

        public struct CompLife
        {

            public static readonly ID ID = new ID("{293D42C9-C7B9-4A8D-88E4-081BBA5637C7}");
            public struct Fields
            {
                public static readonly ID IntroLifeTitle = new ID("{05537A3A-34CD-4A46-BB8E-61B990A62A75}");
            }
        }

        public struct MemberWelcome
        {
            public static readonly ID ID = new ID("{D6F2C9DC-836C-4831-A081-9F52B4A4B3CD}");

            public struct Fields
            {
                public static readonly ID CampaignCode = new ID("{12E3887C-08AF-4837-97AF-81728374FF98}");
            }
        }
        public struct ZipCodeVerificationPage
        {
            public static readonly ID ID = new ID("{F53DDE81-AE70-47C4-9AF5-DCD87D0D7A82}");
        }
        public struct GenreAttribute
        {
            public static readonly ID ID = new ID("{249E4E0B-A07F-4321-96ED-9434C0046FD5}");

            public struct Fields
            {
                public static readonly ID Genre = new ID("{72428A2D-B547-4E83-8B2A-3319B6FE8394}");
            }
        }
        public struct SchemaMarkup
        {
            public static readonly ID ID = new ID("{74159DE1-A691-4A83-8CEE-1D7109BC3457}");
            public struct Fields
            {
                public static readonly ID PageSchemaMarkup = new ID("{C5D74C28-44FA-4537-97DE-3C9F81341100}");
            }
        }
        public struct ComplimentaryLifePage
        {
            public static readonly ID ID = new ID("{861BCE65-8DAB-4B4B-B983-12A0CF873FED}");
        }
		public struct ContentFilter
        {
            public static readonly ID ID = new ID("{D8BE87A5-AD90-45B3-9B5B-AF2963F14423}");
        }
        public struct HeaderOffer
        {
            public static readonly ID ID = new ID("{835E3D17-F4D8-41F1-ADA9-ECE163E493D8}");
            public struct Fields
            {
                public static readonly ID Offer = new ID("{3D2C7CA0-7653-4B31-968C-A6CCDA95954F}");
            }
        }
        public struct ProductOfferCard
        {
            public static readonly ID ID = new ID("{BE6F5ACA-A166-4D70-918A-FC2C330111C5}");

            public struct Fields
            {
                public static readonly ID Eligibility = new ID("{26713851-08B7-45FB-B809-6E3DDE002765}");
                public static readonly ID Image = new ID("{6D238F7E-422F-4034-8614-91CA850EE248}");
                public static readonly ID Title = new ID("{759B52DE-E620-4A28-8498-B9965B37216B}");
                public static readonly ID Description = new ID("{9E269846-70E9-4A47-BF53-87B0262E933B}");
                public static readonly ID Type = new ID("{1E92D319-3D06-44D0-ACA1-D504FA9E70D6}");
                public static readonly ID Link = new ID("{D6DAB03D-D771-4C76-A6C0-7C48F10FE0BE}");
                public static readonly ID Cta = new ID("{FAF5E77E-5245-4751-875B-416384CBB209}");
                public static readonly ID EligibilityDetails = new ID("{AA31F3E2-5BBF-4AB7-91A6-E3F15A779693}");
                public static readonly ID AnchorCard = new ID("{93831F02-E18E-4579-A53E-56A00F830184}");
                public static readonly ID PostData = new ID("{B66FCBBF-9992-4758-A4CC-213149D5C588}");
                public static readonly ID Goal = new ID("{D785C191-24E6-488A-9060-C84D3183AD8D}");
                public static readonly ID ProductCodeDroplink = new ID("{5EEE1604-783B-49C1-AB00-60BA69EE79F2}");
            }
        }
        public struct OfferLink
        {
            public static readonly ID ID = new ID("{0DF50B62-3518-41A8-9246-DAF055A5DF00}");
        }
        public struct OfferLinkItem
        {
            public static readonly ID ID = new ID("{9889BA7D-6BC0-4794-BD3D-96597829A762}");

            public struct Fields
            {
                public static readonly ID RequiresLogin = new ID("{E02093C8-48DD-4F23-8DEA-15736C5D8170}");
            }
        }

        public struct PageAssets
        {
            public static readonly ID ID = new ID("{2D3B7389-4171-406A-8319-03A7807988D5}");
            public struct Fields
            {
                public static readonly ID PageHeaderAssets = new ID("{BBFDFD69-A14E-40BA-A57D-3C4EE5DABFB1}");
                public static readonly ID PageFooterAssets = new ID("{73F01307-2732-4C70-8337-B1484C0760E7}");
            }
        }

        public struct PageAsset
        {
            public static readonly ID ID = new ID("{9727565D-A014-4BBE-8149-57E86C3BAB04}");
            public struct Fields
            {
                public static readonly ID Asset = new ID("{CE0FA224-FDAD-4D27-9117-D769D52BEED8}");                
            }
        }
    }
}