using Sitecore.Data;

namespace Neambc.Neamb.Feature.Account {
	public struct Templates {
		public struct Login {
			public static readonly ID ID = new ID("{A0386F34-2FCF-48C8-9F0A-1956B5AE244E}");

			public struct Fields {
				public static readonly ID Headline = new ID("{352607BB-C371-4A61-86E8-30FDAD841103}");
				public static readonly ID SubHeadline = new ID("{AFC8814A-0FC4-4FDA-B9EB-FECCE2FB1FA3}");
				public static readonly ID Email = new ID("{C8ABB7C9-E3E2-4561-98EA-D4B138D42663}");
				public static readonly ID Password = new ID("{698ACE5E-3A37-4AF6-B27C-C2B4E0091F21}");
				public static readonly ID Show = new ID("{766AD9B9-B3D8-47A7-A755-E22EB7E3E2A1}");
				public static readonly ID RememberMe = new ID("{0345E845-6A99-4BC0-BC97-90D14DBD72E2}");
				public static readonly ID ForgotPassword = new ID("{08A38CC8-59F7-428B-B0B9-429B01F49D5C}");
				public static readonly ID ForgotEmail = new ID("{FEBCA0A7-3B7B-4A5A-A661-6DADF3856C3C}");
				public static readonly ID LoginButton = new ID("{73395710-64D6-47FF-B4F1-ACF8804CB5EB}");
				public static readonly ID NeedAccount = new ID("{C7F3256A-1A87-4945-A037-31A0C84E7D5D}");
				public static readonly ID CreateAccount = new ID("{7A561853-BD43-43F4-8227-37D326BDC657}");
				public static readonly ID EmptyLogin = new ID("{1D4638D2-5CFC-4A4C-933D-009FA930EFF7}");
				public static readonly ID EmptyPassword = new ID("{66A3657F-37F8-414C-B221-242A53F9E082}");
				public static readonly ID InvalidEmail = new ID("{E91DEEC7-A652-494B-B82B-08CD3CBC39E8}");
				public static readonly ID InvalidCredentials = new ID("{E9F80966-33EA-4341-85C2-268509306555}");
				public static readonly ID AccountLocked = new ID("{04E387C3-9C54-4F0C-89C0-6F921DD74581}");
				public static readonly ID AccountAlreadyLockedTokenValid = new ID("{547DBDA3-2B92-4831-A5F4-514177C13A4E}");
				public static readonly ID AlreadyRegistered = new ID("{7D463E84-A0D6-4370-9DD7-DB5F22E65145}");
				public static readonly ID TimeOut = new ID("{BFDB3F79-2DE5-4DB6-B11C-EBFB4844BC1B}");
				public static readonly ID ResetLink = new ID("{D849B2DF-0C97-4542-A544-A7CD1F1E7999}");
				public static readonly ID Tooltip = new ID("{707BF8F9-68D6-4AFD-AE28-DE94EDC3268F}");
			}

		}

		public struct RegistrationForm {
			public static readonly ID ID = new ID("{493EF918-0228-4B59-AEC0-3B9FDA9A9DD9}");

			public struct Fields {
				public static readonly ID Title = new ID("{19024296-A6B7-465D-A847-E9A83EE43E74}");
				public static readonly ID Intro = new ID("{D156F621-EB4E-4C16-AF76-4044F592C623}");
				public static readonly ID Optin = new ID("{EDBF256B-2FAD-42D6-8988-F721A26A8425}");
				public static readonly ID Disclaimer = new ID("{9D2E4558-7692-4A9C-BA15-AEC10E11C74E}");
				public static readonly ID SubmitButton = new ID("{9A13B8DA-85CA-4476-8872-D2D792011720}");
				public static readonly ID Error = new ID("{0E3D003A-96CD-4ED7-BA86-FF7F7E9DF1A6}");
				public static readonly ID Success = new ID("{8D8A96C4-9097-42BF-AB13-D900529E1A47}");
			}
		}

		public struct RegistrationSteps
		{
			public static readonly ID ID = new ID("{83C9411C-6607-4BC2-AC8C-AEAF8B6A53A5}");

			public struct Fields
			{
				public static readonly ID Step1Title = new ID("{1808C5D8-8F6C-4CE2-833B-9F47E8DA400A}");
				public static readonly ID Step1Intro = new ID("{FD137F7D-9D44-4523-B678-B8136D580D0F}");
				public static readonly ID Optin = new ID("{1D854634-8800-49BF-AF31-EB8233A3423E}");
				public static readonly ID Disclaimer = new ID("{D0055A84-6B46-4E57-9180-4A3E135198C6}");
				public static readonly ID NextStepButton = new ID("{1A696F6F-3DAC-4185-93F7-C3D0BF6F5372}");
				public static readonly ID Step2Title = new ID("{AF096608-4272-4FEB-982B-BD06C7982731}");
				public static readonly ID Step2Intro = new ID("{8472D847-10D9-4F8B-A04E-87EDCB951DAC}");
				public static readonly ID SubmitButton = new ID("{95583438-533A-4423-86F2-E1A733B99486}");
				public static readonly ID Errormessage = new ID("{042C1E3B-833A-475D-B150-E6A79DBCE6C5}");
				public static readonly ID SuccessMessage = new ID("{929B275E-59F3-4A34-AAE6-79002B5948F5}");
				public static readonly ID NameSectionLabel = new ID("{DBBD8875-2E06-4A3F-AA54-5B2381D3AE8E}");
				public static readonly ID PasswordSectionLabel = new ID("{50B7BB71-BAE0-4893-8B5B-23FAC8863D7C}");
				public static readonly ID AddressSectionLabel = new ID("{933B3E91-C0CC-4220-97BA-6542AFD83460}");
				public static readonly ID BirthDateSectionLabel = new ID("{973D20DB-6AD3-43DA-8039-15C7048B8A89}");
				public static readonly ID Setp1DescriptionText = new ID("{D53243D3-5E33-49DE-AEAA-E01B364384DD}");

			}
		}

		public struct Profile {
			public static readonly ID ID = new ID("{4F62A9EF-BF7E-477A-AD9F-72B393BDDD83}");

