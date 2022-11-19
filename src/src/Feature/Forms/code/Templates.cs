using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Forms
{
    public struct Templates
    {
        public struct Home
        {
            public static readonly ID ID = new ID("{72051F7F-87F7-4279-B455-5A7EACA902F5}");
        }
        public struct LoginMobilePage
        {
            public static readonly ID ID = new ID("{9EE7F15D-D997-4C88-8FCE-1C44E2C078FB}");
        }

        public struct StatesGlobal
        {
            public static readonly ID ID = new ID("{F1FC2588-F957-4D1E-B6E3-42AAFC56419C}");
        }

        public struct ThankYouPageTemplate
        {
            public static readonly ID ID = new ID("{B497F700-68C9-40CC-8FC5-1B6ADF3D9961}");            
        }

        public struct RegistrationThankYouTemplate
        {
            public static readonly ID ID = new ID("{05CF65D1-3137-40EE-9FB6-FAE3A19BE9FC}");
        }

        public struct ErrorPageTemplate
        {
            public static readonly ID ID = new ID("{D613828E-0662-45F9-9A35-D1C6C47271E8}");
        }

        public struct RegistrationPage
        {
            public static readonly ID ID = new ID("{8F873B39-85AB-4068-A66F-F6428AED4BDE}");
        }

        public struct ZipCodeValidationPage
        {
            public static readonly ID ID = new ID("{009802FF-3592-4EFE-85CC-346EA3698127}");
        }

        public struct ProfilePage
        {
            public static readonly ID ID = new ID("{C544123B-E481-4A1F-AD74-8EF77D48DDC7}");
        }

        public struct ZipCodeValidationTemplate
        {
            public static readonly ID ID = new ID("{AC328FAE-2ECD-42D3-83A7-0DC1B035E2BE}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{69783EE5-4275-4A92-BF07-72A68F2846E6}");
                public static readonly ID Salutation = new ID("{2CF5A467-5E5F-4085-AB2F-4EB8285AE2AB}");
                public static readonly ID Instructions = new ID("{68EF14F7-7717-4C7E-8B3D-F1B2AA2A34A8}");
                public static readonly ID NotYou = new ID("{DCDF5906-0475-4526-BD81-BD05CF5DD7F8}");
                public static readonly ID Next = new ID("{50B7E8BB-4026-4A37-8769-16C11D324DE4}");
                public static readonly ID RegistrationLink = new ID("{A3FBE235-2001-4D35-AEEE-4AA7196395D4}");
                public static readonly ID ZipCode_Label = new ID("{34F08469-D452-4325-8785-4325221B3EB7}");
                public static readonly ID ZipCode_Placeholder = new ID("{03151CC7-2EE1-4B8F-A1DE-B884A9F2F0EA}");
                public static readonly ID MatchErrorMessage = new ID("{AB8BB49E-43E8-4187-9CF6-55A64732A290}");
                public static readonly ID AttemptMessage = new ID("{F65084A1-DA4D-4881-A065-90BDB1284869}");
                public static readonly ID ZipError = new ID("{DE143051-B3E3-4D92-AE2D-ECC26D8AD8FA}");
				public static readonly ID CharactersLimit = new ID("{3FB003DB-7BEA-4218-B9EB-424BED11ABE3}");
			}
		}

        public struct ProfileTemplate
        {
            public static readonly ID ID = new ID("{488E9493-1E67-4E2E-89AD-675A188E6A4E}");
            public struct Fields
            {
                public static readonly ID InvalidCharacters = new ID("{7C0B94A0-16CD-4B91-A793-6FC083B3E0FA}");

                public static readonly ID FirstName_Label = new ID("{DB3367AE-7B4D-429B-8EBF-BD30D2A57677}");
                public static readonly ID FirstName_Placeholder = new ID("{D9BDFA53-A68C-4E74-BB73-C42689C2F253}");
                public static readonly ID FirstName_ErrorMessage = new ID("{97B8B630-9EC7-4052-807A-2470919B6975}");
				public static readonly ID FirstName_CharacterLimit = new ID("{0909640C-24DD-4A73-9C42-23B75B867A0E}");

				public static readonly ID LastName_Label = new ID("{B540C072-076C-45E8-99B7-E0592E3A7715}");
                public static readonly ID LastName_Placeholder = new ID("{07DF3E3A-6929-4BF5-ABA1-AABD791CDEB7}");
                public static readonly ID LastName_ErrorMessage = new ID("{387DD14D-667B-49A4-9C03-45CA8088AAA8}");
				public static readonly ID LastName_CharacterLimit = new ID("{26031DA4-6278-41F0-A01E-C756D8A407B6}");

				public static readonly ID DateOfBirth_Label = new ID("{A0650335-0FE2-46EB-B01E-7E7C9DEA0D2D}");
                public static readonly ID DateOfBirth_Placeholder = new ID("{B481ED26-3261-491E-BF93-E97744022842}");
                public static readonly ID DateOfBirth_ErrorMessage = new ID("{B6ECA548-3F44-4702-959A-D67533BF9EDC}");
				public static readonly ID DateOfBirth_AgeRequirement = new ID("{04AA478E-AA4A-4538-A705-27698570E227}");

				public static readonly ID Address_Label = new ID("{8C682A25-475D-4D4B-8538-9EF1C4EBED89}");
                public static readonly ID Address_Placeholder = new ID("{936D9CB1-7F43-4D67-9625-AEF7E2B7D748}");
                public static readonly ID Address_ErrorMessage = new ID("{D44C3E87-72F3-4589-A2D1-E0113A840994}");
				public static readonly ID Address_CharacterLimit = new ID("{550E7A7C-BE80-4052-A0C5-2B8D9E541BEE}");

				public static readonly ID City_Label = new ID("{3A29A784-280B-4393-B05E-CBC720470FE5}");
                public static readonly ID City_Placeholder = new ID("{524D9A4B-E3E4-488F-A162-FAA777E263F3}");
                public static readonly ID City_ErrorMessage = new ID("{D12908AC-1B76-4A12-B8BB-B1870D662E4D}");
				public static readonly ID City_CharacterLimit = new ID("{1A235878-E5C6-4903-8ED1-43F48171A74B}");

				public static readonly ID State_Label = new ID("{555997ED-797C-4481-BFDC-CB24CCCA0CE4}");
                public static readonly ID State_Placeholder = new ID("{8682210D-0D92-40D3-A527-0B99D358A207}");
                public static readonly ID State_ErrorMessage = new ID("{F7882F79-3D9B-4567-ADDA-AEC51339B083}");

                public static readonly ID ZipCode_Label = new ID("{AB5122B5-C170-4207-BCBA-86E480B4BB84}");
                public static readonly ID ZipCode_Placeholder = new ID("{27FEE4ED-819D-4EA9-B80B-37B9AE2D3A7D}");
                public static readonly ID ZipCode_ErrorMessage = new ID("{98448C9D-3E35-4B5F-BC4C-46ED828AD308}");
				public static readonly ID ZipCode_CharacterLimit = new ID("{4B4B4E04-A4A2-4A20-BA33-230ADB79322B}");

				public static readonly ID PhoneNumber_Label = new ID("{64E0B9F1-4564-4BD6-9412-FB4B5D4509CF}");
                public static readonly ID PhoneNumber_Placeholder = new ID("{A6196F8D-033B-4505-B398-F423951C2AEB}");
                public static readonly ID PhoneNumber_ErrorMessage = new ID("{DBDB0427-40A1-4A0F-AA3B-AB39A20ED7F2}");
				public static readonly ID Phone_CharacterLimit = new ID("{B4593E80-2EED-421A-A3DC-723409054A7C}");

				public static readonly ID Title = new ID("{401AFC4A-102A-42BA-9D23-17178C567B73}");
                public static readonly ID AnnoymousUserMessage = new ID("{1B79AF62-5905-4C7B-8D0E-94EEDA3E1C50}");

                public static readonly ID ProfileTabTitle = new ID("{4CEFF60C-6943-4567-9B16-223500DB62F8}");
                public static readonly ID ProfileHeadline = new ID("{B37A6B0F-0049-4B5E-A3A3-DCBC053CB6E2}");
                public static readonly ID ProfileSubheadline = new ID("{FDBC4029-7A1E-4D6F-8D8F-9EA6EF8A2909}");
                public static readonly ID ProfileOptin = new ID("{3BD498CA-B468-483F-83EA-511737EF57CC}");
                public static readonly ID ProfileQuickLinks = new ID("{FA4FCDE3-CF69-4F84-BE93-C7AFF9B0ECEE}");
                public static readonly ID ProfileSubmitButton = new ID("{4FB5C21A-FE6B-4C01-8616-B183A65E5BCE}");
                public static readonly ID ThankYouPage_Profile = new ID("{5CC34482-0C13-4D86-96C6-77FFAFAB72D3}");

                //username / email
                public static readonly ID UsernameTabTitle = new ID("{A8C88300-19D7-483B-82CF-5F641481CB54}");
                public static readonly ID UsernameHeadline = new ID("{EB52C971-C5C6-41C7-BCCD-ECA742BA9921}");
                public static readonly ID UsernameSubheadline= new ID("{44440E30-24DC-4ADB-BF5B-412ACCE179C7}");
                public static readonly ID UsernameQuickLinks = new ID("{646A313A-590F-48AB-8B0D-D10AEB12358C}");
                public static readonly ID UsernameSubmitButton = new ID("{14078523-0F1B-486C-98C8-01E246081C98}");

                public static readonly ID CurrentUsername_Label = new ID("{BA425B33-CB3F-4C18-A2D8-3EE6C4CC8B6E}");
                public static readonly ID Email_InUse = new ID("{6BB22F35-95A3-416F-B7DB-774648698967}");
                public static readonly ID Email_Warning = new ID("{BC26D75E-A341-474A-A399-DE172D709BC2}");
                public static readonly ID Email_Empty = new ID("{B7AB67AD-DD0A-4692-AD90-B6D1DED8D9F2}");
                public static readonly ID Email_CharactersLimit = new ID("{CDFB3197-BDAF-4280-86DC-9A9A38A5E49F}");
                public static readonly ID Email_InvalidFormat = new ID("{C4833EE6-34E9-4BF9-8FB4-79BF2C2B3467}");
                public static readonly ID Email_NoMatch = new ID("{C6C03AB2-75E5-4BDB-ADDC-FE81CFA557B9}");

                public static readonly ID NewUsername_Label = new ID("{5413D1C2-9515-4633-86E4-21E4B37F8B4F}");
                public static readonly ID NewUsername_Placeholder = new ID("{DB9E6AAE-CB64-4FA2-876B-AFDCDB83E785}");

                public static readonly ID UsernameConfirmation_Label = new ID("{4BC50B8A-3FD8-4BA0-9A04-E17076ECDC10}");
                public static readonly ID UsernameConfirmation_Placeholder = new ID("{E54E600F-B4E9-45A9-9281-1197C9575004}");
                
                public static readonly ID ThankYouPage_Username = new ID("{940CD63B-80A0-4A10-B0DA-4E067DD4BD48}");

                //password
                public static readonly ID PasswordTabTitle = new ID("{0A844B49-A6FD-4D87-B257-7022D27A21BF}");
                public static readonly ID PasswordHeadline = new ID("{2186BF61-163A-477D-BDA2-C4F795F081B9}");
                public static readonly ID PasswordSubheadline = new ID("{12B51FB0-11EA-4B00-B385-7A23648BC495}");
                public static readonly ID PasswordRequirements = new ID("{FA2F0029-FF17-493C-B349-9A6FE90EC44B}");
                public static readonly ID PasswordQuickLinks = new ID("{7E5CB933-E978-490E-BBE6-6EF0A7C3A146}");
                public static readonly ID PasswordSubmitButton = new ID("{F15657A2-0CA1-481E-B01F-0166043C36ED}");
                public static readonly ID PasswordError = new ID("{83486C75-9A54-4437-9EAC-2A42A0C37640}");
                public static readonly ID InvalidPassword = new ID("{8DD16797-AB4A-4B5A-A65D-DBBC94FF8AA2}");
                public static readonly ID ValidPassword = new ID("{E1214E99-D714-48F7-9EB7-C3949FEC6D9B}");


                public static readonly ID CurrentPassword_Label = new ID("{AB47EB62-870B-4564-9289-61A2FCD60822}");
                public static readonly ID CurrentPassword_Placeholder = new ID("{F8B04407-8ECC-490B-A2B1-73D970B1066E}");
                public static readonly ID CurrentPassword_ErrorMessage = new ID("{0099C2BA-3B94-45EA-B770-C7A61528CE6D}");

                public static readonly ID NewPassword_Label = new ID("{BA176D84-4CC9-4A81-A6D9-A442DB504DEB}");
                public static readonly ID NewPassword_Placeholder = new ID("{F5A86667-C734-44DF-B2B0-3C63FD8FFF8D}");
                public static readonly ID NewPassword_ErrorMessage = new ID("{8DD16797-AB4A-4B5A-A65D-DBBC94FF8AA2}");

                public static readonly ID ConfirmNewPassword_Label = new ID("{D509BA1C-A2C6-43FB-A3EA-181C3BA9DA1D}");
                public static readonly ID ConfirmNewPassword_Placeholder = new ID("{2DEE9291-7A63-4403-92DA-AD99810537C5}");
                public static readonly ID ConfirmNewPassword_ErrorMessage = new ID("{F7A121E8-3C05-4AA0-A24A-F1602EF53A93}");
                public static readonly ID ThankYouPage_Password = new ID("{00E8CAAC-3DDD-4826-832D-9F936074F128}");

            }
        }
        public struct LogInHelp
        {
            public static readonly ID ID = new ID("{F8433FD9-6BED-4C7A-9E9D-51D46DF44280}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{457B093D-98D4-45B2-86AA-177CA527A776}");
                public static readonly ID Subheadline = new ID("{07477A5D-ED7A-4213-9B1D-76036FCFFBD8}");
                public static readonly ID ChangePasswordTitle = new ID("{DB62641A-AD86-491F-A87F-3C4C63FACA67}");
                public static readonly ID ChangePasswordDescription = new ID("{8928FA88-18C2-4ED4-813D-FDC8FC3957B6}");
                public static readonly ID ChangePasswordCta = new ID("{FA1D9C5A-7D91-4C5C-9B04-7FEFCDF5645E}");
                public static readonly ID ChangeUsernameTitle = new ID("{4A13D160-32A1-4822-8CB5-C72C5BBDDA39}");
                public static readonly ID ChangeUsenamerDescription = new ID("{B2E3E443-75D2-430F-83B4-9CC06312F68C}");
                public static readonly ID ChangeUsernameCta = new ID("{F8DF4CAC-BBD5-43EC-ABA5-9F0AB15EA7A7}");
                public static readonly ID RetrievePasswordTitle = new ID("{E009CAEC-6A73-45DB-ABC3-302D69B62094}");
                public static readonly ID RetrievePasswordDescription = new ID("{02B582E9-9190-44BD-8111-668C511AE30E}");
                public static readonly ID RetrievePasswordCta = new ID("{9D27B4C0-7DE9-4804-BF98-B828CD4E8ABC}");
                public static readonly ID RetrieveUsernameTitle = new ID("{828CE64E-C7A9-41E2-98BF-CB2D2B9A4E4F}");
                public static readonly ID RetrieveUsernameDescription = new ID("{457D1436-1593-4DEC-BFB8-F0798C23B1E2}");
                public static readonly ID RetrieveUsernameCta = new ID("{BC432C2D-D86D-439F-93A3-29933CC14F18}");
            }
        }
        public struct RetrieveUserName
        {
            public static readonly ID ID = new ID("{7547EF37-7745-4030-992D-5CD099ABA7E6}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{758D4287-E186-4F7D-AA45-3FBF3C19DF51}");
                public static readonly ID Subheadline = new ID("{E4A3A899-36DC-4B75-8862-EB6C971F08A8}");
                public static readonly ID SubmitButton = new ID("{B566DBFA-950A-4846-B04A-21FFC36248BE}");
                public static readonly ID Username = new ID("{23714CD9-C12F-4F20-8599-E516567A3945}");
                public static readonly ID ForgotPassword = new ID("{939E68A4-BA20-477C-8B22-DB28A63F1898}");
                public static readonly ID Login = new ID("{43BFAF32-1530-4CE7-8664-7F2373A3D13A}");
                public static readonly ID NotFound = new ID("{38569D12-DAD5-45E5-A301-6801CFBE6399}");
                public static readonly ID RetrievePassword = new ID("{E849E4EA-7B4E-4687-8D4A-0CACB164C616}");
                public static readonly ID InvalidCharacters = new ID("{ACDF90C3-00A5-48AE-8FEC-81E767311828}");
                public static readonly ID DateOfBirthLabel = new ID("{0F60322A-1AE5-4646-9A8A-30F31A9680D3}");
                public static readonly ID DateOfBirthPlaceholder = new ID("{D2B42926-5AFC-4CA8-8AD3-3DCB627DBAE1}");
                public static readonly ID DateOfBirthErrorMessage = new ID("{827EF3E8-0B73-42DF-93E2-288369911305}");
                public static readonly ID FirstNameLabel = new ID("{9E75352F-2C1A-4E3A-B4D1-02B545FECFE6}");
                public static readonly ID FirstNamePlaceholder = new ID("{BC3CADE1-725D-4CA5-A127-ADBFC0E36830}");
                public static readonly ID FirstNameErrorMessage = new ID("{46538E14-7B4F-4556-9202-29A3146CCFDE}");
                public static readonly ID LastNameLabel = new ID("{95B6A6F2-4E66-453B-9460-744038B18D06}");
                public static readonly ID LastNamePlaceholder = new ID("{C30DCCD3-61BB-4379-BAB8-93A5E62145A1}");
                public static readonly ID LastNameErrorMessage = new ID("{149E5CF9-3C26-49BE-9FC9-5CC60D1A2AA5}");
                public static readonly ID ZipcodeLabel = new ID("{861E30B1-AC0C-49DC-BE51-90D9B3A31F0F}");
                public static readonly ID ZipcodePlaceholder = new ID("{78F96575-2638-4BB5-A109-8D17E1D98010}");
                public static readonly ID ZipcodeErrorMessage = new ID("{4FE2B3A2-7C3B-46A7-A089-A08D53275E31}");

				public static readonly ID FirstName_CharactersLimit = new ID("{F8AA7A42-423D-4995-9812-D70A28565A90}");
				public static readonly ID LastName_CharactersLimit = new ID("{1ADE25E0-A510-4B63-BE5C-56F0450BA677}");
				public static readonly ID Zipcode_CharactersLimit = new ID("{A037AA90-7A81-4AB9-824A-FC6A8FC9D8B8}");
				public static readonly ID DateOfFirst_AgeRequirement = new ID("{4D49F3F6-256D-49A1-B8D8-D514E07F8FC7}");

			}
		}

        public struct RetrievePassword
        {
            public static readonly ID ID = new ID("{6CF7EC58-7F8D-48E2-B96B-F5979303CD74}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{0394F7DC-9D08-4588-AFE2-81F952D69C84}");
                public static readonly ID Subheadline = new ID("{B5A0BCD3-EA9F-4DD8-B646-29262697B78B}");
                public static readonly ID SubmitButton = new ID("{563E05D6-4FB2-457A-A390-432D018F28A8}");
                public static readonly ID NotFound = new ID("{BFD36E1C-7BF7-4D84-B9A2-E229FAB85393}");
                public static readonly ID Sucess = new ID("{A64BA5F4-34E8-402B-92B3-8E4DBE33A064}");
                public static readonly ID ValidToken = new ID("{1FD9CA68-B0CC-4B0C-95EA-BD5CBB26F0AD}");
                public static readonly ID UsernameLabel = new ID("{F29F2ABE-3E55-4504-8EEC-AA81B0C4240A}");
                public static readonly ID UsernamePlaceholder = new ID("{2269D73B-8A4B-4538-AED8-8C6445D56A38}");
                public static readonly ID Email_Empty = new ID("{393F8E7E-7044-4C52-AB29-4D30FC1DC787}");
                public static readonly ID Email_CharactersLimit = new ID("{29A0BE25-6963-4B46-A0E2-1656CA4DC07A}");
                public static readonly ID Email_InvalidFormat = new ID("{38638789-CC66-4906-981B-2EB489B4F5EE}");
                public static readonly ID ResetLink = new ID("{B32573CB-7DF7-4016-8C15-2292A8D6F78A}");
                public static readonly ID CancelLink = new ID("{61EB6A4C-5F23-41E2-9DA4-08332C6452B9}");
            }
		}
        public struct ResetPassword
        {
            public static readonly ID ID = new ID("{DCFE716D-1E15-4BEC-A9D6-402DE283C336}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{F0811BCE-0FC2-4D9D-AD1F-1CF469FEF0AC}");
                public static readonly ID Subheadline = new ID("{9409C3A4-74EE-4B8D-9A73-8175BBEB2564}");
                public static readonly ID PasswordRequirements = new ID("{1724D024-EE84-4BF3-839B-9F840424B864}");
                public static readonly ID SubmitButton = new ID("{07FE7993-6A99-41D9-A951-7546BA4FC7D6}");
                public static readonly ID InvalidToken = new ID("{477E6545-8F80-4EB1-820B-05F52B587C41}");
                public static readonly ID ThankYouPage = new ID("{7F5F04A3-6982-4DAB-B5FB-611EC792E9AF}");
                public static readonly ID NewPasswordLabel = new ID("{3CF52B1C-9746-4935-9BBC-7A459D54817F}");
                public static readonly ID NewPasswordPlaceholder = new ID("{51BE3F9D-DB7E-4B92-A4DB-747CD6613D35}");
                public static readonly ID NewPasswordErrorMessage = new ID("{CC7C15AF-2F95-4DB8-9CBC-AED36F977AAE}");
                public static readonly ID ConfirmPasswordLabel = new ID("{42F3BD44-6F42-4A9B-81ED-1CDAF62BA89F}");
                public static readonly ID ConfirmPasswordPlaceholder = new ID("{B1D1B630-31A2-4498-B141-1710D502390B}");
                public static readonly ID ConfirmPasswordErrorMessage = new ID("{3D1A70AF-4627-4416-A776-871BB98E1BC3}");
                public static readonly ID InvalidPassword = new ID("{DA7E9420-E2B9-4F2C-B8F4-499D199DEAA8}");
                public static readonly ID ValidPassword = new ID("{3BC0AACC-50D2-464C-A2EF-8A715F246436}");
            }
        }

        public struct ThankYouTemplate
        {
            public static readonly ID ID = new ID("{A7FD5976-7636-4EB2-A6F2-2270CFFBEB04}");
            public struct Fields
            {
                public static readonly ID Body = new ID("{8B8DBBBF-4A2E-4B79-9154-738CB28D2C17}");
            }
        }
        public struct PasswordDisavow
        {
            public static readonly ID ID = new ID("{C0374D02-E97B-4341-9B69-0C0298A6A93E}");
            public struct Fields
            {
                public static readonly ID ErrorPage = new ID("{4D260E24-B283-4100-A7E3-4CD6C6FDB17B}");
            }
        }

        public struct DuplicateRegistrationTemplate
        {
            public static readonly ID ID = new ID("{B29234EF-1599-46A4-98F4-A3CBD2D9EE58}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{8316D269-752A-40C7-B7F5-189BFF34899F}");
                public static readonly ID General_Intro= new ID("{7CDF139D-9EB9-4EA0-8E63-F2A1AE854599}");
                public static readonly ID AccountInfo = new ID("{899469CB-3672-4AC2-865F-6281F3E89214}");
                public static readonly ID Explanation = new ID("{AC2B9D8D-3BD4-4436-A5B1-00570D121133}");
                public static readonly ID SubmitButton = new ID("{99EB7F43-1118-4D8B-8C99-F350D6386A33}");                
                public static readonly ID General_Loader = new ID("{A846BD39-8485-4B5C-9457-B4B4F1D389BA}");
                public static readonly ID LoaderImage = new ID("{2B7E9D6F-72A7-4FF0-9F64-13C811AEA216}");
                public static readonly ID Error = new ID("{3BF9BBED-424A-4CC0-AFFD-DB64C634FA38}");

                public static readonly ID SelectedEmail = new ID("{8C731811-55CC-4D33-8C59-526614A23BCE}");
                public static readonly ID UnselectedEmails = new ID("{F74C0EDD-F0F3-4D3A-AFE2-07D57E046B47}");
                public static readonly ID ApplyChanges = new ID("{E3AE3768-8956-4F28-B354-16DD119A14CE}");
                public static readonly ID SelectOther = new ID("{98827D68-1B0B-4A5F-A230-D7C69DD1930C}");
                public static readonly ID AccountSelected_Loader = new ID("{D26E6278-BE2E-4B9E-B663-9DB560CCB8BA}");
                public static readonly ID Password = new ID("{49B75B7F-783D-4B76-B3CE-96A38C36097D}");
                public static readonly ID InvalidCurrentPassword = new ID("{6822FB3F-A5A1-43CA-BD2A-B96203FC6C12}");
                public static readonly ID EmptyPassword = new ID("{3AC995AE-0042-4B17-977A-00FC1FA5A029}");

                public static readonly ID Confirmation_Title = new ID("{B2364359-9973-4AB8-A5C2-A7856CC37380}");
                public static readonly ID Username = new ID("{27C391A1-0D18-483C-88C9-56CCFB7AD149}");
                public static readonly ID ContinueButton = new ID("{72171DC8-C62E-4213-B79B-C186115CA406}");
               
            }
        }

        public struct DuplicateRegistrationPage
        {
            public static readonly ID ID = new ID("{DFA7FBD1-EB44-4DF3-BF86-1438F7ADEFE4}");
        }

        public struct UnsubscribeMail
        {
            public static readonly ID ID = new ID("{23907EDF-2161-4F47-96D4-7B18D16AA3C6}");
            public struct Fields
            {
                public static readonly ID Failure = new ID("{20B4C3DC-5314-4C08-BA32-D20084AD6EC0}");
            }
        }
        public struct SiteSettingsGlobal
        {
            public static readonly ID ID = new ID("{46D06DD7-8928-43F3-AC91-64FFC27778DC}");
        }
        public struct SiteSettings
        {
            public struct Fields
            {
                public static readonly ID MobileLogin = new ID("{1C983212-48B6-4FBE-B1BB-541121E980C1}");
            }
        }
        public struct ProductDetailPageType
        {
            public static readonly ID ID = new ID("{E20C8AEE-8D86-4564-BB55-208152C4D7EB}");
        }
        public struct LoginGlobal
        {
            public static readonly ID ID = new ID("{320FB78C-3992-47D9-8820-98D5C6A58D2D}");
        }

        public struct PassThroughAuthenticationTemplate
        {
            public static readonly ID ID = new ID("{CF112106-8F18-4D78-A090-D8D1BCB633FB}");

            public struct Fields
            {
                public static readonly ID Headline = new ID("{2E33EBAD-CA45-45DD-AA38-5364A00B5F05}");
                public static readonly ID Subheadline = new ID("{0E1786BE-E093-4B00-8240-6031BEA337A9}");
                public static readonly ID Logo = new ID("{7C8FE395-0E26-4531-A753-02B9B9CFB82E}");
                public static readonly ID Password = new ID("{A6E86BB9-42A8-4E9C-81D3-001EC195CB63}");
                public static readonly ID PasswordFieldText = new ID("{71C0F93F-8D15-4DD9-AD6C-57987190827B}");
                public static readonly ID LoginButton = new ID("{241D5C4D-4CAB-43D9-B2D2-E676F9E53573}");
                public static readonly ID ProductCode = new ID("{1D2360BA-9F6B-4BC8-8698-0A82B705E067}");

                public static readonly ID EmptyPassword = new ID("{952C7EDC-D943-4E2E-B9F5-2AB3AFC76567}");
                public static readonly ID InvalidCredentials = new ID("{FF7EB022-CE02-4343-8EAE-DABCA0420C52}");
                public static readonly ID AccountLocked = new ID("{D3B81327-4966-40D0-AA71-0AA0D9549ABD}");
                public static readonly ID AccountAlreadyLocked = new ID("{3ED243BB-2F39-4C00-A5D8-87B08E6BD356}");
                public static readonly ID TimeOut = new ID("{AECF6D74-0AA0-475F-9D74-9185C2583579}");
                public static readonly ID NotEligible = new ID("{92D789EA-38B6-4C6B-B8B6-B554FCE204FF}");
                public static readonly ID DefaultClickSaveURL = new ID("{52D5D559-2113-4E63-A962-CF187F5979AB}");
                public static readonly ID PasswordReset = new ID("{46191A1B-8FEE-4933-8C23-65915D803B65}");

            }

        }
        public struct MarketPlacePageType
        {
            public static readonly ID ID = new ID("{7960D6F0-09C8-451F-ABB5-6E36761009AE}");
        }
    }
}