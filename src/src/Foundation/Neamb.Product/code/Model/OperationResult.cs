using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.Product.Model
{
	public class OperationResult
	{
		public string Url { get; set; }
		public ResultUrlEnum ResultUrl  { get; set; }
        public Dictionary<string, object> PostData { get; set; }
        public OperationResult()
        {
            PostData=new Dictionary<string, object>();
        }
    }
}