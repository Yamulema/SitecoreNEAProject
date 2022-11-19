using Neambc.Neamb.Foundation.Config.Models;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Account.Models
{
	public interface IDateBirthDTO
	{
		string BirthDate { get; set; }
		List<ErrorStatusEnum> ErrorsBirthDate { get; set; }
	}
}