			public struct Fields {
				public static readonly ID AddressLabel = new ID("{4108EE1D-0C10-4163-AE2A-23E2CC56D1C8}");
				public static readonly ID AddressTooltip = new ID("{497C0060-0523-4DB9-AE87-1D43D90C800C}");
				public static readonly ID AddressEmptyField = new ID("{3A0DBA9F-F047-4AC1-AEB9-DB4AAF046C1B}");
				public static readonly ID AddressCharacterLimit = new ID("{C2051581-7DF1-428B-AB7C-539B7FEA7ED4}");
				public static readonly ID AddressInvalidCharacters = new ID("{4C8FE107-2167-495F-B4A7-7577D07E087D}");
				public static readonly ID AddressMinimumCharacterLimit = new ID("{030268F3-ED8A-4C83-B41D-E3A4E4495DD2}");
				public static readonly ID BirthDateLabel = new ID("{1AA39C4C-E420-4765-AAE4-40AA43E9FCED}");
				public static readonly ID BirthDateTooltip = new ID("{4AC782BC-148E-4CBD-ABA7-34691B14A1B5}");
				public static readonly ID BirthDateEmptyField = new ID("{22D2494A-6FEE-453A-8508-FC8C350A7580}");
				public static readonly ID BirthDateAgeRequirement = new ID("{A89EAE5E-8360-4172-9A0C-0C7246C05873}");
                public static readonly ID BirthDateInvalidDOB = new ID("{1EAC71C5-3626-4326-A790-9A79E665C2FE}");
                public static readonly ID CityLabel = new ID("{C4B62C9E-911E-4915-889E-54F0E861EECA}");
				public static readonly ID CityTooltip = new ID("{26E3C345-1CA2-494A-A8C1-35DA5BBC9E7B}");
				public static readonly ID CityEmptyField = new ID("{BC45D3FD-EFCB-46B5-9669-3D75A396535F}");
				public static readonly ID CityCharacterLimit = new ID("{1327E79B-B86E-4709-A401-D31036B9ED5F}");
				public static readonly ID CityInvalidCharacters = new ID("{4E713C65-91D4-4D4C-9E71-E5AAFD95B4EB}");
				public static readonly ID CityMinimumCharacterLimit = new ID("{81F59FEE-BE1C-4216-BE33-33CFEB3A422E}");
				public static readonly ID EmailLabel = new ID("{A0789D92-22F3-4726-B79B-DD1C91EE4ECE}");
				public static readonly ID EmailTooltip = new ID("{341BA223-CC9B-4C18-8530-88D994393367}");
				public static readonly ID EmailEmptyField = new ID("{852633DC-5A7B-43F5-B533-B9A046E56563}");
				public static readonly ID EmailCharacterLimit = new ID("{38F547BF-95DD-4960-AB1B-6B2A8AC02B81}");
				public static readonly ID EmailInvalidFormat = new ID("{D71690EA-7C45-4AE3-AC9A-BAC5E5A04AAE}");
				public static readonly ID EmailInUse = new ID("{5D278223-E69F-4521-BB8E-E7B205308A69}");
				public static readonly ID EmailWarning = new ID("{678EB874-CB56-4A85-BC8D-64776A26B9FD}");
				public static readonly ID FirstNameLabel = new ID("{5B02F656-8C0E-49AA-A594-E89DD3333A1E}");
				public static readonly ID FirstNameTooltip = new ID("{E1E27FC7-3A6D-4F17-968F-91D67DBED460}");
				public static readonly ID FirstNameEmptyField = new ID("{4A08CFD7-853D-478F-8C99-B99E061B0F1D}");
				public static readonly ID FirstNameCharacterLimit = new ID("{9A1BB82C-82A2-4CA7-A5FE-8B6F09FE8B5C}");
				public static readonly ID FirstNameInvalidCharacters = new ID("{4BD97032-914A-493F-9CC1-5D1FD6476E90}");
				public static readonly ID FirstNameMinimumCharacterLimit = new ID("{A4F6A83B-5DE3-4035-BBC7-88823F4B61A7}");
				public static readonly ID LastNameLabel = new ID("{1E31CA98-49C0-417C-AAFD-191A6504089E}");
				public static readonly ID LastNameTooltip = new ID("{22297AE2-BDFB-4D6D-A9ED-61C2F60491CA}");
				public static readonly ID LastNameEmptyField = new ID("{9839A40C-EDA5-4C83-AB7F-75FF4748525D}");
				public static readonly ID LastNameCharacterLimit = new ID("{F7620F0C-1C3A-458E-AD51-A0F6395DCD2D}");
				public static readonly ID LastNameInvalidCharacters = new ID("{D164E8F4-3338-4EDA-8626-D0783A845DAC}");
				public static readonly ID LastNameMinimumCharacterLimit = new ID("{3F535FB5-4D08-485D-A712-72570256D4BF}");
				public static readonly ID PhoneLabel = new ID("{7CF2AE38-6F3A-4C72-A9E0-86C14DB843B7}");
				public static readonly ID PhoneTooltip = new ID("{F45A08F5-9C3F-4D9C-A8A6-64C028750688}");
				public static readonly ID PhoneCharacterLimit = new ID("{FFEE5BC4-EB6E-4B91-9DAA-AB02DDD6A7AF}");
				public static readonly ID PhoneInvalidCharacters = new ID("{A38D52FB-50BF-44EB-A03F-81AF27E6CBB7}");
				public static readonly ID StateLabel = new ID("{9F341C34-8072-4E55-B13A-532CBAA00B7A}");
				public static readonly ID StateTooltip = new ID("{A37B6D96-924F-4C6B-9F9D-EB000E6B2D77}");
				public static readonly ID StateEmptyField = new ID("{32ADEDB0-EA1E-43E1-B344-277E44CD5023}");
				public static readonly ID ZipLabel = new ID("{B458F507-6781-4954-87A9-09E56EB7CBAF}");
				public static readonly ID ZipTooltip = new ID("{BB3C416B-1517-434D-9D11-2DE62AE685AF}");
				public static readonly ID ZipEmptyField = new ID("{835467AE-A55F-45D8-993D-E33DDBB83B93}");
				public static readonly ID ZipCharacterLimit = new ID("{4B568AFC-F8D5-4EE8-893B-BE3C5F54E65E}");
				public static readonly ID ZipInvalidCharacters = new ID("{BD995065-8E92-4B97-826D-E25EA1078578}");
			}

		}

		public struct Password {
			public static readonly ID ID = new ID("{34242C35-3EAD-4F0C-AA93-CD115E2A0A99}");

			public struct Fields {
				public static readonly ID PasswordLabel = new ID("{0B09BB6D-CE81-48EF-AB96-7210C8485AF0}");
				public static readonly ID PasswordRequirements = new ID("{A02FB601-7A01-456B-8596-E4F1DE8076E3}");
				public static readonly ID Tooltip = new ID("{70E1DAD5-6AD1-4F97-AF6C-F4BC7A649948}");
				public static readonly ID PasswordEmptyField = new ID("{6EB22D48-7E0E-4FF5-999D-BD3355CC74E2}");
				public static readonly ID PasswordCharacterLimit = new ID("{70853990-1D72-4E87-82FA-010DD43A7DBB}");
				public static readonly ID PasswordShowText = new ID("{8D1FA09D-2C08-4DD6-B276-5B4D84B1067D}");
				public static readonly ID ConfirmPasswordLabel = new ID("{CEBCC101-0D3E-46E1-AA7E-BD197922D5F5}");
				public static readonly ID ConfirmPasswordInvalid = new ID("{3BD1C95E-74F7-43AB-A401-598465EEB132}");
				public static readonly ID ConfirmPasswordEmptyField = new ID("{6193FB54-3434-468F-A8C1-D70F29403F29}");
				

			}
		}

		public struct StatesGlobal {
			public static readonly ID ID = new ID("{1C0CF0BA-D674-418E-A807-72EF1BA9359C}");
		}

		public struct NameValueItem {
			public static readonly ID ID = new ID("{D1402B59-E079-4856-9DFB-551B6C87B7AE}");

			public struct Fields {
				public static readonly ID ItemValue = new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}");
			}
		}

