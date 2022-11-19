using Sitecore.Data;

namespace Neambc.Seiumb.Foundation.Authentication
{
    public struct Templates
    {
        public struct RegistrationForm
        {
            public static readonly ID ID = new ID("{E3551C4B-E55D-4C9F-A490-D6B19F4E11AB}");
            
            public struct Fields
            {
                public static readonly ID FirstName_Label = new ID("{C87CBA78-6955-4BAB-8872-A5DDA2C11CEB}");
                public static readonly ID FirstName_Placeholder = new ID("{7610C9C3-784E-4C4F-887D-0111C4D6E73E}");
                public static readonly ID FirstName_HelpText = new ID("{71CB11AF-84AA-465D-B000-61C5B565EDCD}");
                public static readonly ID FirstName_ErrorMessage = new ID("{0060AD01-B2C5-4837-9063-950E5F02083E}");
				public static readonly ID FirstName_CharactersLimit = new ID("{BF2C8BB4-9754-4860-81E8-121967E7A609}");

				public static readonly ID LastName_Label = new ID("{E6CF0BB7-CCB6-40A4-996B-264EC06C19EB}");
                public static readonly ID LastName_Placeholder = new ID("{8B21B473-60E9-4B5E-8882-7350C8FA5357}");
                public static readonly ID LastName_HelpText = new ID("{B245D0B2-EDAA-4A45-AEFB-1157BE302A98}");
                public static readonly ID LastName_ErrorMessage = new ID("{8E4FC32A-4116-4DF4-B732-D57F5E854F93}");
				public static readonly ID LastName_CharactersLimit = new ID("{D96C6459-AB6E-4448-983E-31F1689AE098}");

				public static readonly ID Address_Label = new ID("{CA3237D2-53AD-4017-A53B-9CA9DA7295EB}");
                public static readonly ID Address_Placeholder = new ID("{22715A3D-6811-4BC8-B5EC-5BD17769ACA5}");
                public static readonly ID Address_HelpText = new ID("{8C24FE36-0FBC-4366-A4D4-DA0952E65128}");
                public static readonly ID Address_ErrorMessage = new ID("{057FC3B5-9BA5-4289-82AA-07F93D7143FD}");
				public static readonly ID Address_CharactersLimit = new ID("{F978920F-6E46-4FE2-A044-4EB407C5B56D}");

				public static readonly ID City_Label = new ID("{CD30460E-752E-485F-91CD-E75F9C781536}");
                public static readonly ID City_Placeholder = new ID("{89D23857-7339-482A-8427-E4359C095AD1}");
                public static readonly ID City_HelpText = new ID("{C34E7F0F-2382-410A-8B9B-9466D3631875}");
                public static readonly ID City_ErrorMessage = new ID("{810A22EC-A632-4B96-9748-EB20E9298EFE}");
				public static readonly ID City_CharactersLimit = new ID("{BFC46C8F-3054-4E7B-A5BE-0DF503378364}");

				public static readonly ID State_Label = new ID("{6B900E44-ED55-471D-8078-3D3A41329291}");
                public static readonly ID State_Placeholder = new ID("{EE54F453-030B-4FB0-856F-B1E4B62818E8}");
                public static readonly ID State_HelpText = new ID("{D41F4869-48A7-414B-8F2C-B5AE818930E4}");
                public static readonly ID State_ErrorMessage = new ID("{1ABD6447-EBED-429E-BFD4-E8962D835297}");

                public static readonly ID ZipCode_Label = new ID("{9CA35E50-9A1A-4E79-AAB5-0FC82A7DF816}");
                public static readonly ID ZipCode_Placeholder = new ID("{817036E7-7A98-4679-AE49-79AC18AC59C2}");
                public static readonly ID ZipCode_HelpText = new ID("{BA99355F-D2AC-49DC-8956-5645E8BDAFDA}");
                public static readonly ID ZipCode_ErrorMessage = new ID("{B1972EE3-B6F4-4DDB-9E58-6F4E968AE905}");
				public static readonly ID ZipCode_CharactersLimit = new ID("{9EDD1D77-04F2-4069-8621-EE2378745E99}");

