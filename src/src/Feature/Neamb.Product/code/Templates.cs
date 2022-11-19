using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Product
{
	public struct Templates
	{
		public struct ProductCTAs
		{
			public static readonly ID ID = new ID("{CDCEBEEF-02EB-4CF2-86A3-41BC0D31F613}");

			public struct Fields
			{
				public static readonly ID Name = new ID("{D4FDCA7E-7BD4-41DF-A4EC-C0C141341670}");
				public static readonly ID SubHead = new ID("{9B8CD808-D1BD-4B30-90AF-F469DDC93604}");
				public static readonly ID Image = new ID("{D72ECFC6-977B-4D68-BFF7-447A28C979D3}");
				public static readonly ID Highlights = new ID("{E9B0F95E-E944-4541-9D24-635FC272322E}");
				public static readonly ID PartnerHeadline = new ID("{53011B49-6124-429B-8EBC-4E072D2926BE}");
				public static readonly ID PartnerAttribution = new ID("{2A6751AC-8237-4455-9CB4-366517F881B0}");
				public static readonly ID FinePrint = new ID("{5EFF42EB-FF4C-4456-B268-B82504B829AC}");
				public static readonly ID Eligibility = new ID("{D0B9285B-EF9B-4F38-B718-F888996F88BD}");
				public static readonly ID ComingSoon = new ID("{4040EB4A-616E-4D6E-8ED8-0B24437507B6}");
				public static readonly ID AnonymousCTA = new ID("{544FB930-A3F1-48AC-BAF3-F1D8BBE819DB}");
				public static readonly ID AnonymousCTAButtonColor = new ID("{D63C1BEA-0571-4063-A2C3-1CD9BDC8F1FB}");
				public static readonly ID LoginDetails = new ID("{48054719-9807-4282-90C2-4223823E69A1}");
				public static readonly ID ReminderCTA = new ID("{CF0926AB-8B16-4C35-8DD7-D942519D8C2C}");
				public static readonly ID ReminderDetails = new ID("{E37C684A-F980-4372-B566-5E4349648E8E}");
				public static readonly ID ReminderSet = new ID("{A3A184C1-207A-45EC-A516-40104A3C7976}");
				public static readonly ID PrimaryCTAType = new ID("{31379DF6-1740-444F-A62B-D926EA9511EC}");
				public static readonly ID PrimaryCTALink = new ID("{43F51A1D-D4E9-49AC-9BE6-C20D4F612785}");
				public static readonly ID PrimaryCTAButtonColor = new ID("{F3557637-9181-4934-A90C-FDADAA671698}");
				public static readonly ID PrimaryPostData = new ID("{A7D8B68E-BD55-4921-A3A5-C7672AEB341F}");
				public static readonly ID SecondaryCTAType = new ID("{07E61B1D-23EA-4A2E-8188-BB0277C90E47}");
				public static readonly ID SecondaryCTALink = new ID("{B3A3335E-0481-48C8-B942-568E93776F51}");
				public static readonly ID SecondaryCTButtonColor = new ID("{99FB106B-EC54-4B37-9D3E-0CB396369598}");
				public static readonly ID SecondaryPostData = new ID("{C4F155C2-735E-49C9-9EDB-4ACA334C5470}");
				public static readonly ID Notification = new ID("{87A78D4A-6B5B-4385-934D-2C1E5882000B}");
				public static readonly ID EligibilityDetails = new ID("{D89DAB59-FBE1-4179-94BE-2BB70CA8517A}");
				public static readonly ID Eyebrow = new ID("{27E8BA31-6953-411D-9F7E-E578D38A74EF}");
				public static readonly ID Category = new ID("{AD8E3BA4-0806-4278-B131-DFFA69F84BC3}");
				public static readonly ID SubCategory = new ID("{9696566F-F13B-4CCE-B532-E94BFC8C2227}");
				public static readonly ID SubGroup = new ID("{F4A6A478-CD21-4E0F-ADA0-B3E065D55EC2}");
				public static readonly ID Flag = new ID("{EDB93386-6E27-4B1E-9958-E883BA60C462}");
				public static readonly ID IsOmni = new ID("{61D27059-6217-4160-B479-7D741007F554}");
				public static readonly ID GoalTriggerPrimary = new ID("{1B9A2A00-62C3-4F67-8F68-FE8D9BF0B925}");
				public static readonly ID GoalTriggerSecondary = new ID("{904C25D4-43CE-4A61-AD84-866FAA37D1DD}");
				public static readonly ID ProductCodeDroplink = new ID("{D7125B4C-E4AA-4C56-A7E2-A5BC2369B88B}");
				public static readonly ID ProductContactDetails = new ID("{3432881D-01C5-4D40-9F8A-9868B049C926}");
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

		public struct SiteSettings
		{
			public static readonly ID ID = new ID("{C7EADD3C-19BC-463B-B0CC-A862E99E5B50}");

			public struct Fields
			{
				public static readonly ID Chat = new ID("{5FBA9701-F6A5-4753-AA15-57E33DE365DD}");
				public static readonly ID Phone = new ID("{F949BC80-EED6-475F-82CB-EC62F180C3B0}");
				public static readonly ID Email = new ID("{E0A29AB9-1AB2-425A-B3C0-5EB830B25514}");
				public static readonly ID SignIn = new ID("{56D767AD-7C8E-4837-B5E1-9DB1FA136B6E}");
			}
		}

		public struct ProductCategoriesAttributes
		{
			public static readonly ID ID = new ID("{0559E9DC-8D47-4F19-B3FC-FEB431BA0BB5}");

			public struct Fields
			{
				public static readonly ID ProductCategories = new ID("{E23A009C-D7B3-4202-B7A5-8C7DB3F4290D}");
			}
		}

		public struct PageCategoryItem
		{
			public static readonly ID ID = new ID("{A40A2FA8-803C-4BEB-87A3-5CF0BF0BDEA4}");

			public struct Fields
			{
				public static readonly ID Value = new ID("{96848D32-0918-446D-B4DB-97124E8E1339}");
				public static readonly ID Page = new ID("{B8A8ED31-E66B-42A3-A333-326A1C118AB4}");
			}
		}

		public struct _Product
		{
			public static readonly ID ID = new ID("{9D5243AD-9807-4335-9777-C9E0419BFE77}");

			public struct Fields
			{
				public static readonly ID TermsAndConditions = new ID("{2BEC068C-14CB-41BA-92C7-C9F871B33EA1}");
			}
		}

		public struct Partner
		{
			public static readonly ID ID = new ID("{0175A916-9B04-4F6C-BD8F-D9E2EF4AFB18}");

			public struct Fields
			{
				public static readonly ID Logo = new ID("{78C54E22-B4DF-4CA0-9B70-ED66A357CD6A}");
			}
		}

		public struct LoginPage
		{
			public static readonly ID ID = new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}");
		}

		public struct PageInfo
		{
			public static readonly ID ID = new ID("{367B8E27-D435-49A7-BA34-5D8F44FC1EB8}");

			public struct Fields
			{
				public static readonly ID PageTitle = new ID("{F71F7747-F88D-499B-AC69-D3A6DC9B0A88}");
			}
		}

		public struct ProductPage
		{
			public static readonly ID ID = new ID("{D1889EB8-BE95-4E99-B8E9-3A0AEB8F4800}");
		}

		public struct ProductAnchoredHeader
		{
			public static readonly ID ID = new ID("{38717512-639E-4403-8DE2-FD78F567AA12}");

			public struct Fields
			{
				public static readonly ID Title = new ID("{91AB2F85-3C8C-476F-92D9-4665B8FE4C67}");
				public static readonly ID AnchoredNavigation = new ID("{51BBAACB-D2BA-4679-9BEB-BA35191F7E20}");
			}
		}

		public struct LandingPageCta
		{
			public static readonly ID ID = new ID("{3A159950-8BA8-4164-A465-B25C37A696CD}");

			public struct Fields
			{
				public static readonly ID Eyebrow = new ID("{73DA7401-76AF-43F5-9B88-CE1AD1F858DA}");
				public static readonly ID Name = new ID("{726D32E5-8648-495F-B7C2-B501ADA2CACF}");
				public static readonly ID Subhead = new ID("{7AA5219B-B2B8-4C2D-95C4-8710B3D63A88}");
				public static readonly ID PartnerAtributtion = new ID("{4D755B73-F802-476A-8F0D-0FD5D3CFDC8D}");
				public static readonly ID TermsAndConditions = new ID("{C25D0CB0-FCBC-4420-A34B-ED195FD0A69B}");
				public static readonly ID Placement = new ID("{EF646AE3-6438-4461-935E-FB30908FD95C}");
				public static readonly ID Image = new ID("{DE957504-99EE-494C-AE14-871AD0BB90D7}");
				public static readonly ID Video = new ID("{7EE70AC5-DB4C-437B-BC18-944983B48DB7}");
				public static readonly ID CtaLink = new ID("{1014EB0B-613F-4F30-B828-DAE6A6A967F9}");
				public static readonly ID FinePrint = new ID("{8C38FF5F-196B-4E53-952A-1A29A15618B3}");

				public static readonly ID ProgramCode = new ID("{182160D2-B7B0-4CFC-A894-A23D86A02186}");
				public static readonly ID SweepstakeId = new ID("{6260FC2F-B6BA-4186-B5E4-01B2FDAE12FB}");
				public static readonly ID Cellcode = new ID("{EF4758BD-B1ED-4886-AD8A-CA2B9829C22C}");
				public static readonly ID Campaigncode = new ID("{6CBECBB2-1374-4A50-8F14-ACAC4920606C}");
				public static readonly ID Acknowledgement = new ID("{495AC4CA-A2C6-4A1F-BDB8-2687D8119595}");
				public static readonly ID AcknowledgementText = new ID("{15DCD7C4-056B-4553-9E4B-94DC764FA445}");
				public static readonly ID ShowContactInfo = new ID("{24D1E8E6-FE3D-4DC3-8C08-6AF42CD592E1}");
				public static readonly ID ContactInfoText = new ID("{689AA09D-A929-45AB-B744-12EC75515262}");
				public static readonly ID Eligibility = new ID("{8AA6CAE7-1E5A-4929-80A4-79BD58783812}");
				public static readonly ID Login = new ID("{E494A4F2-BAA2-4D8A-84B5-813994C956F8}");
				public static readonly ID LoginDetails = new ID("{A02D086C-F869-4B1B-B50E-43356AF241E8}");
				public static readonly ID NotEligible = new ID("{2AAA0C63-E24B-4BB1-A921-313C3B328BDF}");
				public static readonly ID Thankyou = new ID("{31FD9495-A12E-49F9-A5D8-24E61D93E29E}");
				public static readonly ID Error = new ID("{AB261FF6-73C9-4F27-961D-F2EEA898F273}");
			}
		}

		public struct TextPlacements
		{
			public static readonly ID Left = new ID("{B77751D5-7900-4378-898A-9C1607A84B30}");
			public static readonly ID Right = new ID("{0424CED8-4C06-48AB-A255-3A990E87DFC7}");
		}

		public struct MultiRowOffer
		{
			public static readonly ID ID = new ID("{90E39B98-2ED9-4977-B531-85DED386F80B}");

			public struct Fields
			{
				public static readonly ID Header = new ID("{81D59DB1-E31A-4129-A145-1CFACED5C4A7}");
				public static readonly ID Subhead = new ID("{F3690BF5-5469-4DAF-90D5-26847904384D}");
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
				public static readonly ID ViewDetails = new ID("{EDCF1890-78C2-4A78-BDEE-C9562A0A4E72}");
			}
		}

		public struct SpecialOffer
		{
			public static readonly ID ID = new ID("{C23718EB-13D3-417A-80EB-A8942BE2B236}");

			public struct Fields
			{
				public static readonly ID Name = new ID("{F3102F8C-2F2F-4648-8682-EC88571C0718}");
				public static readonly ID Subhead = new ID("{06D7A089-44FC-4B01-BBB7-ACD5EE3CBE0D}");
				public static readonly ID Image = new ID("{F83C5010-7F0B-4B55-9DF7-028BE79C8374}");
				public static readonly ID Link = new ID("{F023D359-4D14-46B7-B0FD-C7A82DC599D6}");
				public static readonly ID Type = new ID("{0347C813-3259-4A38-9745-F0B975AB0D49}");
				public static readonly ID FinePrint = new ID("{8C999BE6-D52E-4792-903D-D7A788F76780}");
				public static readonly ID Eligibility = new ID("{D3939DAB-3D2C-4AC9-99F8-1F22F9D82387}");
				public static readonly ID Cta = new ID("{B544FBAA-7200-49B6-A80E-E6A550A40863}");
				public static readonly ID LoginDetails = new ID("{4920407D-E0F3-452F-A769-B7FD09B22BB2}");
				public static readonly ID EligibilityDetails = new ID("{1EDC40E1-3E5C-4F20-B608-A2BA6CDFEBCA}");
				public static readonly ID ComingSoon = new ID("{268984E2-3728-4222-B8CA-9068561AE3DF}");
				public static readonly ID ReminderCTA = new ID("{34221F91-E371-4FC0-A3BE-570F7F41561B}");
				public static readonly ID ReminderDetails = new ID("{817BA51E-B65E-4B6E-BD69-3186C7203B84}");
				public static readonly ID ReminderSet = new ID("{56D1A132-BEFE-4559-9242-1898AF3246C2}");
				public static readonly ID PostData = new ID("{36052EFB-7C06-4AF1-AC5B-9BB368D893A9}");
				public static readonly ID Goal = new ID("{85933895-5CEE-4F92-86D1-DAB1DC28339E}");
				public static readonly ID ProductCodeDroplink = new ID("{C281C8C8-E549-47DD-A707-5099B2B28C22}");
			}
		}

		public struct GuideCta
		{
			public static readonly ID ID = new ID("{34DCBCF0-4249-406F-BAB8-0320CE867767}");

			public struct Fields
			{
				public static readonly ID Name = new ID("{EC18CB4E-04F7-4507-B746-86473CF5D30F}");
				public static readonly ID Description = new ID("{11E7A37C-E88A-4FD7-8514-E57DF5DD33C8}");
				public static readonly ID Image = new ID("{E702943C-99B5-41C4-B728-BA230A64CFE2}");
				public static readonly ID MaterialId = new ID("{700EF1BF-C47B-4265-AB43-1801D462B86C}");
				public static readonly ID Cta = new ID("{019C75C9-9075-4936-91E8-36CC4469ED10}");
				public static readonly ID GeneralError = new ID("{A8B35A1B-D9FF-46C1-860C-23378A1CCF96}");
				public static readonly ID CtaLogin = new ID("{D4AC55C9-717A-45CA-857C-B7BEB041BD4D}");
			}
		}

		public struct MediaLibrary
		{
			public static readonly ID ID = new ID("{3D6658D8-A0BF-4E75-B3E2-D050FABCF4E1}");
		}

		public struct OfferLinkList
		{
			public static readonly ID ID = new ID("{9AB2B0E7-3BD7-4537-92A5-8E7F76351FD4}");
			public struct Fields
			{
				public static readonly ID Logo = new ID("{3E248098-A560-4431-9F53-16CBAAAAC4F1}");
				public static readonly ID LoginButton = new ID("{E1B48DEF-7C54-447E-8B71-8038A04E823E}");
				public static readonly ID Headline = new ID("{C4114479-2F5B-4CE0-97D6-0D4188729132}");
			}
		}

		public struct Reminder
		{
			public static readonly ID ID = new ID("{E09011DE-20E8-4400-B5E7-92F2C9F9348F}");
			public struct Fields
			{
				public static readonly ID Cta = new ID("{CF0926AB-8B16-4C35-8DD7-D942519D8C2C}");
				public static readonly ID ReminderDetails = new ID("{E37C684A-F980-4372-B566-5E4349648E8E}");
				public static readonly ID ReminderSet = new ID("{A3A184C1-207A-45EC-A516-40104A3C7976}");
				public static readonly ID ComingSoon = new ID("{4040EB4A-616E-4D6E-8ED8-0B24437507B6}");
			}
		}
		public struct Step
		{
			public static readonly ID ID = new ID("{D008261B-FB78-40F4-B7C5-CCB5F8CE0261}");
			public struct Fields
			{
			}
		}
		public struct NewsletterStep
		{
			public static readonly ID ID = new ID("{EE59E60C-4880-4F20-9788-BC6BD90CE130}");
			public struct Fields
			{
				public static readonly ID Enabled = new ID("{BFA9BBEC-9145-4988-910E-C4618BCA7C1E}");
				public static readonly ID Newsletter = new ID("{CC989A4C-05EE-484D-9936-5B0201D933FA}");
			}
		}
		public struct Newsletters
		{
			public static readonly ID ID = new ID("{C41CF1EE-DF4D-48E7-AC84-2E634BEC279E}");
			public struct Fields
			{
				public static readonly ID Id = new ID("{55DE5632-19E9-4C6F-B983-4770542AB84F}");
			}
		}
		public struct Sweepstakes
		{
			public static readonly ID ID = new ID("{E0470041-0F50-43DD-9414-A242B051A4C4}");

			public struct Fields
			{
				public static readonly ID HideCTAButton = new ID("{7ECDD3DD-2576-4188-9632-F0FF951BA8E5}");
				public static readonly ID SendEmailNotification = new ID("{FDF37AEA-86E1-4EAE-A546-7EA6E6EA8AD1}");
			}
		}
		public struct RetirementSeminarCta
		{
			public static readonly ID ID = new ID("{57AF696E-73B6-41AD-9494-30A39BF48667}");

			public struct Fields
			{
				public static readonly ID Seminar = new ID("{942C9D25-8D1C-41EA-94D7-1FCC778C195A}");
				public static readonly ID RegisteredUserMessage = new ID("{B7F8583B-1988-4F64-A4EB-EEC81726EAED}");
				public static readonly ID InvalidSeminarErrorMessage = new ID("{E0CC1D08-7CB3-48FA-B3AA-D2C7AD391AEA}");
				public static readonly ID InvalidSeminarNotification = new ID("{905F2E9B-790E-4C0E-B50B-DD144BBF8500}");
				public static readonly ID NotEligibleNotification = new ID("{CC82A459-94DB-4FCA-9D92-F4042C9E05AC}");
			}
		}
		public struct ProductCtaLite
		{
			public static readonly ID ID = new ID("{5FF6A507-7D14-4EE5-B85B-12082E2C85D7}");
		}
		public struct ProductRedirect
		{
			public static readonly ID ID = new ID("{3D72C5CE-6691-4E51-AEFC-59D1C19CF688}");

			public struct Fields
			{
				public static readonly ID ErrorPage = new ID("{011E4945-4005-480E-ACA2-75708BFAAA5E}");
			}
		}
		public struct HomePage
		{
			public static readonly ID ID = new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}");
		}

		public struct CarouselOfferCards
		{
			public static readonly ID ID = new ID("{3E34AD99-8375-4C2E-A165-6EA028F29ACB}");

			public struct Fields
			{
				public static readonly ID Cards = new ID("{DB1D0C0F-D462-4EC2-8D56-FA4C60F8FC28}");
			}
		}

		public struct CarouselOfferItem
		{
			public static readonly ID ID = new ID("{5D6F46E9-A502-4293-9258-8AF63AB0214C}");

			public struct Fields
			{
				public static readonly ID Title = new ID("{72B4F734-F12C-4ABF-BAD7-4139E2F24A09}");
				public static readonly ID PromotionalText = new ID("{ACDBB338-867C-4022-976D-58E460447E10}");
				public static readonly ID Image = new ID("{218A7191-F228-41D6-B7D8-2EFF359F9A9F}");
				public static readonly ID ButtonColor = new ID("{7EED079F-7FAF-438A-8EF3-0B06EB998060}");
				public static readonly ID Description = new ID("{BCA7B2E9-2A71-4EB4-81C8-4401EDE3B5BC}");
				public static readonly ID EligibilityDetails = new ID("{0210FF04-FE1C-4FA8-AEF9-E816BF249204}");
				public static readonly ID Logo = new ID("{927CE05B-9515-4803-8259-29F46C1D66C3}");
			}
		}
		public struct OfferLinkItem
		{
			public static readonly ID ID = new ID("{9889BA7D-6BC0-4794-BD3D-96597829A762}");

			public struct Fields
			{
				public static readonly ID RequiresLogin = new ID("{E02093C8-48DD-4F23-8DEA-15736C5D8170}");
			}
		}
		public struct ProductMultiOffer
		{
			public static readonly ID ID = new ID("{4702F4F0-06A9-4504-914D-9BD50CC1ADCB}");

			public struct Fields
			{
				public static readonly ID Title = new ID("{873DBC70-4B08-4B21-A56A-ACC8A04B933F}");
				public static readonly ID FooterNote = new ID("{D2BD9B38-CC39-4A84-B0C7-89C8824878B3}");
				public static readonly ID CancelButton = new ID("{CE6C72AB-B3BF-45D7-86C3-6A6FC66412DF}");
				public static readonly ID ProductMapping = new ID("{C4018BF9-8F3B-4C6C-9854-22F9C703AC18}");
				public static readonly ID TargetUrl = new ID("{51DC495C-959A-479E-89A6-90A30DAC1C4E}");
				public static readonly ID PostParamToken = new ID("{D090981F-D000-491F-8850-0B752D16A080}");
				public static readonly ID RadioGroups = new ID("{2233875B-3C4C-4793-9A73-202C100A179B}");
				public static readonly ID ErrorMessage = new ID("{F8D2CD47-99DB-4EE2-8224-586A10A50949}");
			}
		}

		public struct PmoRadioButtonGroup
		{
			public static readonly ID ID = new ID("{14B78E07-CCB3-43C4-ABB0-A131E07469D5}");

			public struct Fields
			{
				public static readonly ID GroupDescription = new ID("{3B3EBED7-13D2-4B68-9436-7B19EC40BB72}");
				public static readonly ID GroupParameter = new ID("{81FF9606-FC92-4D70-A614-61E18E2DCB79}");
			}
		}

		public struct PmoRadioButtonOption
		{
			public static readonly ID ID = new ID("{06047265-16FE-480B-B52D-5B304E2F71F1}");

			public struct Fields
			{
				public static readonly ID RadioDisplayText = new ID("{6094197C-B365-410A-8D96-73A18845732E}");
				public static readonly ID RadioValueText = new ID("{10CCE255-FCB0-4FF7-AC49-29EA4F522CAE}");
				public static readonly ID DefaultSelection = new ID("{C1719EA6-0193-453D-8AAA-C112D8BD2E89}");
				public static readonly ID ParameterId = new ID("{825D85DD-5EAB-4CDD-A3AF-B343050898D8}");
				public static readonly ID ParameterMatchValue = new ID("{CCF9E49E-05FA-41A7-BF80-C1025DB733D1}");
			}
		}
		public struct ProductMappingOption
		{
			public static readonly ID ID = new ID("{8DBA9459-048C-4B21-AF17-B7B23C190275}");

			public struct Fields
			{
				public static readonly ID CalculatedProductId = new ID("{924B2E00-9A3A-4227-8558-2C55201A31A4}");
				public static readonly ID ResultProductId = new ID("{5861FCD8-0924-45F6-9EF5-626CD5756629}");
				public static readonly ID ProductParams = new ID("{7CEC3A4E-C33D-4F3D-B9C7-4361C74D6381}");
			}
		}


		public struct ProductSubMenu
		{
			public static readonly ID ID = new ID("{3C13505A-F69B-43A2-8B38-02FBC951CA34}");

			public struct Fields
			{
				public static readonly ID CTALink = new ID("{F8AAD234-AF61-448A-A58F-A1726F08BE3D}");
				public static readonly ID Links = new ID("{BFBA9DA8-1D80-4D9B-A5EC-587DDC4B7BB9}");
			}
		}

		public struct RetirementProduct
		{
			public static readonly ID ID = new ID("{A6DB2D17-7A1B-46C0-90F7-67D78F4D1E32}");

			public struct Fields
			{
				public static readonly ID Name = new ID("{7ED63802-7AB1-468F-9F8D-E272A26276D2}");
				public static readonly ID Subhead = new ID("{C98A7F9D-A227-4497-9A15-13E975DD0C69}");
				public static readonly ID CTALink = new ID("{E01CD945-5EFB-450E-975B-D55F5EA67495}");
				public static readonly ID Image = new ID("{6C5C54B4-AB4F-44ED-84C3-76CDF8CB63AA}");
			}
		}
		public struct ProductPageView
		{
			public static readonly ID ID = new ID("{5B510EBD-083A-4F46-BFB9-526ACFCABD17}");

			public struct Fields
			{
				public static readonly ID ProductPageView = new ID("{5B510EBD-083A-4F46-BFB9-526ACFCABD17}");

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