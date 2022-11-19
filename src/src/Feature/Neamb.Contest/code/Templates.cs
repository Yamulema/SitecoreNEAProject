using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Contest
{
    public struct Templates
    {
        public struct ConstestSubmission
        {
            public static readonly ID ID = new ID("{3821604B-5F69-4D8B-91DB-2D9D6907C134}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{F1147965-4C58-4427-8609-81C2F08137CE}");
                public static readonly ID Description = new ID("{153AE9D5-0978-48ED-B07E-8D331771E640}");
                public static readonly ID FileName = new ID("{BE0B8ABB-D510-45B6-BDCF-D7926A4C872C}");
                public static readonly ID DisclosureText = new ID("{B1DAE03F-FCD8-4672-985A-AC4AAEDF6807}");
                public static readonly ID AddButton = new ID("{7A9C6ECF-0C18-4A21-B012-06D27AC33B09}");
                public static readonly ID SendButton = new ID("{7D8E815A-7C05-40C9-8612-C7440C94C908}");
                public static readonly ID AnonymousUser = new ID("{450AAC8F-F4E4-4A25-B840-5F1228CBA080}");
                public static readonly ID ThankyouMessage = new ID("{0EBF0805-77BC-46ED-9D41-E1BCDC0D5AB0}");
                public static readonly ID ClosedContestText = new ID("{78D845C9-29DC-4610-B8DF-836707446F64}");
                public static readonly ID AllowSize = new ID("{D5EA9A98-1B26-4D56-ACE3-732E8A5E2E64}");
                public static readonly ID AllowType = new ID("{10CF466C-CC73-4D7B-B2DE-7C30C0ECAB7E}");
                public static readonly ID StartSubmissionDate = new ID("{609F3E3F-2656-4E56-8714-B5FE79B9B590}");
                public static readonly ID EndSubmissionDate = new ID("{0B7DBD54-1197-466E-9E4A-6B1F4A599008}");
                public static readonly ID DestinationLink = new ID("{D8E55D66-0942-4F66-B38E-61C8F4EA5F76}");
                public static readonly ID Error = new ID("{A8E0EE72-E25C-48F2-A673-9FE533BCEB2B}");
                public static readonly ID EmptyField = new ID("{8F8D429E-333D-453E-BB6E-5863031F934C}");
                public static readonly ID CharacterLimit = new ID("{0634851D-A791-4081-B573-1812A065FBD3}");
                public static readonly ID InvalidCharacters = new ID("{754C69BE-1AB1-4604-AE36-39F2732947D4}");
                public static readonly ID MinimumCharacterLimit = new ID("{C74C82D2-F389-400B-B9C2-A6698D20ED51}");
                public static readonly ID ErrorTypeNotAllowed = new ID("{AE628654-335F-4ADE-93BC-3A09462249D9}");
                public static readonly ID ErrorSizeNotAllowed = new ID("{1D92E87A-DA5A-4E96-9904-36542DD2A934}");
                public static readonly ID UploadFileEmpty = new ID("{24B362DA-B886-4E39-8320-37B168342424}");
	            public static readonly ID ContestCode = new ID("{95BE9CC9-E983-46BE-8949-F2F204593E02}");
            }
		}

        public struct ConstestVote
        {
            public static readonly ID ID = new ID("{CEFF0DD5-20FD-40A9-ACD3-C624736B57A2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{6C9A3744-DE1F-43FF-86AF-328C9C0C87F0}");
                public static readonly ID Description = new ID("{FD555E77-C636-4037-BC86-ACAB1378C12A}");
                public static readonly ID AnonymousUser = new ID("{0A48F56B-2DC3-4363-BEA1-0101E6D16EA2}");
                public static readonly ID ClosedContestText = new ID("{77CB8C65-94E0-4F5F-9B11-052F662E2BD9}");
                public static readonly ID StartVoteDate = new ID("{99413B86-9314-4A70-864B-1087E345ADAB}");
                public static readonly ID EndVoteDate = new ID("{02C8FB0E-E80B-4968-9284-C2674EE064A6}");
                public static readonly ID VoteConfirmationMessage = new ID("{68ED71E7-7EE9-4C26-B6C7-7D5C8A66F9E2}");
                public static readonly ID VoteLimitNotification = new ID("{5A4F4CE3-69A7-40A6-A66F-098FDF20B05C}");
                public static readonly ID ContestVoteCode = new ID("{8E88FF3B-A014-440F-8CE7-67E1E5F6900E}");
                public static readonly ID Pagination = new ID("{F35B5A54-AB87-4F6B-A1A0-FCD1C414F751}");
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
                public static readonly ID InlineButtons = new ID("{ABBBEC1F-4935-4A2A-8586-D0417F65F797}");
                public static readonly ID CodeSnippet = new ID("{051E5ACF-950A-4733-B339-54AC5701B998}");
            }
        }
    }
}