using Neambc.Neamb.Foundation.Config.Models;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Account.Models
{
	public interface IPasswordDTO 
	{
		string Password { get; set; }

		string ConfirmPassword { get; set; }
		List<ErrorStatusEnum> ErrorsPassword { get; set; }
		List<ErrorStatusEnum> ErrorsConfirmPassword { get; set; }
	}
}
