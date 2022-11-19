using Neambc.Neamb.Foundation.Config.Models;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class ErrorsViewModel
    {
        public bool HasGeneralError { get; set; }
        public bool HasDuplicateAccount { get; set; }
        public bool HasErrorPassword { get; set; }
        public bool HasErrorUsername { get; set; }
        public bool IsProcessedSucessfully { get; set; }
        public List<ErrorItem> ErrorList { get; set; }
    }

    public class ErrorItem
    {
        public string FieldName { get; set; }
        public List<string> ErrorType { get; set; }
        public List<ErrorStatusEnum> ErrorEnumList { get; set; }
    }
}