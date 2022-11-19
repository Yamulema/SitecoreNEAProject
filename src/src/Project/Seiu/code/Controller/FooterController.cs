using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Seiumb.Project.Seiu.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Project.Seiu.Controller
{
    public class FooterController : SitecoreController
    {
        public ActionResult Footer()
        {
            var model = new Footer()
            {
                Datasource = GetDatasource()
            };
            return View("/Views/Web/Renderings/Footer.cshtml", model);
        }

        private Item GetDatasource() {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            return dataSource ??
                Sitecore.Context.Database.GetItem(Neambc.Seiumb.Project.Web.Templates.SitecoreExtensions.SiteSettingsGlobal.ID);
        }
    }
}