		public struct ZipCodeVerificationForm {
			public static readonly ID ID = new ID("{8CBC4081-90EB-4385-9AAC-8A03F153FD72}");

			public struct Fields {
				public static readonly ID Title = new ID("{DA918EE6-F81B-489B-B5C6-C3ADE12D2F93}");
				public static readonly ID Salutation = new ID("{507A2550-5D67-48B2-9BEE-11BCE78B36EF}");
				public static readonly ID Instructions = new ID("{597BA0CD-608E-4A5F-853C-1E1B515E5122}");
				public static readonly ID Label = new ID("{EA87EEDD-B499-4EE5-B00E-026ABD6FB9D8}");
				public static readonly ID Next = new ID("{C0C93AB4-0084-48A1-A047-F30F345FFE0C}");
				public static readonly ID NotYou = new ID("{DD671C02-4E5E-4126-9225-A0F9EBBFA42C}");
				public static readonly ID Registration = new ID("{739FE8E8-1424-4766-B8E1-ACC4F05DC91C}");
				public static readonly ID Attempts = new ID("{4D214322-6F1C-4F4E-BE37-F92D681FB566}");
				public static readonly ID EmptyField = new ID("{46694BCE-32DA-46A6-A94C-761FA1007951}");
				public static readonly ID MatchNotFound = new ID("{1C764A84-74CD-4978-9A21-19CEACC23102}");
				public static readonly ID CharactersLimit = new ID("{FBDDF5D8-6355-4D73-9892-BC2B250DF9BB}");
				public static readonly ID Tooltip = new ID("{2F57D39E-4931-48C4-92F7-D7F7F2670CF6}");
				public static readonly ID EmptyFieldZipCode = new ID("{3BD6C766-F28E-4FBA-A3D3-04BFB5DF86D1}");
				public static readonly ID ZipCodeMinimunCharacters = new ID("{A9F5C898-80AB-4887-BBA9-89EC8098D54B}");
				public static readonly ID InvalidCharacters = new ID("{59B2EB1B-05B2-4C6C-A89F-B6603FE154F4}");
			}
		}

		public struct DuplicateRegistration {
			public static readonly ID ID = new ID("{64EAC7CD-B234-48A6-AF59-CA5E67F33A94}");

			public struct Fields {
				public static readonly ID Title = new ID("{5205D8E5-675C-4C42-811F-321E6668CAEE}");
				public static readonly ID Intro = new ID("{D6C19526-FDE1-4214-B9FF-CECE5422BA92}");
				public static readonly ID AccountInfo = new ID("{0AD721D3-7758-4424-B0D4-A1D1E81FF068}");
				public static readonly ID Explanation = new ID("{8768DCC9-A8E8-4293-9071-3DCE4D876A36}");
				public static readonly ID Send = new ID("{06311FC6-EA59-4F5E-84FB-509A2BA42606}");
				public static readonly ID Error = new ID("{0ABB22D2-1F99-44C3-8B41-BA10F4C69990}");
				public static readonly ID Loader = new ID("{73BCD9F3-4AB6-48E7-954F-EBFB54BCEADC}");
				public static readonly ID LoaderImage = new ID("{3F8A8EFC-CBD5-4419-891C-CD4E9BBAA26D}");
				public static readonly ID ApplyChanges = new ID("{3E48D9BC-216E-4C0D-8EE5-7200FE28C759}");
				public static readonly ID InvalidPassword = new ID("{B1C99FCB-FD9A-4B9A-8BFD-2AB068A55016}");
				public static readonly ID Password = new ID("{3774E5AB-C313-4240-892A-53B0F283944A}");
				public static readonly ID EmptyPassword = new ID("{2E44246D-FC18-48C5-836C-47B221E59CDA}");
				public static readonly ID SelectAnother = new ID("{C0FA6559-31DF-4A50-BDF0-309A89F3224C}");
				public static readonly ID SelectedEmail = new ID("{1BC0B974-1AAF-4F41-90FB-A370D757E466}");
				public static readonly ID UnselectedEmails = new ID("{B87283CD-A7BD-4E79-A4C3-903F71527ED5}");
				public static readonly ID ContinueButton = new ID("{DD6912B0-8A06-4BFE-AB83-0B8A81005B21}");
				public static readonly ID ConfirmationTitle = new ID("{867771E5-1CFA-4A37-8CC1-14C71FC5ADD8}");
				public static readonly ID ConfirmationUsername = new ID("{5CEBEBE5-3C96-41AC-BA54-38D0B90EEA0C}");
				public static readonly ID UnselectedEmailsError = new ID("{8ABD843A-DF7F-437F-9F44-C708332F3AD1}");
			}
		}

		public struct DuplicateRegistrationPage {
			public static readonly ID ID = new ID("{300BAF41-8DF0-4C83-9741-CF3D61529BF8}");
		}

