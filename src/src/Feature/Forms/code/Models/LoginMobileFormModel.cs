using Neambc.Seiumb.Feature.Forms.Enums;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class LoginMobileFormModel : LoginModel
    {
       public string PreviousPage { get; set; }
        public string ActionUrl { get; set; }
        public string ActionKind { get; set; }
        public string ActionTitle { get; set; }
        public string ActionProcedurePar1 { get; set; }
        public string ActionProcedurePar2 { get; set; }
        public string MaterialId { get; set; }
		public string MdsId { get; set; }
		public string Action { get; set; }
        public string ProductName { get; set; }
        public string ActionType { get; set; }
        public string Contextitemid { get; set; }
        public string Calllinkid { get; set; }
        public string Actiontarget { get; set; }
        public string Postparameterid { get; set; }
    }
}