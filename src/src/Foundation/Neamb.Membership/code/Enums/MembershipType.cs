using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Membership.Enums
{
    public enum MembershipType
    {
        None = 0,
        [Description("Not a member")]
        NotAMember = 1,
        [Description("Active")]
        Active = 2,
        [Description("Active Professional Sub")]
        ActiveProfessionalSub = 3,
        [Description("Early Enroll Professional FT")]
        EarlyEnrollProfessionalFt = 4,
        [Description("Early Enroll Prof Sub")]
        EarlyEnrollProfSub = 5,
        [Description("Substitute")]
        Substitute = 6,
        [Description("Associate")]
        Associate = 7,
        [Description("Educational Support")]
        EducationalSupport = 8,
        [Description("Active Esp Sub")]
        ActiveEspSub = 9,
        [Description("Early Enroll ESP")]
        EarlyEnrollEsp = 10,
        [Description("Early Enroll Educ. Support Substitute")]
        EarlyEnrollEducSupportSubstitute = 11,
        [Description("Life")]
        Life = 12,
        [Description("Life/Non Working")]
        LifeNonWorking = 13,
        [Description("Active-Life/Retired-Lifetime")]
        ActiveLifeRetiredLifetime = 14,
        [Description("Active-Life/Retired-Annual")]
        ActiveLifeRetiredAnnual = 15,
        [Description("Life Non-working/Retired LifeTime")]
        LifeNonWorkingRetiredLifetime = 16,
        [Description("Life Non-working/Retired Annual")]
        LifeNonWorkingRetiredAnnual = 17,
        [Description("Staff")]
        Staff = 18,
        [Description("Staff/Non-Member Eligible for MB Program")]
        StaffNonMemberEligibleForMbProgram = 19,
        [Description("Staff/Non-Member - MB & NEA Staff")]
        StaffNonMemberMbAndNeaStaff = 20,
        [Description("Reserve")]
        Reserve = 21,
        [Description("Retired")]
        Retired = 22,
        [Description("Retired Lifetime")]
        RetiredLifetime = 23,
        [Description("Pre-Retired")]
        PreRetired = 24,
        [Description("Student")]
        Student = 25,
        [Description("Agency Shop Employee")]
        AgencyShopEmployee = 26,
        [Description("No National Mshp/MEA-MFT")]
        NoNationalMshpMeaMft = 27,
        [Description("NYSUT Retired")]
        NysutRetired = 28,
        [Description("NYSUT Associate Members")]
        NysutAssociateMembers = 29,
        [Description("NYSUT Staff")]
        NysutStaff = 30,
        [Description("NYSUT Non-Chartered Locals")]
        NysutNonCharteredLocals = 31,
        [Description("NYSUT Affiliated Groups")]
        NysutAffiliatedGroups = 32,
        [Description("TSTA State-Only Life Member")]
        TstaStateOnlyLifeMember = 33,
        [Description("NYSUT NEA Member")]
        NysutNeaMember = 34,
        [Description("NDU (ND AFT non-educational member)")]
        Ndu = 35,
        [Description("Complimentary")]
        Complimentary = 36,
        [Description("Test-Student")]
        TestStudent = 37,
        [Description("Test-Active")]
        TestActive = 38,
        [Description("Test-ESP")]
        TestEsp = 39,
        [Description("Test-Retired")]
        TestRetired = 40,
        [Description("Test-Pre-Retired")]
        TestPreRetired = 41,
        [Description("Test - SEIU Active")]
        TestSeiuActive = 42,
        [Description("Test - SEIU Retired")]
        TestSeiuRetired = 43,
        [Description("Test - SEIU Inactive")]
        TestSeiuInactive = 44,
        [Description("SEIU Active Members")]
        SeiuActiveMembers = 45,
        [Description("SEIU Fee Payer")]
        SeiuFeePayer = 46,
        [Description("SEIU Inactive")]
        SeiuInactive = 47,
        [Description("SEIU Retired")]
        SeiuRetired = 48,
        [Description("Associated Child")]
        AssociatedChild = 55,
        [Description("Clark County NV On Hold")]
        ClarkCountyNvOnHold = 56,
        [Description("Associated Parent")]
        AssociatedParent = 57,
        [Description("Associated Spouse/Domestic Partner")]
        AssociatedSpouseDomesticPartner = 58,
        [Description("Cancelled")]
        Cancelled = 59,
        [Description("Awaiting Verification")]
        AwaitingVerification = 60,
        [Description("New Member Awaiting Verification")]
        NewMemberAwaitingVerification = 61,
        [Description("SEIU Temp")]
        SeiuTemp = 62
    }
}