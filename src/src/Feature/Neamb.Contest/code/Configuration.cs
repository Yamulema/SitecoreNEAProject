using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Contest
{
    public static class Configuration
    {
        public static string UserVoteKeyGroup => Sitecore.Configuration.Settings.GetSetting("UserVoteKeyGroup");
        public static string ContestKeyGroup => Sitecore.Configuration.Settings.GetSetting("ContestKeyGroup");
        public static string VoteKeyGroup => Sitecore.Configuration.Settings.GetSetting("VoteKeyGroup");
        public static int UserVoteCooldown => int.Parse(Sitecore.Configuration.Settings.GetSetting("UserVoteCooldown"));
        public static string S3VoteFolder => Sitecore.Configuration.Settings.GetSetting("S3VoteFolder");
        public static string S3VoteFileName => Sitecore.Configuration.Settings.GetSetting("S3VoteFileName");
        public static string S3SubmissionExtension => Sitecore.Configuration.Settings.GetSetting("S3SubmissionExtension");
        public static string SubmissionGroup => Sitecore.Configuration.Settings.GetSetting("SubmissionGroup");
        public static int SubmissionsCacheDuration => int.Parse(Sitecore.Configuration.Settings.GetSetting("SubmissionsCacheDuration"));
        public static ID SiteSettingsId => new ID(Sitecore.Configuration.Settings.GetSetting("SiteSettingsId"));
        public static int SubmissionPageSize => int.Parse(Sitecore.Configuration.Settings.GetSetting("SubmissionPageSize"));        
    }
}