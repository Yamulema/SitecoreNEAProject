using Sitecore.Data;

namespace Neambc.Neamb.Feature.GeneralContent
{
    public struct Templates
    {
        public struct BasePageTemplates
        {
            public static readonly ID ID = new ID("{6EAA0401-9FE3-4E55-A8D5-C3E914DE0129}");
            public struct Article
            {
                public static readonly ID ID = new ID("{15547760-E485-446C-B7C0-98A660FD577E}");
                public struct Fields
                {
                    public static readonly ID PageBodyBody = new ID("{845D44DB-ABA6-406C-8300-0ABAAA9A19A3}");
                    public static readonly ID PageBodyBodyBackgroundColor = new ID("{CAECAFD2-BA43-4B8E-9CFE-1921500D0F6C}");
                    public static readonly ID BodyCopyBodyHeightLimit = new ID("{2C35129E-3A76-4324-AC98-24572E226A82}");
                }
            }
        }
        public struct SmallAccordion
        {
            public static readonly ID ID = new ID("{A49A235F-B5CD-433E-8D84-696BBFFC28EE}");
            public struct Fields
            {
                public static readonly ID Header = new ID("{8BA95705-3B7D-408B-9EA0-082A708E66E0}");
                public static readonly ID Subhead = new ID("{98011FF5-54AF-490B-8816-0E9EA2F161C9}");
            }
        }
        public struct SmallAccordionItem
        {
            public static readonly ID ID = new ID("{EB59A6BF-0B7E-4A0B-B9D9-DE3DCED37ECA}");
            public struct Fields
            {
                public static readonly ID Header = new ID("{2CCDA41E-0525-4EF5-9A02-0A9C87D2075D}");
                public static readonly ID Subhead = new ID("{9F8E9C78-17BB-4EB3-931C-5F326A8182A3}");
            }
        }
        public struct Attribution
        {
            public static readonly ID ID = new ID("{35490EA6-618A-4416-9E22-58CCD6390956}");
            public struct Fields
            {
                public static readonly ID Authors = new ID("{0CCD3F73-1942-4EEC-8383-884507E75041}");
                public static readonly ID PublishDate = new ID("{D7DF4284-50D1-4F7E-BF50-4D038B6C6E58}");
                public static readonly ID LastUpdated = new ID("{F0A62EF6-4CB8-4AB8-A239-591D2D0658F3}");
            }
        }
        public struct CategoryItem
        {
            public static readonly ID ID = new ID("{D74A94A9-9855-43E2-A4FA-0E5C14D9CB82}");
            public struct Fields
            {
                public static readonly ID Value = new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}");
            }
        }
        public struct Shareable
        {
            public static readonly ID ID = new ID("{9A54D88A-395B-4463-9B6A-7BA30FC9FE75}");
            public struct Fields
            {
                public static readonly ID ShowSocialShare = new ID("{EDAC8876-554A-4341-A01E-3A88AEDE0D01}");
            }
        }

        public struct SiteSettings
        {
            public static readonly ID ID = new ID("{4A236EC7-6029-4230-A16D-688A1CA89832}");

            public struct Fields
            {
                public static readonly ID InlineButtons = new ID("{ABBBEC1F-4935-4A2A-8586-D0417F65F797}");
                public static readonly ID Snippet1 = new ID("{81D552EA-DAFC-4561-8344-BA44EC1489BD}");
                public static readonly ID AddThisCodeSnippet = new ID("{051E5ACF-950A-4733-B339-54AC5701B998}");
            }
        }

        public struct PageBody
        {
            public static readonly ID ID = new ID("{C8B4ACA4-5BC1-4AE6-A6B1-2667C81F7BC9}");
            public struct Fields
            {
                public static readonly ID PageBodyBody = new ID("{845D44DB-ABA6-406C-8300-0ABAAA9A19A3}");
                public static readonly ID PageBodyBodyBackgroundColor = new ID("{CAECAFD2-BA43-4B8E-9CFE-1921500D0F6C}");
            }
        }

        public struct RichTextBlock
        {
            public static readonly ID ID = new ID("{4B9C98F8-8EF5-4F6C-B256-DF7CD14FC4CB}");
            public struct Fields
            {
                public static readonly ID Content = new ID("{7E8C87BC-E8FB-493D-BD99-5A0F738A5B7B}");
            }
        }

