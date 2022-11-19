using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Contest.Model
{
	public class ContestFileItem
	{
		public Guid Key { get; set; }
		public string FileName { get; set; }
		public string Webuserid { get; set; }
		public string Mdsid { get; set; }
	}
}