				public static readonly ID DateOfBirth_Label = new ID("{F873A76B-FA18-4F6B-AC30-8701C8DA4648}");
                public static readonly ID DateOfBirth_Placeholder = new ID("{7FE60339-93B2-4A0C-BEBA-A2D0E0ED170B}");
                public static readonly ID DateOfBirth_HelpText = new ID("{8B843852-9457-4301-959C-B74CE1235EAE}");
                public static readonly ID DateOfBirth_ErrorMessage = new ID("{6E9C583C-DFD1-4338-93A8-904450DF9EDD}");
				public static readonly ID DateOfBirth_AgeRequirement = new ID("{5B0CB774-FEE6-45DE-A43E-E0162D0BF65F}");

				public static readonly ID Phone_Label = new ID("{F0D82D58-4964-4A08-99F2-30FBE426197B}");
                public static readonly ID Phone_Placeholder = new ID("{884853BA-8A68-4E2C-91F0-F8B4DE3CE7FE}");
                public static readonly ID Phone_HelpText = new ID("{99F4EE80-1215-4B33-A1B8-310F495334F1}");
                public static readonly ID Phone_ErrorMessage = new ID("{835369C8-50AD-4CEC-886D-06AF7FA57C82}");
				public static readonly ID Phone_CharactersLimit = new ID("{83581C12-615F-4BF2-88A6-2BE6D92CBA5E}");

				public static readonly ID Email_Label = new ID("{E1745377-26CF-4C3B-9DFF-960E398E0B24}");
                public static readonly ID Email_Placeholder = new ID("{D746755E-594B-429B-B621-B64F45515716}");
                public static readonly ID Email_HelpText = new ID("{C44F3F5C-A94D-4ECD-B42D-11C239897E36}");
                public static readonly ID Email_InUse = new ID("{B605D4BD-B117-4C16-8814-BB99BC2C76A2}");
                public static readonly ID Email_Warning = new ID("{087F46C3-54B7-47D0-A8DA-9E42AEDC999C}");
                public static readonly ID Email_Empty = new ID("{6AA5B755-FD02-4565-8894-3BE157F3415D}");
                public static readonly ID Email_CharactersLimit = new ID("{4ACC301A-675D-4D51-9909-27B4874B29DB}");
                public static readonly ID Email_InvalidFormat = new ID("{D39FA88B-C8A3-411E-8DCB-9AE6059DE05C}");

                public static readonly ID DesiredPassword_Label = new ID("{D5E26F70-9702-43A3-89B9-4178254F0E12}");
                public static readonly ID DesiredPassword_Placeholder = new ID("{5A6AB86F-A86F-45A1-9E81-E5FF38FEDDC2}");
                public static readonly ID DesiredPassword_HelpText = new ID("{BB8FAD17-C373-4200-B3AD-03DE33D67864}");
                public static readonly ID DesiredPassword_ErrorMessage = new ID("{8C1AB329-AD6C-437E-9DA9-A636FF71BA25}");

                public static readonly ID ConfirmPassword_Label = new ID("{B659DCCA-99E2-4D88-B100-BBED97191491}");
                public static readonly ID ConfirmPassword_Placeholder = new ID("{A89C006C-1607-468E-890E-C2F04E098A7B}");
                public static readonly ID ConfirmPassword_HelpText = new ID("{7CDC9B5A-0581-4ADA-8650-45E8DE55DA11}");
                public static readonly ID ConfirmPassword_ErrorMessage = new ID("{9987F6BA-A3BC-4022-9417-4649401FC9CD}");

