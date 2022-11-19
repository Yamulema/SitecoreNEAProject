

using System.ComponentModel;

namespace Neambc.Neamb.Foundation.Eligibility.Model
{
    /// <summary>
    /// Status that is returned in response of webservice ProductRulesCheck
    /// </summary>
    public enum EligibilityResultEnum
    {
	    [Description("None")]
		None = 0,
	    [Description("MdsidNotFound")]
        MdsidNotFound = 1,
		[Description("NotMember")]
        NotMember = 2,
		[Description("NotEligible")]
        NotEligible = 3,
		[Description("Restricted")]
        Restricted = 4,
		[Description("Eligible")]
        Eligible = 5
    }
}