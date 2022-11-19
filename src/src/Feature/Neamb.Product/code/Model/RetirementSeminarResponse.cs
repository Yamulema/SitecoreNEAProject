using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Product.Model
{
    public class RetirementSeminarResponse
    {
        public bool HasError { get; set; }
        public bool ProcessedSucessfully { get; set; }
        public bool ErrorAuthentication { get; set; }

        public RetirementSeminarResponse() {
            HasError = false;
            ProcessedSucessfully = false;
            ErrorAuthentication = false;
        }
    }
}