        public struct ContactUsForm
        {
            public static readonly ID ID = new ID("{1A5A6B90-853C-4F0D-84C6-68240CE1C5BB}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{4C4363A1-F5F8-4787-9273-4C3AE95C38C3}");
                public static readonly ID Intro = new ID("{AA23B301-E735-4570-96E7-9E82CE0E5709}");
                public static readonly ID Submit = new ID("{CBFAAA1F-574F-4FD2-A90D-14D1525A1364}");
                public static readonly ID SelectionLists = new ID("{5753C0FE-15F9-4521-936C-70B09EB2FAF4}");
                public static readonly ID Success = new ID("{810621FD-C787-48D9-890F-A8E2B4F708FC}");
                public static readonly ID Error = new ID("{E7ADBBA2-7FFD-4A54-B426-2F32642A0104}");
	            public static readonly ID RequiredCaptchaError = new ID("{AE14ECDE-5842-408E-83F8-98A1A75DB1D9}");
                public static readonly ID StateAffiliateLabel = new ID("{D144C823-A75D-4991-BC68-6D1F86EB934A}");
                public static readonly ID StateAffiliateTooltip = new ID("{83A479F1-5886-492D-941E-0DFC88552519}");
                public static readonly ID TopicLabel = new ID("{BC615AF1-34C1-4FE6-A8F5-819C1225D25F}");
                public static readonly ID TopicTooltip = new ID("{F9ACCD68-B5FB-4520-89CA-41C417A4D4D6}");
                public static readonly ID TopicNoSelection = new ID("{491007BC-A0D6-4A51-9E1E-A39A9B277674}");
                public static readonly ID MessageLabel = new ID("{22752840-D43D-4307-9626-86B9B9EE220E}");
                public static readonly ID MessageEmptyField = new ID("{D5FDA117-AF49-42DB-9DB9-993B800E549C}");
                public static readonly ID MessageCharactersLimit = new ID("{4B699871-2371-4DDF-ACB8-F9E22FF6341C}");
                public static readonly ID MessageInvalidCharacters = new ID("{1FC8B560-C79E-4A16-830B-BFCC8A8B3C8A}");
                public static readonly ID MessageCharactersCount = new ID("{A3F62490-AF38-4C25-BD7D-C7BB63E77E9B}");
            }
        }

        public struct Name
        {
            public static readonly ID ID = new ID("{14B21118-F060-4D0D-922D-42D4F720589A}");
            public struct Fields
            {
                public static readonly ID FirstNameLabel = new ID("{5B02F656-8C0E-49AA-A594-E89DD3333A1E}");
                public static readonly ID FirstNameTooltip = new ID("{E1E27FC7-3A6D-4F17-968F-91D67DBED460}");
                public static readonly ID FirstNameEmptyField = new ID("{4A08CFD7-853D-478F-8C99-B99E061B0F1D}");
                public static readonly ID FirstNameCharactersLimit = new ID("{9A1BB82C-82A2-4CA7-A5FE-8B6F09FE8B5C}");
                public static readonly ID FirstNameInvalidCharacters = new ID("{4BD97032-914A-493F-9CC1-5D1FD6476E90}");
                public static readonly ID FirstNameMinCharacterLimit = new ID("{A4F6A83B-5DE3-4035-BBC7-88823F4B61A7}");
                public static readonly ID LastNameLabel = new ID("{1E31CA98-49C0-417C-AAFD-191A6504089E}");
                public static readonly ID LastNameTooltip = new ID("{22297AE2-BDFB-4D6D-A9ED-61C2F60491CA}");
                public static readonly ID LastNameEmptyField = new ID("{9839A40C-EDA5-4C83-AB7F-75FF4748525D}");
                public static readonly ID LastNameCharactersLimit = new ID("{F7620F0C-1C3A-458E-AD51-A0F6395DCD2D}");
                public static readonly ID LastNameInvalidCharacters = new ID("{D164E8F4-3338-4EDA-8626-D0783A845DAC}");
                public static readonly ID LastNameMinCharacterLimit = new ID("{3F535FB5-4D08-485D-A712-72570256D4BF}");
            }
        }

        public struct Email
        {
            public static readonly ID ID = new ID("{B4A7ADBD-CF0A-4BF1-86BF-0A801D75A441}");
            public struct Fields
            {
                public static readonly ID Label = new ID("{A0789D92-22F3-4726-B79B-DD1C91EE4ECE}");
                public static readonly ID Tooltip = new ID("{341BA223-CC9B-4C18-8530-88D994393367}");
                public static readonly ID EmptyField = new ID("{852633DC-5A7B-43F5-B533-B9A046E56563}");
                public static readonly ID CharactersLimit = new ID("{38F547BF-95DD-4960-AB1B-6B2A8AC02B81}");
                public static readonly ID InvalidFormat = new ID("{D71690EA-7C45-4AE3-AC9A-BAC5E5A04AAE}");
                public static readonly ID EmailWarning = new ID("{678EB874-CB56-4A85-BC8D-64776A26B9FD}");
            }
        }
        public struct _CategoryItem
        {
            public static readonly ID ID = new ID("{D1402B59-E079-4856-9DFB-551B6C87B7AE}");
            public struct Fields
            {
                public static readonly ID Value = new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}");
            }
        }

        public struct LanguageToggle
        {
            public static readonly ID ID = new ID("{CB80D522-886F-47C0-B740-D029FD4DB5A8}");
            public struct Fields
            {
                public static readonly ID Default = new ID("{8DCF8F52-E74E-46CE-B897-DFE19D9D4E11}");
            }
        }
        public struct SecuredContent
        {
            public static readonly ID ID = new ID("{F6EC692D-57DC-4806-8747-735390BFB1F6}");
            public struct Fields
            {
                public static readonly ID SecuredBody = new ID("{760AC7D0-FDC1-4B48-A07C-621DB1598E06}");
            }
        }
        public struct ProductCTA
        {
            public static readonly ID ID = new ID("{CDCEBEEF-02EB-4CF2-86A3-41BC0D31F613}");
            public struct Fields
            {
                public static readonly ID ChatSnippet = new ID("{9B3E5D4A-0B15-4AEB-9EC9-6C8B16A501FE}");
                public static readonly ID ProductCodeDroplink = new ID("{D7125B4C-E4AA-4C56-A7E2-A5BC2369B88B}");
            }
        }

        public struct ProductCategory
        {
            public static readonly ID ID = new ID("{EEA0A232-C12B-4422-99C4-216A4E16FCDF}");
        }

        public struct ChatSnippetTemplate
        {
            public static readonly ID ID = new ID("{B09A9213-F675-420D-B7CF-D0CA0409406E}");
            public struct Fields
            {
                public static readonly ID ChatSnippet = new ID("{7F6652FC-16BB-4BF5-A67A-4DE01A776465}");
            }
        }

        public struct GuideCTA
        {
            public static readonly ID ID = new ID("{34DCBCF0-4249-406F-BAB8-0320CE867767}");
            public struct Fields
            {
                public static readonly ID ChatSnippet = new ID("{9B3E5D4A-0B15-4AEB-9EC9-6C8B16A501FE}");
                public static readonly ID Name = new ID("{EC18CB4E-04F7-4507-B746-86473CF5D30F}");
                public static readonly ID Description = new ID("{11E7A37C-E88A-4FD7-8514-E57DF5DD33C8}");
                public static readonly ID Image = new ID("{E702943C-99B5-41C4-B728-BA230A64CFE2}");
                public static readonly ID MaterialId = new ID("{700EF1BF-C47B-4265-AB43-1801D462B86C}");
                public static readonly ID Cta = new ID("{019C75C9-9075-4936-91E8-36CC4469ED10}");
                public static readonly ID GeneralError = new ID("{A8B35A1B-D9FF-46C1-860C-23378A1CCF96}");
                public static readonly ID CtaLogin = new ID("{D4AC55C9-717A-45CA-857C-B7BEB041BD4D}");
            }
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
        public struct RetirementIncomeCalculator
        {
            public static readonly ID ID = new ID("{1731FFCB-6DA8-4D31-97E2-2F0FEB87C83B}");
            public struct Fields
            {
                public static readonly ID AnonymousUser = new ID("{CFB0001D-A41E-48D9-8014-F4FE30BE51D1}");
                public static readonly ID IFrame = new ID("{E22D6508-7A9D-4BF4-B6EF-BB476032998E}");
            }
        }
	    public struct StatisticsCustom
	    {
		    public static readonly ID ID = new ID("{4D53FB3F-03CD-44FC-9CEC-EE81A5E0AD51}");
		    public struct Fields
		    {
			    public static readonly ID LastPublishDate = new ID("{AC72BAE6-B9F8-450D-94F5-87EEF71C5B03}");
		    }
	    }

	    public struct ChatSnippet
	    {
		    public static readonly ID ID = new ID("{BB400478-A5F0-41D1-85F6-7E28D3B7F657}");
		    public struct Fields
		    {
			    public static readonly ID Snippet = new ID("{3DBBC60D-5F36-4357-84F2-3C6B2C36E986}");
		    }
	    }
	    public struct UnsubscribeMail
	    {
		    public static readonly ID ID = new ID("{F4E56560-7F53-4D21-9B92-ADBF9E9A9903}");
		    public struct Fields
		    {
			    public static readonly ID Failure = new ID("{9C0C8D88-C78F-4283-8196-0380ED52203A}");
			    public static readonly ID Success = new ID("{74594B71-1810-4243-8487-008B192ED101}");
			    public static readonly ID Error = new ID("{E34AE485-F0E2-473A-A9E0-53FA8027CD5F}");
		    }
		}
        public struct LightBox
        {
            public static readonly ID ID = new ID("{DAF3EF5E-01D4-4FDD-A5B6-DB0F92FDC131}");
            public struct Fields
            {
                public static readonly ID Id = new ID("{9FB3A97A-37BB-4E67-A58E-B2534174C70A}");
                public static readonly ID Content = new ID("{57834E91-0493-4A02-A73A-819053FC04E0}");
            }
        }

        public struct SessionVariable
        {
            public static readonly ID ID = new ID("{C41CF1EE-DF4D-48E7-AC84-2E634BEC279E}");
            public struct Fields
            {
                public static readonly ID SessionVariables = new ID("{DC658572-8A3B-4006-BD7E-81C428F6AE80}");
            }
        }

        public struct Wizard {
            public static readonly ID ID = new ID("{8F4D3D34-49E2-4778-A7C1-1E67AA4345F4}");
            public struct Fields {
                public static readonly ID WelcomeMessage = new ID("{EDFA480B-B68E-4D42-AA26-6755A54E1459}");
                public static readonly ID Steps = new ID("{41C0EBAF-FCB6-4731-8543-DC3D59963353}");
                public static readonly ID StartButtonText = new ID("{61EFAB48-6573-41B1-BA02-D523C019C733}");
                public static readonly ID SkipButtonText = new ID("{B26E24B0-1887-4BF4-91E0-69DF14783F6C}");
                public static readonly ID Logo = new ID("{64950355-42E9-4B2D-9CEB-89AB8043E7DE}");
                public static readonly ID Back = new ID("{A756EB96-8F87-42F2-ACB0-2A12C43B149A}");
                public static readonly ID Next = new ID("{AF180F5C-9B48-49C3-AC4C-62C92CAF88B1}");
                public static readonly ID End = new ID("{5B90C439-B296-4B96-AB75-E01C6BDDBA70}");
                public static readonly ID StepText = new ID("{F94DCC4E-76B8-4E63-915E-655B15F18BBA}");
                public static readonly ID AnonymousUser = new ID("{9561B080-B632-492A-8D13-10EDE4DB641E}");
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
		public struct RedirectUrl
		{
			public static readonly ID ID = new ID("{B5967A68-7F70-42D3-9874-0E4D001DBC20}");
			public struct Fields
			{
				public static readonly ID RedirectItem = new ID("{544E5C57-1626-4972-8535-196F000E4B1F}");
				public static readonly ID RequestedUrl = new ID("{5CF9CCB5-5F2E-4F06-946D-3A952B089A68}");
			}
		}
		public struct ParentRedirect
		{
			public static readonly ID ID = new ID("{5443128F-ED27-40A5-B84D-10C28B316800}");
        }
        public struct WizardStep
        {
            public static readonly ID ID = new ID("{3D6E9153-142E-4E47-85C2-45D6C50BE5EA}");
        }
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
        public struct TopListing
        {
            public struct Fields
            {
                public static readonly ID HeadlineText = new ID("{22E74BA8-194B-496D-8484-20C7D4956297}");
                public static readonly ID Topics = new ID("{0EED3EDC-413E-4904-959F-E6FB74E1DCD5}");
                public static readonly ID ExpandText = new ID("{1D87765D-30C7-4613-BE61-CCC2B46666C8}");
                public static readonly ID CollapseText = new ID("{FE87A58F-6F65-40CD-A8D8-BB0F1ED07FDC}");
            }
        }

        public struct TwoColumnTout
        {
            public struct Fields
            {
                public static readonly ID Headline = new ID("{080A4635-D616-4AA8-9F0E-740B5F994272}");
                public static readonly ID Image = new ID("{F906A079-E335-4026-A1CA-C744CDD558FF}");
                public static readonly ID Content = new ID("{F423A4CE-C661-4C89-BCF0-754BC54BBD7C}");
                public static readonly ID BackgroundColor = new ID("{3028E1F8-8F51-4CFE-98C7-51262079823C}");
                public static readonly ID ImageAligmentRight = new ID("{6B42B684-A0E3-4C15-AC1E-B3B7F18688A8}");
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
        public struct HalfBackgroundTout
        {
            public static readonly ID ID = new ID("{AB57C66F-2B99-4D74-A59E-8357F9128ABD}");

            public struct Fields
            {
                public static readonly ID LeftBackgroundImage = new ID("{33EDE554-30AC-4ECB-8E4B-6F1087025184}");
                public static readonly ID MobileBackgroundImage = new ID("{D0F3854B-FE88-441B-9C5F-14C0C27C427C}");
                public static readonly ID FloatingImage = new ID("{482712BE-0930-49B2-B07D-A1BD9F632500}");
                public static readonly ID Body = new ID("{05E4E713-1864-42BB-A332-76F870FF5218}");
                public static readonly ID BackgroundColor = new ID("{8C3564D4-1BA2-4C69-8C73-18D3E31BABA6}");
                public static readonly ID Link = new ID("{8B031392-3830-4BA7-8847-D330E0E9A54C}");
                public static readonly ID LinkHtmlProperties = new ID("{0A5F632E-9E72-43B3-893C-73FF2C2F5402}");
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

        public struct LightBoxVideo
        {
            public static readonly ID ID = new ID("{63AEE6BD-8606-4DA5-B833-EE2DB4057F51}");

            public struct Fields
            {
                public static readonly ID ID = new ID("{E13B6ECB-818C-4B8D-B84E-E43D1E8AD93E}");
                public static readonly ID Video = new ID("{7787AA4A-D1A2-4614-889B-BC9F4695C54E}");
            }
        }

        public struct FourStepsCarousel
        {
            public static readonly ID ID = new ID("{CB44D591-101E-455B-AB5D-629F01112512}");
            public struct Fields
            {
                public static readonly ID HeaderText = new ID("{98E756EC-B53A-44E0-9F8D-16CC36B7A53C}");
                public static readonly ID BottomText = new ID("{5CC48F30-BD75-407A-81F3-B33384ADC5C9}");
                public static readonly ID Step1 = new ID("{1E762114-9CA2-44B3-A5F3-B443C6A21B42}");
                public static readonly ID Step2 = new ID("{98C3A5BD-65F0-41EE-A65F-7390F88791E7}");
                public static readonly ID Step3 = new ID("{228D453F-074A-400C-B433-96FB677E2B8E}");
                public static readonly ID Step4 = new ID("{65960079-9A43-4D63-A5C2-9D6F51FB5A23}");
            }
        }

        public struct TestYourKnowledge
        {
            public static readonly ID ID = new ID("{84A86B14-1015-424D-8892-CDD127C291B6}");

            public struct Fields
            {
                public static readonly ID HeadLine = new ID("{18B359B8-8908-425E-ADDB-2982E282CDBE}");
                public static readonly ID Question = new ID("{F39A7712-332C-4963-BB37-DBD359B19390}");
                public static readonly ID SubmitButton = new ID("{AA26C5BF-C478-4E4D-9F28-93C07BCB0B47}");
                public static readonly ID QuestionImage = new ID("{B715B6BB-3528-4755-A16A-33FAAC7954D9}");
                public static readonly ID ConfirmationImage = new ID("{5517B683-643A-4105-887A-D06BA2DABB5B}");
                public static readonly ID ConfirmationText = new ID("{199F781B-7F31-4524-AA11-1AAC82CDF5E9}");
                public static readonly ID ConfirmationLink = new ID("{3F9E6499-1AED-4AFF-AC25-1883FCE8DD92}");

                public static readonly ID Option1 = new ID("{4F4E2ACA-6FF5-4906-9C76-F0D7B32B6FA4}");
                public static readonly ID Option2 = new ID("{3F56423D-9766-4B6D-9181-E44D5B4A44CE}");
                public static readonly ID Option3 = new ID("{6594AA45-D647-477A-A666-1B02A1A99070}");
                public static readonly ID Option4 = new ID("{F95F9C97-A866-48A6-BE43-B550BA14CB42}");
                public static readonly ID Option5 = new ID("{0190EB0B-97D8-4F39-B6D6-8149067C4431}");
                public static readonly ID Option6 = new ID("{BA4B609E-EC5F-4564-B65D-BC5DEB47A9BC}");
                public static readonly ID Option7 = new ID("{9A736C10-F40A-49DB-BCC1-3E065547EE25}");
                public static readonly ID Option8 = new ID("{F8690059-2679-4217-B393-5E52A9F943BE}");
                public static readonly ID Option9 = new ID("{FB7E250A-4100-4F08-8D63-CEEA096FA33B}");
            }
        }

        public struct UnionSavingsCalculator
        {
            public static readonly ID ID = new ID("{F5134472-BBD7-4948-AD8B-39202C49C3EB}");
            public struct Fields
            {
                public static readonly ID Description = new ID("{9AFF6F4C-96BD-496C-9A83-7594A29EF310}");
                public static readonly ID Image = new ID("{439294E8-3AB8-426C-8263-F9FD00FD9A17}");
                public static readonly ID ButtonText = new ID("{5B792F13-C9D9-4429-B452-1EA77F1784B8}");
                public static readonly ID HeadlineText = new ID("{B455CEC7-9C28-4D2A-9ABD-8DD07D769A48}");
                public static readonly ID ButtonTextStep2 = new ID("{1973C952-1E61-4DAB-9B9D-5BF17F54B9CC}");
                public static readonly ID PreResultText = new ID("{6ABAD63D-D920-498F-8302-96CD80B74C07}");
                public static readonly ID PostResultText = new ID("{38FA9EC7-9824-45F4-B43F-9AEBB24650FD}");
                public static readonly ID ResultDescription = new ID("{32D7EB3E-4FD1-41E4-932E-ACFB3F09EB91}");
                public static readonly ID BackButtonText = new ID("{6018ABCB-CE07-436C-AF98-1B467B2543A9}");
            }
        }
        public struct SavingsCalculatorProduct
        {
            public static readonly ID ID = new ID("{EE33F874-C4F0-4994-B9B8-CDCF0F701BF6}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{3843AC7D-DC3B-4190-AED0-1A253C3F0EE0}");
                public static readonly ID Icon = new ID("{51A286F0-6AFA-46EF-865D-0D9919CCBFFB}");
                public static readonly ID Description = new ID("{6B1EBD74-BBC3-4069-821A-49DB27F21B33}");
                public static readonly ID Link = new ID("{EC39D41D-4BEC-473A-BE62-C9F4D69B8C7D}");
                public static readonly ID Saving = new ID("{63042156-133C-4E50-9870-618F854D5A25}");                
            }
        }
        public struct InteractiveQuestion
        {
            public static readonly ID ID = new ID("{31A640A6-C438-4A45-9834-8622BB65B5D1}");
            public struct Fields
            {
                public static readonly ID Imagen = new ID("{ABF7AF0E-055B-4C7B-B086-C965850C2FCC}");
                public static readonly ID Question = new ID("{FC7D0509-EA9E-4D0D-BBFA-1CC1DA56A64A}");
                public static readonly ID NoButton = new ID("{1A4D4108-CB5C-4E45-9933-50BD9B3919FA}");
                public static readonly ID YesButton = new ID("{71E98276-48DB-4F52-9096-D07C096F7940}");
                public static readonly ID BackButton = new ID("{8ADB0F03-0091-494F-9D0C-A226AAD963C7}");
                public static readonly ID IsLastQuestion = new ID("{918BB2FE-9C30-4D63-AF34-A9CE81FDD6FC}");
                public static readonly ID NextButton = new ID("{84579C8C-23E8-47CA-8BA9-410318C725A1}");
                public static readonly ID ShowResultLink = new ID("{6062AD26-CC51-40BF-8294-48C7684DB302}");
                public static readonly ID ShowResultText = new ID("{829B095E-6FA2-44FF-9B40-795CC692E17F}");
                public static readonly ID DisclaimerText = new ID("{A8C9F213-3916-4128-985E-ADACDEFD862D}");
                public static readonly ID HeadlineText = new ID("{A8B909D0-A66E-4D7E-9E12-F3ACC2FEB1DE}");
                public static readonly ID TipText = new ID("{FFEB3D18-5D44-40C8-8C94-7C6AD03A4070}");
                public static readonly ID HeadlineTextNegativeAnswer = new ID("{72D061AF-D69D-454B-BB1C-C62E7FF3F537}");
                public static readonly ID TipTextNegativeAnswer = new ID("{DB946BA7-1164-4374-B132-41B135ADC740}");
                public static readonly ID DisclaimerTextNegativeAnswer = new ID("{639085FD-E3CC-458C-A717-0ADC46960608}");
            }
        }
        public struct InteractiveQuestionnaire
        {
            public static readonly ID ID = new ID("{31A640A6-C438-4A45-9834-8622BB65B5D1}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{F067FD48-388C-42C9-BCC4-0EA78AC22FBD}");
                public static readonly ID Subheadline = new ID("{B58A59C6-6DE5-4E31-9D9A-1C12D851EA6C}");
                public static readonly ID BeginningButton = new ID("{44274A3A-A032-4628-A2C6-AF8F2A18CAC6}");
                public static readonly ID Image = new ID("{B5AA3B00-C38A-4CFA-B735-C934581BF84A}");
                public static readonly ID Logo = new ID("{056F1053-6C94-4869-AE10-3E5FB3CF882B}");
                public static readonly ID RetakeLinkText = new ID("{415571EE-E34C-41B1-8685-958B28758446}");
                public static readonly ID ResultScore = new ID("{45E8197E-97BC-4AFD-AA8B-5FB2657E47A9}");
                public static readonly ID PositiveDisclaimer = new ID("{23FB3560-208C-4426-8F60-5D475B8ECE8D}");
                public static readonly ID PositiveResultHeadline = new ID("{EC62EB09-B7BB-4BA1-ACE9-175F634D50D9}");
                public static readonly ID PositiveResultSubheadline = new ID("{AA8FF98C-8F7A-460B-8F30-4A778DA12244}");
                public static readonly ID PositiveResultLink = new ID("{8445E40D-E969-4804-BD6C-E196A76E97DF}");
                public static readonly ID PositiveResultImage = new ID("{A7BD535E-E38F-4F2E-AB85-2200D1C03807}");
                public static readonly ID NegativeResultHeadline = new ID("{1162CD1A-36B6-4CEA-BC44-7223CA3711D3}");
                public static readonly ID NegativeResultSubHeadline = new ID("{C159EE63-EDDF-4F33-A395-404E6B68B9A7}");
                public static readonly ID NegativeResultLink = new ID("{ABB1FAE6-B028-4170-9B18-6409E98DF8F7}");
                public static readonly ID NegativeResultImage = new ID("{6E6006BF-DDFF-4E67-B551-88BEAE8FFC65}");
                public static readonly ID NegativeDisclaimer = new ID("{2833D271-7A32-473B-A4BC-76B843A1ACC8}");
            }
        }

        public struct DebtConsolidationCalculator
        {
            public static readonly ID ID = new ID("{12BD7A71-95DC-4408-BEA1-FFC9166E3518}");
            public struct Fields
            {
                public static readonly ID Introduction = new ID("{545A4C9E-0202-4653-81F4-5B4C215824F8}");
                public static readonly ID LoanLabel = new ID("{D5570EDA-1152-4593-B7F4-80CC51C692C8}");
                public static readonly ID LoanToolTip = new ID("{AB99208C-A220-4B0C-8DFE-28908ED08F43}");
                public static readonly ID LoanMonthsLabel = new ID("{3F7BB3A1-C11A-4CD1-B063-4B97DF8289B6}");
                public static readonly ID OptionLabel = new ID("{699671DA-3876-4A28-A2DB-93688B75D940}");
                public static readonly ID LoanBalanceLabel = new ID("{7F07707D-C3C7-452D-B448-057DB9F688D5}");
                public static readonly ID LoanCurrentPayment = new ID("{ECF53293-6CF2-465C-A972-D5F9D8EB3AF1}");
                public static readonly ID LoanAprLabel = new ID("{950B41F2-6B1B-42F2-BC61-DDCF4FB31FD1}");
                public static readonly ID AddLoanLink = new ID("{5C554AEB-43DE-411C-8212-BA8BF77EB9DE}");
                public static readonly ID TotalLoanIcon = new ID("{F5BE4DC4-6BAF-4AA5-A1CC-1696FCB1F371}");
                public static readonly ID TotalLoanHeadline = new ID("{171A8197-F6AD-4BC8-BDB2-9A7F91499A22}");
                public static readonly ID TotalLoanDescription = new ID("{DF6D9C23-E952-4411-A1E7-5F4A1023D926}");
                public static readonly ID TotalMonthlyIcon = new ID("{3A819FFA-5FDF-45E9-B141-DF14A7D4E164}");
                public static readonly ID TotalMonthlyHeadline = new ID("{0B181E85-524F-4FBD-8E92-52EEA852AAF9}");
                public static readonly ID AverageAprIcon = new ID("{90C94FAE-3130-4EF4-A500-CC4861BB9BCB}");
                public static readonly ID AverageAprHeadline = new ID("{D378FDF9-120D-4CAC-BF4C-D52ACBA7F24E}");
                public static readonly ID ExploreButton = new ID("{EAF01E46-AF2B-4680-9FA2-55BFD7004EDF}");
                public static readonly ID ExploreOptionsDescription = new ID("{E83CAA5C-EB7D-402F-9204-711622001FEF}");
                public static readonly ID ExploreOptionsAprLabel = new ID("{3334D4E5-E482-44BC-A149-136C2E65C0CF}");
                public static readonly ID ExploreOptionsTermsLabel = new ID("{1D97B81A-3C6A-46D6-BCC5-D021E4C467BA}");
                public static readonly ID ExploreOptionsEstimatedCostLabel = new ID("{E6202B86-EF7B-4E68-A095-F607CA14A8B9}");
                public static readonly ID ExploreOptionsEstimatedCostToolTip = new ID("{6A23B445-2CEE-42ED-B744-C02DA0E2E8D6}");
                public static readonly ID ExploreOptionsCurrentPaymentLabel = new ID("{D0FDF158-DD00-43F9-BA9C-367CAEBD9B07}");
                public static readonly ID ExploreOptionsFooterNote = new ID("{DF1475BB-A3AE-4E1D-AE18-6DEED7D6D1F1}");
            }
        }
        public struct ImageTextBlock
        {
            public static readonly ID ID = new ID("{F732DFD4-5BFB-44C3-8581-81EB282F0EDF}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{F8B4F024-F33E-4D72-B8B1-9E94E9B43F41}");
                public static readonly ID Image = new ID("{F5DF54C0-B219-4549-851F-01A8135BD240}");
                public static readonly ID Text = new ID("{3373F988-F814-41D7-A00D-A053F9D513A7}");
                public static readonly ID Link = new ID("{DC6565AA-F6B5-41A2-A66C-52E0FC949366}");
                public static readonly ID LinkStyle = new ID("{16636CC8-A3F8-44CC-9DDE-B4F3A7B782E2}");
            }
        }
    }
}