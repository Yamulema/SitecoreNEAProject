using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Product
{
    public struct Templates
    {
		public struct LoginPage
		{
			public static readonly ID ID = new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}");
		}

		public struct ProductCTAs
		{
			public static readonly ID ID = new ID("{CDCEBEEF-02EB-4CF2-86A3-41BC0D31F613}");

			public struct Fields
			{
				public static readonly ID Category = new ID("{AD8E3BA4-0806-4278-B131-DFFA69F84BC3}");
				public static readonly ID SubCategory = new ID("{9696566F-F13B-4CCE-B532-E94BFC8C2227}");
				public static readonly ID SubGroup = new ID("{F4A6A478-CD21-4E0F-ADA0-B3E065D55EC2}");
                public static readonly ID GoalTriggerPrimary = new ID("{1B9A2A00-62C3-4F67-8F68-FE8D9BF0B925}");
                public static readonly ID GoalTriggerSecondary = new ID("{904C25D4-43CE-4A61-AD84-866FAA37D1DD}");
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
                public static readonly ID ProductCodeDroplink = new ID("{5EEE1604-783B-49C1-AB00-60BA69EE79F2}");
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


        public struct CategoryItem
		{
			public static readonly ID ID = new ID("{D1402B59-E079-4856-9DFB-551B6C87B7AE}");

			public struct Fields
			{
				public static readonly ID Value = new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}");
			}
		}

        public struct ProductCtaLite
        {
            public static readonly ID ID = new ID("{5FF6A507-7D14-4EE5-B85B-12082E2C85D7}");
        }

        public struct PromoToutLite
        {
            public static readonly ID ID = new ID("{DA5CA49A-B5C7-4A4D-B3F2-822A73DBB367}");
        }
        public struct ProductPage
        {
            public static readonly ID ID = new ID("{D1889EB8-BE95-4E99-B8E9-3A0AEB8F4800}");
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
        public struct HomePage
        {
            public static readonly ID ID = new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}");
        }
    }
}