		public struct HomePage {
			public static readonly ID ID = new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}");
		}

		public struct ZipCodeVerificationPage {
			public static readonly ID ID = new ID("{F53DDE81-AE70-47C4-9AF5-DCD87D0D7A82}");
		}

		public struct LoginPage {
			public static readonly ID ID = new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}");
		}

		public struct RegistrationPage {
			public static readonly ID ID = new ID("{016CB02B-98DA-403E-B75F-538BF642DFE8}");
		}

		public struct ProfilePage {
			public static readonly ID ID = new ID("{6512B2CA-ADFA-4B70-A4F4-541224FD47CC}");
		}

		public struct ProfilePassword {
			public static readonly ID ID = new ID("{8DFF374B-52A5-48A9-BE20-412D6C173856}");

			public struct Fields {
				public static readonly ID AnonymousUser = new ID("{BD07C2BD-A305-4C49-9932-8ECFCA872E25}");
				public static readonly ID ProfileSubmit = new ID("{40170FDB-87BD-444A-AD70-1C6648D5E84D}");
				public static readonly ID PasswordSectionHeader = new ID("{7FDBB15C-7F61-40C7-9015-1D72076BDA24}");
				public static readonly ID ShowText = new ID("{091DE1B0-2C31-4E1E-A354-4A0A4566811F}");
				public static readonly ID PasswordSubmit = new ID("{C2429042-1CAC-48FB-AD8B-7DF616CB8A7E}");
				public static readonly ID SuccessProfileModal = new ID("{50108D96-52D9-435C-8675-C8B12E815C30}");
				public static readonly ID SuccessPasswordModal = new ID("{D48F88AD-0719-40AE-A98A-447A7FE16844}");
				public static readonly ID Error = new ID("{13397EB4-783A-4D95-84DA-8585388D027D}");
				public static readonly ID Optin = new ID("{12C307D7-37B2-4373-9B71-19024F0DC9CD}");
				public static readonly ID AvatarButton = new ID("{C2A8E663-ABBD-43F6-8F9A-49E930B2D59D}");
			}
		}

		public struct ChangePassword {
			public static readonly ID ID = new ID("{8C9D68CA-77EC-45C6-9BF8-D485AFFF2E6C}");

			public struct Fields {
				public static readonly ID CurrentPasswordLabel = new ID("{24585508-FA9F-4CEA-8FF9-7B9BDF6C4BB5}");
				public static readonly ID CurrentPasswordTooltip = new ID("{69C925AE-FD0E-43A7-B0A4-E7A56B7628EB}");
				public static readonly ID CurrentPasswordEmpty = new ID("{C18EBD8B-D381-4B41-89F8-488971E2559D}");
				public static readonly ID CurrentPasswordInvalid = new ID("{677B0202-22CB-49D9-B7EF-6962863BB510}");
				public static readonly ID NewPasswordLabel = new ID("{FFF72422-7E2F-4B2E-A841-1FF22DB99959}");
				public static readonly ID NewPasswordRequirements = new ID("{BFC6EEB2-75B9-4912-949C-D7FF32C0A35A}");
				public static readonly ID NewPasswordConfirmLabel = new ID("{06C1F572-B06B-4A90-8928-FA54767443B2}");
				public static readonly ID NewPasswordMismatch = new ID("{F03C039E-63C4-44B2-9CA3-2418EC5F9415}");
				public static readonly ID MinimumCharacterCount = new ID("{564BD487-3910-468C-8FF7-569C3DA4C0D1}");
				public static readonly ID RequiredPassword = new ID("{019C25CA-5A98-4009-9957-1B3D1EB719A1}");
				public static readonly ID NoSpecialCharactersError = new ID("{13058662-CB9A-446C-9DFB-AA6E0542321F}");
				public static readonly ID ConfirmationMessage = new ID("{3392DBAA-4D87-4656-B6A3-01DF2E597542}");
			}
		}

		public struct Newsletters {
			public struct Fields {
				public static readonly ID Id = new ID("{55DE5632-19E9-4C6F-B983-4770542AB84F}");
				public static readonly ID Vendor = new ID("{965D672E-D77A-4128-BAFA-2C6B0451EBC1}");
				public static readonly ID Headline = new ID("{1E76F3F4-8C45-4622-B347-0E5BB4B0A699}");
				public static readonly ID Description = new ID("{A6EA7686-5F0C-4635-B305-00060B13D1D1}");
				public static readonly ID Subscribe = new ID("{3C8D6C04-DA57-4C5D-B337-BDB3F772AEEC}");
				public static readonly ID Subscribed = new ID("{AF3661AC-FE96-4E44-A99D-89D3FE07FE2D}");
				public static readonly ID Unsubscribe = new ID("{B6E2B48E-4C48-4A83-87BE-1CBB9D7D5E93}");
			}
		}

		public struct SettingsAndSubscriptions {
			public struct Fields {
				public static readonly ID Headline = new ID("{A2EF5FA0-C7BC-4F2C-8711-E05AB5224A9B}");
				public static readonly ID Subheadline = new ID("{6F1779E6-DDEB-4F18-8641-4E98FE949919}");
				public static readonly ID Newsletters = new ID("{142DB6A2-D4AC-4143-BE3B-90FD821C6AD9}");
				public static readonly ID AnonymousUser = new ID("{7CC3025D-F38B-4E21-94FF-4888BE794F93}");
				public static readonly ID Error = new ID("{4B52F327-742D-4937-B99A-42AB10BF24F4}");
				public static readonly ID TitlePhysicalEmail = new ID("{A5C22023-202B-4739-A166-51D9386CCBDD}");
				public static readonly ID HealinePhysicalEmail = new ID("{C005D907-0156-42B5-9E10-60E6034FAF29}");
				public static readonly ID DescriptionPhysicalEmail = new ID("{5295AC47-A60A-4CC9-AE6F-3C991506DA9E}");
				public static readonly ID SubscribePhysicalEmail = new ID("{2B5C7252-E419-4EAB-9B58-DE30CD8C3DA2}");
				public static readonly ID UnSubscribePhysicalEmail = new ID("{89C385CE-3601-43F8-A101-89490F46D5F0}");
				public static readonly ID SubscribedPhysicalEmail = new ID("{20039400-D638-44A3-821D-2D22C175C71C}");

			}
		}

		public struct CategoryItem {
			public static readonly ID ID = new ID("{D1402B59-E079-4856-9DFB-551B6C87B7AE}");

			public struct Fields {
				public static readonly ID Value = new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}");
			}
		}

        public struct ComplementaryLifeInsurance
        {
            public static readonly ID ID = new ID("{293D42C9-C7B9-4A8D-88E4-081BBA5637C7}");

            public struct Fields
            {
                #region General

                public static readonly ID AnonymousUser = new ID("{3C184CB8-8B49-4FDC-9573-8D68BF439CE4}");
                public static readonly ID Header = new ID("{820B4F82-AF76-459D-B7DE-00E6185894BF}");
                public static readonly ID Save = new ID("{271DF272-89E4-40F3-8CFA-45844238D061}");
                public static readonly ID Success = new ID("{F7DD0D4C-862C-4E9A-9D1B-0114189BB4C4}");
                public static readonly ID NewUserHeader = new ID("{BABA881B-92AE-4025-9472-7B03A34577C3}");
                public static readonly ID CompIntroHeader = new ID("{11AF567A-A908-424E-922C-373F852200B9}");
                //public static readonly ID IntroLifeTitle = new ID("{05537A3A-34CD-4A46-BB8E-61B990A62A75}");
            

        
                #endregion

                #region Beneficiaries

				public static readonly ID Add = new ID("{01AB13D5-B4F1-4491-9BBF-1AD9DBB6CF31}");
				public static readonly ID Beneficiaries_Headline = new ID("{C75E442C-1C5A-49E1-A3E9-DDF8938D31EB}");
				public static readonly ID Beneficiaries_Tooltip = new ID("{CD00F86D-4850-4EFF-AA37-07401E10D80B}");
				public static readonly ID PayoutInvalid = new ID("{5FB74C2C-EE52-47AC-A8F2-97F368BA0FF1}");
				public static readonly ID PayoutZeroed = new ID("{0774936F-CD4D-41D1-92A9-46B0C00CF6F8}");
                public static readonly ID Payoutleft = new ID("{7ADD69E8-B095-47EF-83A1-825A8CBADE4D}");
            #endregion

				#region Not Eligible
				public static readonly ID EligibilityDetails = new ID("{A4A2922A-459F-45EE-A0A7-C5D8E9BECEF7}");
				public static readonly ID Notification = new ID("{A436F985-584D-4F54-AD09-B9ABBE6E7A00}");
				#endregion
			}
		}

		public struct EditUpdateInformation {
			public static readonly ID ID = new ID("{D04395BC-85C3-41BE-98F0-90AFEBFE1895}");

			public struct Fields {
				public static readonly ID AnonymousUser = new ID("{EEAD2228-A966-4E9C-AEBF-D513148A6519}");
				public static readonly ID Title = new ID("{EEAAE07F-49D4-4D1A-90EC-84EA6D5498B5}");
				public static readonly ID Intro = new ID("{BB4741DD-D7E9-4F4C-A03F-F08916CF37FD}");
				public static readonly ID Submit = new ID("{2BC5657C-1286-4CC9-9F2F-05A817C88625}");
				public static readonly ID Back = new ID("{05850666-900D-4CCF-BC63-0AA2C0A14380}");
				public static readonly ID Profile = new ID("{EC1FC891-6438-457D-A6BC-5522991B3EB1}");
				public static readonly ID Error = new ID("{BADDA2DB-E9FF-4DB1-9B3B-F58D0665C452}");
			}
		}

		public struct MbcDbField {
			public static readonly ID ID = new ID("{5C14A8D3-13FB-44FC-A059-1AE2F543BDAA}");

			public struct Fields {
				public static readonly ID MbcDbId = new ID("{41FD8FEA-AF79-4266-917B-D2922B8F173D}");
			}
		}

		public struct ForgotPassword {
			public struct Fields {
				public static readonly ID Headline = new ID("{5827C5A7-2C7B-4D0B-8088-3F733A803A38}");
				public static readonly ID Subheadline = new ID("{F2F6351D-4C72-4F7B-B02E-9B69E89343B3}");
				public static readonly ID RequestLink = new ID("{FA0ACB1C-9179-4B4A-8B3A-EDB4EFD15C88}");
				public static readonly ID Signin = new ID("{6691AE0E-A950-4786-A95B-A5AFF609DDC2}");
				public static readonly ID NotFound = new ID("{AE905FA9-497E-46BA-BD6B-9300E9643DB5}");
				public static readonly ID Success = new ID("{8991D53E-4972-4AEF-A028-607D39581451}");
				public static readonly ID Error = new ID("{622F436D-FB70-4969-8AAD-9B2CD4803B93}");
				public static readonly ID PasswordDisavow = new ID("{43E5F26E-2EB3-4F4B-ADCE-DB160EC49AA5}");
				public static readonly ID PasswordResetPage = new ID("{3AA63315-DC50-476A-A1A2-0B3E02024BB2}");				
			}
		}

		public struct ResetPassword {
			public struct Fields {
				public static readonly ID Headline = new ID("{9F9E88C7-5822-4203-A199-B4D1113ADA46}");
				public static readonly ID Subheadline = new ID("{3799D6F1-80A1-4C04-9030-6EEEB0226E96}");
				public static readonly ID Submit = new ID("{1A4CA60F-8B6D-478D-8DB0-A24FAB4229F0}");
				public static readonly ID Success = new ID("{92DEBD9B-4651-4515-82CD-9EB8EA545BC0}");
				public static readonly ID Error = new ID("{7B2CA140-9C6B-49C9-9D72-06DEEDFB5BD3}");
				public static readonly ID Expired = new ID("{793E7496-9EC1-4BF0-B72B-A57C73191460}");
			}
		}

		public struct ResetPasswordDisavow {
			public struct Fields {
				public static readonly ID Success = new ID("{4D3FD892-6891-47D9-A85E-051716407280}");
				public static readonly ID Error = new ID("{959EA749-FACD-4BDF-97CA-8A1C5C5878E9}");
			}
		}

		public struct ForgotPasswordPage {
			public static readonly ID ID = new ID("{25FBED04-5D2E-44B1-8C05-492A9246D2D8}");
		}

		public struct ResetPage {
			public static readonly ID ID = new ID("{FCD0B7D8-BCA1-42ED-A8E0-22C101D22FCD}");
		}

		public struct ResetPageDisavow {
			public static readonly ID ID = new ID("{71B872ED-2C1F-498A-8414-1BE313C448FD}");
		}

		public struct ForgotEmailPage {
			public static readonly ID ID = new ID("{7DC69553-4813-4956-AC32-F8B4C65AE686}");
		}

		public struct RetrieveUsername {
			public struct Fields {
				public static readonly ID Title = new ID("{AC915E4A-5C5B-4AA4-B42E-0BB0B673B049}");
				public static readonly ID Intro = new ID("{3C541146-3BFE-421E-AAA3-5336C5DBAB7B}");
				public static readonly ID Submit = new ID("{CA2AB08E-FFFF-44B1-AB11-D41F35094474}");
				public static readonly ID SignInLink = new ID("{41605645-F66F-4C13-B432-C4F3C39F03E7}");
				public static readonly ID NotFound = new ID("{FD02528B-833E-48BF-990F-410DEAADCA36}");
				public static readonly ID Error = new ID("{B243119B-F492-44B0-A8A9-024BCCD14719}");
				public static readonly ID Username = new ID("{B61D21E9-4F56-4605-B5F5-D889AF43A449}");
				public static readonly ID SignIn = new ID("{49466C7F-BE55-481E-8241-7153CD28EE07}");
				public static readonly ID ForgotPasswordField = new ID("{66BD46DE-852E-4C93-9724-FE063C297399}");
				public static readonly ID RetrievePassword = new ID("{C8948625-1B3F-4082-B7EF-73C0C699225B}");
				public static readonly ID FirstNameLabel = new ID("{6ACC717B-4164-43D7-8DB2-7DE510CA586C}");
				public static readonly ID FirstNameTooltip = new ID("{79B24FFC-C2C4-4A82-8CC3-C19898C25FF0}");
				public static readonly ID FirstNameEmptyField = new ID("{ECA5ECD1-161A-44FE-91DE-6710A65CEE97}");
				public static readonly ID FirstNameCharactersLimit = new ID("{AA6DE8D7-399C-4AB8-BC09-3DC995382641}");
				public static readonly ID FirstNameInvalidCharacters = new ID("{E064480A-A8B3-4808-BF8C-A3AA55E19571}");
				public static readonly ID LastNameLabel = new ID("{C9E11E83-A10D-40A7-A6A7-C73F97F621CD}");
				public static readonly ID LastNameTooltip = new ID("{12DB64AB-04C4-4B0C-8C5E-EB7245B1E27D}");
				public static readonly ID LastNameEmptyField = new ID("{83A56EDF-3BB8-495F-87D8-97D9BDD8F160}");
				public static readonly ID LastNameCharactersLimit = new ID("{6FF1843C-636A-4B82-A884-ABDE4B1C93DA}");
				public static readonly ID LastNameInvalidCharacters = new ID("{ECD8619A-5152-4CC2-A135-3D0BC1443B71}");
				public static readonly ID BirthdateLabel = new ID("{2070E3B0-10AA-4FF2-8FBF-57D16F8E23B1}");
				public static readonly ID BirthdateTooltip = new ID("{5384DE56-43CB-455C-9797-9EF757459C11}");
				public static readonly ID BirthdateEmptyField = new ID("{70AA908B-1E48-4C1B-B76B-C84D04CD2142}");
                public static readonly ID BirthdateInvalidDOB = new ID("{29567A58-36F4-4852-937E-3543F52FBED5}");
                public static readonly ID BirthdateAgeRequirement = new ID("{5D0151D5-DD4B-42C3-94DD-9DCE26927B0F}");
				public static readonly ID ZipLabel = new ID("{8DF3D84C-9D69-4229-A63F-3D9269C6CBF6}");
				public static readonly ID ZipTooltip = new ID("{FEE8614D-CD68-4CA8-BCA7-6184FE69B347}");
				public static readonly ID ZipEmptyField = new ID("{EF24FE82-855B-41F5-9423-055F5A95EE37}");
				public static readonly ID ZipCharactersLimit = new ID("{56B3D228-5387-4738-8CFB-B052F4AEB598}");
				public static readonly ID ZipInvalidCharacters = new ID("{C169DC0D-0F6F-42B9-B18F-F826CCE5220A}");

			}
		}

		public struct Beneficiary {
			public static readonly ID ID = new ID("{293D42C9-C7B9-4A8D-88E4-081BBA5637C7}");

			public struct Fields {
				#region General

				public static readonly ID AnonymousUser = new ID("{32EFD30A-FEA2-49F8-82B8-3DE559E783A1}");
				public static readonly ID Title = new ID("{D11EC4FA-6868-4BED-B74D-F21BB76B5E96}");
				public static readonly ID Intro = new ID("{9CB2E2C5-603C-4314-B9EC-8C9D55FB150C}");
				public static readonly ID Save = new ID("{922C041F-EC66-4C94-B613-1BFED8F81ADF}");
				public static readonly ID Back = new ID("{71CD3BAD-F991-49BC-A193-65FD8EE42EA3}");

				#endregion

				#region Type

				public static readonly ID TypeLabel = new ID("{C17E4EED-8A7E-4407-BCFF-55B001075B65}");
				public static readonly ID TypeTooltip = new ID("{F6F32128-32B5-44E6-83EB-33F32A4F0775}");
				public static readonly ID TypeInvalid = new ID("{73A85D50-3231-4898-8CB2-9750CAEAB159}");

				#endregion

				#region First Name

				public static readonly ID FirstNameLabel = new ID("{D854D7A5-B242-47FA-BC6B-2296367645C2}");
				public static readonly ID FirstNameTooltip = new ID("{2DC8D321-4E3F-4769-A789-F89728F9E751}");
				public static readonly ID FirstNameEmpty = new ID("{EBA46E4A-6822-4BB6-B757-ED9C518D36BC}");
				public static readonly ID FirstNameCharactersLimit = new ID("{0FD58512-AC87-4158-9B3F-ED071F6C0795}");
				public static readonly ID FirstNameInvalidCharacters = new ID("{05D56B40-F439-4976-B036-6170C01D7ADE}");
				public static readonly ID FirstNameMinimumCharactersLimit = new ID("{679CDAF3-0E75-4DC2-B087-F9597ECD8D47}");

				#endregion

				#region Middle Initial

				public static readonly ID MiddleInitialLabel = new ID("{9EB78D68-C7A8-44A3-8427-28D790519797}");
				public static readonly ID MiddleInitialTooltip = new ID("{1A895A55-5776-453C-94FB-57C7D30E1BC4}");

				public static readonly ID MiddleInitialCharactersLimit =
					new ID("{EAFE7E4A-E78B-47A5-B154-B193EF8FB198}");

				public static readonly ID MiddleInitialInvalidCharacters =
					new ID("{88BC7019-CD69-49CC-ABA3-06DD6ABC1092}");

				#endregion

				#region Last Name

				public static readonly ID LastNameLabel = new ID("{9930BD90-D3E7-4D64-B402-4FF8F3DC5661}");
				public static readonly ID LastNameTooltip = new ID("{9D8E9A59-4B8B-45E8-BD8E-F32C3C505D81}");
				public static readonly ID LastNameEmpty = new ID("{6DABE4CD-AE5B-407E-8DAA-9D6D07436A83}");
				public static readonly ID LastNameCharactersLimit = new ID("{8C15C958-A11F-44A2-AE47-EA0058CDE4CB}");
				public static readonly ID LastNameInvalidCharacters = new ID("{C848E700-B061-4B1B-975A-189ABA00DBB9}");
				public static readonly ID LastNameMinimumCharactersLimit = new ID("{D4F26F0D-87C0-4363-B56A-60E6D6276CA6}");

				#endregion

				#region Email

				public static readonly ID EmailLabel = new ID("{0808F79E-2A3B-48DA-89F8-5D36208331C3}");
				public static readonly ID EmailTooltip = new ID("{4BFA9C10-10D1-4D96-AEBB-26FD8BE8FE8B}");
				public static readonly ID EmailEmpty = new ID("{B80708FF-3C7E-4195-A47F-857B82FA54AE}");
				public static readonly ID EmailCharactersLimit = new ID("{5871301B-78FC-4602-A2CC-88DA7E28EE7D}");
				public static readonly ID EmailInvalidCharacters = new ID("{C163B979-7629-4C16-9FD7-3F048937ECAF}");
				#endregion

				#region Other Entity Name

				public static readonly ID OtherEntityNameLabel = new ID("{05E5A58A-3230-44AF-9E75-8530BA0A132C}");
				public static readonly ID OtherEntityNameTooltip = new ID("{945353BC-48CE-4720-B766-F1CBE0F4E623}");
				public static readonly ID OtherEntityNameEmpty = new ID("{B0855D4F-EBA8-444C-96AE-03677E997761}");

				public static readonly ID OtherEntityNameCharactersLimit =
					new ID("{AB3283C4-D60E-49B9-BADC-E4D31EE4397F}");

				public static readonly ID OtherEntityNameInvalidCharacters =
					new ID("{BC401DF1-4BFA-4C11-A77D-28696B762AFD}");

				#endregion

				#region Relationship

				public static readonly ID RelationshipLabel = new ID("{6BDC17AC-F3A2-4FB6-A323-8462C78B3772}");
				public static readonly ID RelationshipTooltip = new ID("{A07E37E9-F0A9-4647-8EAA-B509E770F109}");
				public static readonly ID RelationshipEmpty = new ID("{CB309087-16D9-4D4A-8001-3B90CAAF7116}");
				public static readonly ID RelationshipValues = new ID("{8C2EDDD9-6249-4E75-9B7D-D8908B2B93E2}");

				#endregion

				#region PayoutPercentage

				public static readonly ID PayoutPercentageLabel = new ID("{ACC3449A-C04E-4246-B04B-A2CD59848BA0}");
				public static readonly ID PayoutPercentageTooltip = new ID("{9817A4B5-F7E3-4650-B400-78E7A1898FED}");
				public static readonly ID PayoutPercentageEmptyField = new ID("{07D1C2DF-82DB-4137-895E-C8C068DA88F7}");
				public static readonly ID PayoutPercentageInvalidValue = new ID("{CAB90495-C48B-4A95-A975-5687A1A55541}");
				public static readonly ID PayoutPercentageCharactersLimit = new ID("{2F79C21C-6FE2-4A50-962D-DD105CDC1E55}");
                public static readonly ID PayoutLeft = new ID("{4990AD0C-01AA-456D-B5F4-6CB0A6B0DB21}");

                #endregion
            }
		}
		public struct FieldRepForm {
			public static readonly ID ID = new ID("{96762139-9CD6-4CA8-9668-28F771CC579D}");
			public struct Fields {
				public static readonly ID Title = new ID("{2BEC5727-2AF9-4E27-ADED-822ED73B63ED}");
				public static readonly ID Intro = new ID("{D963B7D7-3C09-4C3A-B02E-70FFBF55E77C}");
				public static readonly ID Label = new ID("{E4A12B0B-823B-4B64-A9BB-BFE117740D90}");
				public static readonly ID Tooltip = new ID("{430D2EE1-9FF1-417B-B037-920AEE1754D1}");
				public static readonly ID EmptyField = new ID("{956992BC-44AB-479E-AC9B-B0D4C880726F}");
				public static readonly ID Submit = new ID("{184EFE87-4D19-43F5-A787-9DE7CB354531}");
			}
		}
		public struct _PageCategoryItem {
			public static readonly ID ID = new ID("{A40A2FA8-803C-4BEB-87A3-5CF0BF0BDEA4}");
			public struct Fields {
				public static readonly ID Value = new ID("{96848D32-0918-446D-B4DB-97124E8E1339}");
				public static readonly ID Page = new ID("{B8A8ED31-E66B-42A3-A333-326A1C118AB4}");
			}
		}

		public struct MemberWelcome {
			public static readonly ID ID = new ID("{D6F2C9DC-836C-4831-A081-9F52B4A4B3CD}");
			public struct Fields {
				public static readonly ID CampaignCode = new ID("{12E3887C-08AF-4837-97AF-81728374FF98}");
				public static readonly ID Headline = new ID("{163C97E1-7BCB-4597-8DC1-C3FC7A9C9841}");
				public static readonly ID Cold = new ID("{E21D4748-85B4-4AC9-A26E-CBA003FF6E80}");
				public static readonly ID ChatIconText = new ID("{EB54D432-C49A-486D-B1EF-82E407979957}");
				public static readonly ID Warm = new ID("{FD04D7CF-09EA-4129-8A2B-19FEEBC11241}");
				public static readonly ID Description = new ID("{4AEAF3C2-7342-434D-A845-56DE8F550C0D}");
				public static readonly ID PersonalCode = new ID("{F3853411-AA75-40F0-B8DA-9D9150DB030C}");
				public static readonly ID EmptyCode = new ID("{528C2251-84E0-4F97-A7EB-01B2D80A1C2B}");
				public static readonly ID InvalidCode = new ID("{476CED8D-15B7-4040-975B-E7A4E4D9FB4B}");
				public static readonly ID Video = new ID("{873FAFA7-6BDB-4D81-A8A6-6AEE458267C9}");
				public static readonly ID Register = new ID("{D4BFC6B4-5ACC-4145-8837-A1B1DABD6355}");
				public static readonly ID FinePrint = new ID("{D169ADB0-F8EE-45F9-9DF9-564DC8D5B229}");
				public static readonly ID MdsidMaxLenght = new ID("{E4D3C9DC-E142-4BE4-815C-A43334AE9924}");
				public static readonly ID MdsidInvalidCharacters = new ID("{647B4493-8482-4C72-9136-3A4A1652EAE2}");
			}
		}

		public struct Shareable
		{
			public static readonly ID ID = new ID("{9A54D88A-395B-4463-9B6A-7BA30FC9FE75}");
			public struct Fields
			{
				public static readonly ID ShowSocialShare = new ID("EDAC8876-554A-4341-A01E-3A88AEDE0D01");
			}
		}

		public struct MemberVerificationForm {
			public static readonly ID ID = new ID("{87690C52-8AB6-49BD-84B3-66DD4E5E410D}");
			public struct Fields {
				public static readonly ID Salutation = new ID("{A6795515-1B7E-4A25-B8A4-9C91A639F0B1}");
				public static readonly ID InstructionsRegistered = new ID("{1920290D-6992-454F-A2C8-089D36027A49}");
				public static readonly ID InstructionsNotRegistered = new ID("{236E7853-E821-4CEF-9199-C15CA7F6D45E}");
				public static readonly ID Submit = new ID("{02213864-133B-4F28-8C46-466CD5A4F8CE}");
				public static readonly ID NotYou = new ID("{7577D10F-1312-4029-A9B2-22E7EA98AA46}");
				public static readonly ID Error = new ID("{D0C54F48-6644-4CE2-9A6D-B2A5ED1982B4}");
				public static readonly ID IdLabel = new ID("{5DBA1038-7A94-4A50-BFC4-5C1357514831}");
				public static readonly ID IdTooltip = new ID("{F56F5005-B3B6-41D4-8EFF-D422F9C29AEB}");
				public static readonly ID PasswordLabel = new ID("{2DE67AC9-A904-4297-B112-A6DBB7459337}");
				public static readonly ID ShowText = new ID("{A8BEC641-C72A-485F-B4A7-C0BE46E4C4FD}");
				public static readonly ID PasswordTooltip = new ID("{E8FC2051-E5A7-4A6C-8002-39EA2BDE14B2}");
				public static readonly ID PasswordEmptyField = new ID("{38C6219B-565D-47D2-802B-54B5E3F037B3}");
				public static readonly ID PasswordInvalid = new ID("{1711D64C-681E-4BE6-A007-40CE5A9DDC63}");
				public static readonly ID AccountLocked = new ID("{E2F64742-B7DD-4631-8A3F-24A6C6B3C73F}");
				public static readonly ID AccountAlreadyLockedValidToken = new ID("{7370F309-EAA7-455B-B8A7-9E0BF9CEC328}");
				public static readonly ID TimeOut = new ID("{3F78918F-7F2B-4AAD-B82D-1C4444648168}");
				public static readonly ID ZipLabel = new ID("{8E7AA275-45D1-4E79-AF3D-62C64163C5E9}");
				public static readonly ID ZipTooltip = new ID("{CEBB1070-33F8-4D83-90B5-7DD33E68CF35}");
				public static readonly ID ZipEmptyField = new ID("{8EC69771-C138-4BF4-9E3C-4A503AFA1BB6}");
				public static readonly ID ZipCharactersLimit = new ID("{7BD23135-E991-4D3D-B7C5-D4158CF72410}");
				public static readonly ID MatchNotFound = new ID("{3C43FD5A-B4E6-4932-A6D9-ED219B2C2FCD}");
				public static readonly ID MinimumCaracterCount = new ID("{163E5A58-2F18-4853-BCF6-8995156082E6}");
				public static readonly ID NoEspecialCaracterError = new ID("{0C47929E-28A7-4E0B-9000-8759C1D12171}");
				public static readonly ID InvalidCharacters = new ID("{A9A5B705-A5F0-4CF6-A2AB-6394A465519B}");
			}
		}
		public struct MemberCard {
			public static readonly ID ID = new ID("{217B5275-E13E-40A7-8EAB-7F4CB1392AEF}");
			public struct Fields {
				public static readonly ID Headline = new ID("{1749B884-2876-40DB-920E-1C6CC29AC6AA}");
				public static readonly ID Logo = new ID("{64B8C684-8DE5-4236-92E5-3D96EAB71E2B}");
				public static readonly ID Intro = new ID("{736F3B64-ED71-4E29-AA78-D41C4D81F405}");
				public static readonly ID Description = new ID("{404A846D-BCCD-4284-8819-3F767420BDD7}");
				public static readonly ID Instruction = new ID("{56056EB1-98D5-49BC-9653-136BC097A5A5}");
				public static readonly ID Submit = new ID("{E740E53E-8CA6-494D-A64D-FA301D17BA2C}");
				public static readonly ID ErrorMessage = new ID("{0ECD5F55-4DCA-44E6-A761-5F3497C704D5}");
				public static readonly ID ExistingUserText = new ID("{B7D1C1F8-0AC8-40B4-A951-673B1B5BA527}");
				public static readonly ID EmptyCode = new ID("{4D8BAE27-473D-4145-A352-9899E736C920}");
			}
		}
		public struct MemberCardLoginForm {
			public static readonly ID ID = new ID("{75FCC427-9025-4B21-8150-31DAF724DF35}");
			public struct Fields {
				public static readonly ID Salutation = new ID("{0AC5BFAE-3B0F-45B2-9708-C71F48D5C4F7}");
				public static readonly ID Login = new ID("{C5EC4F8D-F57A-499F-AE63-72A30B73C119}");
				public static readonly ID NotYou = new ID("{3CB714A9-E394-4AD2-BE03-78369C455897}");
				public static readonly ID Error = new ID("{7D6FC6D0-C286-423B-A9DA-11725A9F4F79}");
				public static readonly ID Label = new ID("{CF4A9435-A9B5-45EA-9A61-A8DBECDA378B}");
				public static readonly ID ShowText = new ID("{42FE4141-F134-4473-8F32-B8490F490D26}");
				public static readonly ID Tooltip = new ID("{BDFAB9B4-852A-422F-A7ED-0721AAAEC986}");
				public static readonly ID EmptyField = new ID("{D63CE1DA-B54B-49D4-A490-61382ECBF359}");
				public static readonly ID Invalid = new ID("{D3A1D835-E131-4D20-B372-DC252D9968F4}");
				public static readonly ID AccountLocked = new ID("{ADC024EF-2AB6-4B83-82D4-6D5EF2B4019A}");
				public static readonly ID AccountAlreadyLockedValidToken = new ID("{470FD7BC-46A1-4DFF-B800-A6B92E012B2F}");
				public static readonly ID TimeOut = new ID("{C959ACC8-0467-4305-B795-5301B8AEC278}");
			}
		}
		public struct SiteSettings {
			public static readonly ID ID = new ID("{C7EADD3C-19BC-463B-B0CC-A862E99E5B50}");
			public struct Fields {
				public static readonly ID Chat = new ID("{5FBA9701-F6A5-4753-AA15-57E33DE365DD}");
				public static readonly ID Phone = new ID("{F949BC80-EED6-475F-82CB-EC62F180C3B0}");
				public static readonly ID Email = new ID("{E0A29AB9-1AB2-425A-B3C0-5EB830B25514}");
				public static readonly ID HeaderAvatar = new ID("{EC93C27B-A621-4B20-AD31-8559715607C7}");
				public static readonly ID InlineButtons = new ID("{ABBBEC1F-4935-4A2A-8586-D0417F65F797}");
				public static readonly ID AddThisCodeSnippet = new ID("{051E5ACF-950A-4733-B339-54AC5701B998}");
			}
		}
		public struct AvatarPage {
			public static readonly ID ID = new ID("{7E0E6A1F-E84A-4A15-A1CF-E8DE225528F2}");
		}

		public struct InviteFamilyMember {
			public static readonly ID ID = new ID("{0C376C9A-A8D5-4E93-B077-A06D2201A870}");
			public struct Fields {
				public static readonly ID AnonymousUser = new ID("{C46444A3-00F4-466C-ACDB-26EB20D38143}");
				public static readonly ID Headline = new ID("{5AC1AF71-EBA0-4E4D-8BA3-CDF385DF61A2}");
				public static readonly ID SucessfulMessage = new ID("{D24036BD-0BDE-4B66-948C-767D1B7BCD32}");
				public static readonly ID Title = new ID("{402E57F9-7488-423E-AD55-70C13135A2F7}");
				public static readonly ID Add = new ID("{8D9CDD04-ECB6-49DA-9C1A-B6095B09E47A}");
				public static readonly ID Remove = new ID("{7CC3E0A7-3916-488B-9F89-6657CC82C72F}");
				public static readonly ID ConfirmRemoveModal = new ID("{F26ADC12-67E6-4B32-9A8E-994C5AF68124}");
				public static readonly ID ConfirmRemove = new ID("{61D1AC9F-2ACC-4C09-B704-91C665F17857}");
				public static readonly ID RejectRemove = new ID("{8DD02770-8B71-4E4B-BE86-3F78969350AF}");
				public static readonly ID Error = new ID("{2B995616-6A0D-47E8-B6DE-FADBDD8593F9}");
				public static readonly ID Avatar = new ID("{A196B71F-DB0D-4A46-BE8B-81F3E0A67D75}");
				public static readonly ID ResendInvitationText = new ID("{26BE5A9B-D212-46CB-A7F6-C18A7ECB4073}");
				public static readonly ID ResendInvitationModal = new ID("{C6FB8426-5163-4703-902D-71BED674D5FA}");
				public static readonly ID ConfirmResend = new ID("{64032736-BDF7-475E-9CFE-5E4F6CBD1C7B}");
				public static readonly ID RejectResend = new ID("{F040D124-6242-4B1C-B26A-A18654D98967}");
				public static readonly ID FamilyNumberLimit = new ID("{CD4A8940-6B9A-4C07-A377-C9A12F0772FE}");
			}
		}

		public struct FamilyMember {
			public static readonly ID ID = new ID("{E3A5D04C-4A1B-48DB-9603-7A4A754375BB}");
			public struct Fields {
				public static readonly ID AnonymousUser = new ID("{475BAC36-E5B8-4A1E-8B62-075DC83323F2}");
				public static readonly ID Headline = new ID("{364C6846-2B6A-436F-82C4-B98985FD1C03}");
				public static readonly ID Subheadline = new ID("{57B11314-D4B9-4604-BF8B-B3D06B5A6B69}");
				public static readonly ID SendInvitation = new ID("{D00621C3-2181-4DA4-A7D7-68A88C452405}");
				public static readonly ID Cancel = new ID("{D5103C30-5784-42B4-A437-614337F89C60}");
				public static readonly ID Back = new ID("{A9F58D07-7864-4E01-B0C5-4D828077425B}");
				public static readonly ID Error = new ID("{AAB95E8F-444C-4EDE-B1FC-717C4A462147}");
				public static readonly ID RelationshipLabel = new ID("{9451E63B-5772-43F7-8EB3-B97A21F4623D}");
				public static readonly ID RelationshipTooltip = new ID("{FCC897D1-EB0C-4E02-BA13-CAE716B55966}");
				public static readonly ID RelationshipEmpty = new ID("{966DE172-5EE4-44E3-B81F-C639CA70DB6C}");
			}
		}
		public struct RelationshipInviteFamilyGlobal {
			public static readonly ID ID = new ID("{AFC4FA85-02B7-46AE-B65E-8A581FFDB765}");
		}
		public struct InviteFamilyPage {
			public static readonly ID ID = new ID("{7C9DC1C9-228D-4885-961A-1CBA0BBCEA06}");
		}
        public struct WizardStep
        {
            public static readonly ID ID = new ID("{3D6E9153-142E-4E47-85C2-45D6C50BE5EA}");
        }
    }
}