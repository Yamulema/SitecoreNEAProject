using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Account.Models
{
	public enum AuthenticationResultEnum
	{
		Valid,
		Duplicated,
		ErrorFromService,
		ErrorNotValid,
		None
	}
}