using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
	public class PdfRequest
	{
		public string ProductIemId { get; set; }
		public string Email { get; set; }
		public string PdTransDate { get; set; }
		public string PdFirstName { get; set; }
		public string PdLastName { get; set; }
		public string PdDob { get; set; }
		public int PdMdsid { get; set; }
		public string PdAddress { get; set; }
		public string PdCity { get; set; }
		public string PdState { get; set; }
		public string PdZip { get; set; }
		public string PdMemberType { get; set; }
		public string Custom1 { get; set; }
		public string Custom2 { get; set; }
		public string Custom3 { get; set; }
		public string Custom4 { get; set; }
		public string Custom5 { get; set; }
	}
}