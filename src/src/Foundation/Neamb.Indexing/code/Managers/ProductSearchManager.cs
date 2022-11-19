using System;
using System.Linq;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.Indexing.Models;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Data;
using Neambc.Neamb.Foundation.Configuration.Manager;

namespace Neambc.Neamb.Foundation.Indexing.Managers
{
    [Service(typeof(IProductSearchManager))]
    public class ProductSearchManager : IProductSearchManager
    {
        protected ISearchIndex Index;
        private IGlobalConfigurationManager _globalConfigurationManager;

        public ProductSearchManager(IGlobalConfigurationManager globalConfigurationManager)
        {
            _globalConfigurationManager = globalConfigurationManager;
            Index = ContentSearchManager.GetIndex(_globalConfigurationManager.NeambIndex);            
        }

        public ProductResult GetContentPages(string productCode)
        {
            ProductResult resultDetail = null;
            using (var context = Index.CreateSearchContext())
            {
                try
                {
                    var productCodeId = GetProductByCode(productCode);
                    if(productCodeId == null || productCodeId == Guid.Empty)
                    {
                        return null;
                    }

                    var productCodeIdString = productCodeId.ToString().ToLower().Replace("-", "");
                    var query = PredicateBuilder.True<ProductResult>();

                    var productParentPredicate = PredicateBuilder.True<ProductResult>();
                    productParentPredicate = productParentPredicate.And(x => x._Parent == "bb7b06f276ae45e6ba698592f0eaddcd");
                    query = query.And(productParentPredicate.Boost(0f));

                    var productCodePredicate = PredicateBuilder.True<ProductResult>();
                    productCodePredicate = productCodePredicate.And(x => x.ProductCodeSm.Contains(productCodeIdString));
                    query = query.And(productCodePredicate.Boost(0f));

                    //Max number of records for the search results
                    var queryable = context.GetQueryable<ProductResult>().Where(query);

                    //Hits search engine.
                    var result = queryable.GetResults();
                    if (result.Hits.Any())
                    {
                        var resultDetails = result.Hits.Select(x => x.Document).ToList();
                        resultDetail = resultDetails.FirstOrDefault(x => x.ProductCodeSm.Contains(productCodeIdString));
                    }
                    return resultDetail;
                }
                catch (Exception ex)
                {
                    Log.Fatal("Exception on Search", ex, this);
                    return null;
                }
            }
        }

        private Guid GetProductByCode(string productCode)
        {
            var productCodeFieldID = new ID("{B01AD396-BC36-486A-839E-889926842C54}");
            var parentProductContainerID = new ID("{C4D7E815-5666-4991-9C17-949EF49970D3}");
            var productCodeId = Guid.Empty;

            var productcodesList = Sitecore.Context.Database.GetItem(parentProductContainerID);
            if (productcodesList == null)
            {
                return productCodeId;
            }

            var product = productcodesList.GetChildren().FirstOrDefault(x => x.Fields[productCodeFieldID].Value == productCode);

            if(product == null)
            {
                return productCodeId;
            }

            productCodeId = product.ID.Guid;

            return productCodeId;
        }
    }
}