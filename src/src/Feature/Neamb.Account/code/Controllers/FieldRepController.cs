using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;

namespace Neambc.Neamb.Feature.Account.Controllers
{
    public class FieldRepController : BaseController
    {
        private readonly ICacheManager _cacheManager;
        public FieldRepController(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public ActionResult FieldRep()
        {
            return View("/Views/Neamb.Account/FieldRep.cshtml", CreateModel());
        }
        private FieldRepDTO CreateModel()
        {
            var fieldRepDTO = new FieldRepDTO(_cacheManager);
            fieldRepDTO.Initialize(RenderingContext.Current.Rendering);
            return fieldRepDTO;
        }
        [HttpPost]
        [ValidateFormHandler]
        
		public ActionResult FieldRep(FieldRepDTO model)
        {
            return Redirect(Sitecore.Links.LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(model.State)));
        }
    }
}