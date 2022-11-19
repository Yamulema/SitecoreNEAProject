using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Feature.Cards.Repositories.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Cards.Repositories
{
    public abstract class PageCardDealer : IPageCardDealer
    {
        protected Item Datasource { get; set; }
        protected List<ID> TemplateFilters = new List<ID>();
        protected readonly ISearchManager _searchManager;
        protected readonly IGlobalConfigurationManager _globalConfigurationManager;
        protected readonly IGtmService _gtmService;
		protected PageCardDealer(ISearchManager searchManager, IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService)
        {
            _searchManager = searchManager;
			_globalConfigurationManager = globalConfigurationManager;
            _gtmService = gtmService;
		}

        /// <summary>
        /// Maps sitecore item to a given PageCard type model.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        protected abstract T GetPageCard<T>(Item page);
        
        #region Private Methods

        /// <summary>
        /// Gets all the cards defined by content.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="datasource"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetPageCards<T>(Item page, Item datasource) where T : PageCard;
        

        /// <summary>
        /// Gets all the pages defined under the Items field.
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        protected abstract IEnumerable<Item> GetUserDefinedPages(Item datasource);
        #endregion
    }
}