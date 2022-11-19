using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Account
{
    public static class Configuration
    {
        public static string SubscriptionsResultParameterName => Sitecore.Configuration.Settings.GetSetting("SubscriptionsResult");
        public static string EditBeneficiaryIdParameterName => Sitecore.Configuration.Settings.GetSetting("EditBeneficiaryId");
        public static string ProfilePageId => Sitecore.Configuration.Settings.GetSetting("ProfilePageId");
        public static ID NamedIndividualId => new ID(Sitecore.Configuration.Settings.GetSetting("NamedIndividualId"));
        public static ID OtherEntityId => new ID(Sitecore.Configuration.Settings.GetSetting("OtherEntityId"));
        public static string ComplimentaryLifeResultParameterName => Sitecore.Configuration.Settings.GetSetting("ComplimentaryLifeResult");
        public static string WelcomeMdsidParameterName => Sitecore.Configuration.Settings.GetSetting("WelcomeMdsidParameterName");
        public static string WelcomeEmailParameterName => Sitecore.Configuration.Settings.GetSetting("WelcomeEmailParameterName");
        public static string CompLifeDefaultCampaignCode => Sitecore.Configuration.Settings.GetSetting("CompLifeDefaultCampaignCode");
        public static string CompLifeDefaultCellCode => Sitecore.Configuration.Settings.GetSetting("CompLifeDefaultCellCode");
        public static string CompLifeEmailCustomerKey => Sitecore.Configuration.Settings.GetSetting("CompLifeEmailCustomerKey");
        public static string CompLifeEmailDefaultCellCode => Sitecore.Configuration.Settings.GetSetting("CompLifeEmailDefaultCellCode");
        public static string CompLifeEmailDefaultCampaignCode => Sitecore.Configuration.Settings.GetSetting("CompLifeEmailDefaultCampaignCode");
        public static string CompLifeOtherEntityDefaultRelationship => Sitecore.Configuration.Settings.GetSetting("CompLifeOtherEntityDefaultRelationship");
        public static string CompLifeOtherEntityDefaultDisplayRelationship => Sitecore.Configuration.Settings.GetSetting("CompLifeOtherEntityDefaultDisplayRelationship");
    }
}