                public static readonly ID Banner = new ID("{CA6F26EC-9FA0-4644-AFB1-70B248494429}");
                public static readonly ID Title = new ID("{E6B017D4-B218-4239-B6BD-A11F6C6F1EAB}");
                public static readonly ID UserIntro = new ID("{903CA71F-0CBC-4933-A9C7-D3DAA25986E6}");
                public static readonly ID PasswordRequirements = new ID("{5630CBF2-9263-47AB-BA4C-05C902B2AEFC}");
                public static readonly ID Asterisk = new ID("{4017F1BC-0803-4E57-BEDB-80CB18294286}");
                public static readonly ID OptIn = new ID("{E3C2BFD5-BD34-4625-97F9-8307F0FE38D2}");
                public static readonly ID ThankYouPage = new ID("{C04BFAB1-98F6-4ACC-9128-F9A2D3BC17A5}");
                public static readonly ID InvalidCharacters = new ID("{16153284-6C8E-48B5-98B6-5E7AD7304FE3}");
                public static readonly ID InvalidPassword = new ID("{9CA6DB16-3BE9-4EF4-A13A-7E39E14D21AC}");
                public static readonly ID ValidPassword = new ID("{8FF7E64B-CA9E-4501-9B5D-D0AD72B84673}");

                public static readonly ID SubmitButton = new ID("{713151EE-EFC6-4294-814B-F8C7D5AFC841}");
                public static readonly ID RightRail = new ID("{B1881726-718C-4382-87B2-B6D554AF52A4}");
            }
        }

        public struct LoginForm
        {
            public static readonly ID ID = new ID("{EF4EE7CA-CE03-491C-931F-E25465332261}");

            public struct Fields
            {
                public static readonly ID Logo = new ID("{D40384B3-37BD-47F8-B897-9AA3FC303D54}");
                public static readonly ID ResetLink = new ID("{E99AA538-CE55-40B7-99A2-AF4B836EF921}");
                public static readonly ID CancelLink = new ID("{BDDC4F61-51C2-4858-A73B-CECFCD2EF2AE}");
                public static readonly ID Image = new ID("{B6B427DB-1EBD-4007-A9D4-89833B9AD87D}");
                public static readonly ID Registered_Headline = new ID("{71C82F68-C765-49F4-8FE9-0078CB8C6F04}");
                public static readonly ID Registered_Subheadline = new ID("{C24EE1EC-2B80-4A5E-91D7-B9A109BC4A4D}");
                public static readonly ID EmailPlaceholder = new ID("{DEC7A189-AE49-412F-A3A7-56120AB8EA99}");
                public static readonly ID PasswordPlaceholder = new ID("{D6310F29-07F5-4617-A0B2-5FB05F9C2CBD}");
                public static readonly ID RememberMe = new ID("{A3914310-944E-4DE3-9CB7-0FB36FB7DE62}");
                public static readonly ID LoginButton = new ID("{B62FBAF3-11B8-4FB9-B01B-5022F0037656}");
                public static readonly ID ForgotCredentials = new ID("{E65B5404-616B-49C5-BBD6-4ACD9C9C4358}");
                public static readonly ID NotRegistered_Headline = new ID("{D999616E-C09D-4E01-BA13-114DEC22197E}");
                public static readonly ID NotRegistered_Subheadline = new ID("{5E6E62E8-CB22-4BE2-BB01-A2FBF5C6C2AE}");
                public static readonly ID RegisterButton = new ID("{53B7FF52-4272-48E8-A3EF-0CD5EFBA8D7B}");
                public static readonly ID Help = new ID("{73290CE9-AD8B-42EE-AAEE-64E19B2E5E03}");
                public static readonly ID EmptyField = new ID("{B4A2F730-A2E5-4753-B07C-1B532738E1DE}");
                public static readonly ID InvalidCredentials = new ID("{1ACE64B0-5102-431F-9375-9DF4521B3AC1}");
                public static readonly ID AccountLocked = new ID("{19CCB63C-3DBE-478F-B820-8DA70DEDA43F}");
                public static readonly ID AccountAlreadyLocked = new ID("{8FC290BF-7488-4D5C-9F15-FD57884CF745}");
                public static readonly ID AlreadyRegistered = new ID("{9BF20FFB-8A6E-4C46-AFB8-1D38D8793014}");
				public static readonly ID TimeOut = new ID("{2FA94994-0D59-4888-9400-2E1D5479A7DA}");
			}
		}

        public struct ProductDetailPageType
        {
            public static readonly ID ID = new ID("{E20C8AEE-8D86-4564-BB55-208152C4D7EB}");
        }
    }
}