using Neambc.Neamb.Foundation.Config.Models;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Account.Models
{
    public interface IUsernameBasicDTO 
	{
		string FirstName { get; set; }

		string LastName { get; set; }
		string Zip { get; set; }

		List<ErrorStatusEnum> ErrorsFirstName { get; set; }
		List<ErrorStatusEnum> ErrorsLastName { get; set; }
		List<ErrorStatusEnum> ErrorsZip { get; set; }
	}
}
