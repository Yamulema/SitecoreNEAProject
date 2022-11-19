using Sitecore.Data;

namespace Neambc.Seiumb.Feature.ContactUs
{
    public class Templates
    {
        public struct ContactUs
        {
            public static readonly ID ID = new ID("{C692B8D8-32B6-4BAB-99D9-D05A3806A770}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{73EB4DBB-CCED-4B4E-9C53-804B65F9BE37}");
                public static readonly ID ContactInfo = new ID("{E4771DF7-8B06-4BED-B2D8-A990F2668690}");
                public static readonly ID FormTitle = new ID("{718EEB75-D423-414E-B2B2-6715D9E7A575}");
                public static readonly ID Asterisk = new ID("{3608C1AB-2BF4-4DE8-A8B3-1A1030058AC1}");
                public static readonly ID SubmitButton = new ID("{AA89F9FF-434D-4659-A434-C58638743B80}");
                public static readonly ID FirstNameLabel = new ID("{7E94C873-C754-4591-8F4E-079AF3A0E1B7}");
                public static readonly ID FirstNamePlaceholder = new ID("{B6C6B959-722D-4852-A95E-277C7DE15BAF}");
                public static readonly ID FirstNameErrorMessage = new ID("{8279584A-91D9-4B0F-A37C-0806E637F60B}");
				public static readonly ID FirstNameErrorCharactersLimit = new ID("{279F5CE7-B385-496F-BFA8-568CFE05FC6B}");
				public static readonly ID LastNameLabel = new ID("{6FCF2EF9-9C7F-4B3C-A795-A53EB28991DE}");
                public static readonly ID LastNamePlaceholder = new ID("{B47671D4-F84A-4D52-AE71-A16AE6D448BE}");
                public static readonly ID LastNameErrorMessage = new ID("{9DC70B5D-3EAD-4B40-BDCE-A967C5203D56}");
				public static readonly ID LastNameErrorCharactersLimit = new ID("{4D4AD5E3-14EE-4654-A877-5A1E64C6AD26}");
				public static readonly ID EmailLabel = new ID("{2AC4EC02-E31B-4752-9B44-B7F39B2E0CC1}");
                public static readonly ID EmailPlaceholder = new ID("{7EE20E5B-AD0F-41A4-B39B-4E35F0BE23C4}");
                public static readonly ID Email_Empty = new ID("{F50A411B-D6BC-4959-A554-E0E8A49BD1E8}");
				public static readonly ID Email_CharactersLimit = new ID("{821A8A3F-8E08-479A-ABB6-7BF0B9B3280A}");
				public static readonly ID Email_InvalidFormat = new ID("{51FC1CE9-8857-447E-9C8C-53794DA71054}");
				public static readonly ID PhoneLabel = new ID("{DF40E090-E8B1-45B2-8FA3-0AED26ADEC43}");
                public static readonly ID PhonePlaceholder = new ID("{180734AF-BCDB-419E-BC94-8292623AE4EA}");
                public static readonly ID PhoneErrorMessage = new ID("{C7D3E622-2F70-4B51-893D-B8A8D5944627}");
				public static readonly ID PhoneErrorCharactersLimit = new ID("{F53C1AD4-88C3-48D1-9CCD-8F27721A005F}");
				public static readonly ID LocalUnionLabel = new ID("{7DCB1F58-2AC8-4350-B18E-EBFD13D0CE8A}");
                public static readonly ID LocalUnionPlaceholder = new ID("{5D18A973-914B-4335-A0B2-05AB2EC46C86}");
                public static readonly ID StateLabel = new ID("{9789D145-A95D-4C8D-BCDE-B16E72025888}");
                public static readonly ID StatePlaceholder = new ID("{675032C5-DB2B-4930-BC3D-23AE37E898C1}");
                public static readonly ID TopicLabel = new ID("{B3292186-70FC-47D8-8D99-D6672CF08CBF}");
                public static readonly ID TopicPlaceholder = new ID("{E466CEB4-BA44-48D9-BB53-BE61F96914D3}");
                public static readonly ID TopicErrorMessage = new ID("{AB35A653-A7A7-4B50-BC5F-74D1DF829EC8}");
                public static readonly ID MessageLabel = new ID("{3CE45352-1CF6-47F5-A382-EB5D85C135BF}");
                public static readonly ID MessageErrorMessage = new ID("{F10F3068-A705-4E67-A08A-55264337B8D5}");
                public static readonly ID InvalidCharacters = new ID("{7B6A246A-9B82-4C10-B5B7-3204EBC33130}");
                public static readonly ID CharactersCount = new ID("{879631CD-4D04-4E73-933E-AD9FBC4966ED}");
                public static readonly ID CaptchaError = new ID("{DB4D7B10-E8D8-45CB-BD9E-DAF95EE4CDD6}");
            }
        }
        public struct StatesGlobal
        {
            public static readonly ID ID = new ID("{F1FC2588-F957-4D1E-B6E3-42AAFC56419C}");
        }
        public struct TopicsGlobal
        {
            public static readonly ID ID = new ID("{AD5D2B08-DC4F-4229-BEA3-52A917C42251}");
        }
        public struct ThankYouPageTemplate
        {
            public static readonly ID ID = new ID("{B497F700-68C9-40CC-8FC5-1B6ADF3D9961}");
        }
        public struct ErrorPageTemplate
        {
            public static readonly ID ID = new ID("{D613828E-0662-45F9-9A35-D1C6C47271E8}");
        }
    }
}