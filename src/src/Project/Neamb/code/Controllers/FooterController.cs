using Neambc.Neamb.Project.Web.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;

namespace Neambc.Neamb.Project.Web.Controllers
{
    public class FooterController : BaseController
    {
        public ActionResult Footer()
        {
            return View("/Views/Neamb.Web/Renderings/Footer.cshtml", CreateModel());
        }
        private FooterDTO CreateModel()
        {
            var footerDTO = new FooterDTO();
            footerDTO.Initialize(RenderingContext.Current.Rendering);
            return footerDTO;
        }
    }
}