using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Account.Models
{
	public enum RelationshipEnum
	{
		[Description("01")]
		Spouce,
		[Description("07")]
		Sister,
		[Description("06")]
		Brother,
		[Description("35")]
		DomesticPartner,
		[Description("56")]
		Other
	}
}