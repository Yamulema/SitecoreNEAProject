using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent.Interfaces
{
	public interface ICompLifeMemberEligibilityManager
	{
		bool GetResultEligibility(string value);
	}
}