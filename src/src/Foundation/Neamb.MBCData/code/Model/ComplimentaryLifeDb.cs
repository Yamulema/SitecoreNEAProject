using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
    public class ComplimentaryLifeDb
    {
        public int IndvID { get; set; }
        public List<BeneficiaryDb> Beneficiaries { get; set; }
        public int? Flag { get; set; }
        public string CampCode { get; set; }
        public string CellCode { get; set; }

        public ComplimentaryLifeDb()
        {
            Beneficiaries = new List<BeneficiaryDb>();           
        }
    }
}