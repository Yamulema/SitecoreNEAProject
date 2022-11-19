using System;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Manager {
	[Service(typeof(IProductManager))]
	public class ProductManager : IProductManager {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly IOracleDatabase _oracleManager;
		private readonly ISessionManager _sessionManager;
        private readonly IMdsLoggingManager _mdsLoggingManager;
        private readonly IAnalyticsManager _analyticsManager;

        public ProductManager(
            ISessionAuthenticationManager sessionAuthenticationManager,
            IOracleDatabase oracleManager,
			ISessionManager sessionManager, IMdsLoggingManager mdsLoggingManager, IAnalyticsManager analyticsManager) {
            _sessionAuthenticationManager = sessionAuthenticationManager;
			_oracleManager = oracleManager;
			_sessionManager = sessionManager;
            _mdsLoggingManager = mdsLoggingManager;
            _analyticsManager = analyticsManager;
        }

        /// <summary>
		/// Insert into mds when the user click in cta action in product component
		/// </summary>
		/// <param name="productcode">Product code</param>
		public void ExecuteMdsLoggingProcessCta(string productcode) {
			ExecuteMdsLoggingProcessInner(productcode, ConstantsNeamb.CtaClickProductCode, "C");
		}

		/// <summary>
		/// Insert into mds when the user click in efulfillment click in product component
		/// </summary>
		/// <param name="materialId">Material id</param>
		public void ExecuteMdsLoggingProcessMaterial(string materialId) {
			ExecuteMdsLoggingProcessInner("", ConstantsNeamb.MaterialClickProductCode, "", materialId);
		}

        public void ExecuteMdsLoggingProcessInner(
            string productCode,
            string nameForSession,
            string typeProcess,
            string materialId = ""
        ) {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            var mdsId = accountMembership.Mdsid;
            var cellCode = _sessionAuthenticationManager.GetCellCode();
            _mdsLoggingManager.ExecuteMdsLoggingProcess(productCode,nameForSession,typeProcess,mdsId,cellCode,materialId);
        }

        /// <summary>
        /// Insert into mds when the user visited the product page
        /// </summary>
        /// <param name="productCode">Product code</param>
        public void ExecuteMdsLoggingProcessView(string productCode) {
			ExecuteMdsLoggingProcessInner(productCode, ConstantsNeamb.ViewProductCode, "V");
		}

		public AccountUserBase GetAccountUser(AccountMembership accountMembership) {
			return new AccountUserBase {
				Mdsid = accountMembership.Mdsid,
				Profile = accountMembership.Profile,
				Username = accountMembership.Username
			};
		}

		/// <summary>
		/// Get the object to be sent to the GTM in product dimensions
		/// </summary>
		/// <param name="renderingItem">Rendering item</param>
		/// <returns>GTM object</returns>
		public ProductCustomDimension GetProductDimensions(Item renderingItem)
		{
			var categoryGtmValue = renderingItem[Templates.ProductCTAs.Fields.Category];
			string categoryGtm = ConstantsNeamb.ProductDimensionNone;
			if (categoryGtmValue != null && !string.IsNullOrEmpty(categoryGtmValue))
			{
				var categoryItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(categoryGtmValue));
				categoryGtm = categoryItem[Templates.CategoryItem.Fields.Value];
			}

			var subCategoryGtmValue = renderingItem[Templates.ProductCTAs.Fields.SubCategory];
			string subCategoryGtm = ConstantsNeamb.ProductDimensionNone;
			if (subCategoryGtmValue != null && !string.IsNullOrEmpty(subCategoryGtmValue))
			{
				var subCategoryItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(subCategoryGtmValue));
				subCategoryGtm = subCategoryItem[Templates.CategoryItem.Fields.Value];
			}

			var subGroupGtmValue = renderingItem[Templates.ProductCTAs.Fields.SubGroup];
			string subGroupGtm = ConstantsNeamb.ProductDimensionNone;
			if (subGroupGtmValue != null && !string.IsNullOrEmpty(subGroupGtmValue))
			{
				var subGroupItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(subGroupGtmValue));
				subGroupGtm = subGroupItem[Templates.CategoryItem.Fields.Value];
			}
			var productCustomDimension = new ProductCustomDimension()
			{
				ProductCategory = categoryGtm,
				ProductSubcategory = subCategoryGtm,
				ProductSubgroup = subGroupGtm
			};
			return productCustomDimension;
		}

        /// <summary>
        /// Set Goal programatically CTA action
        /// </summary>
        /// <param name="productId">product rendering id</param>
        /// <param name="goalId">Goal id</param>
        public void SetGoalsProducts(string productId, string goalId) {
            var productItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(productId));

            var items = ((Sitecore.Data.Fields.MultilistField) productItem.Fields[new ID(goalId)]).GetItems();
            foreach (var goalItem in items) {
                _analyticsManager.SetGoal(goalItem);}

        }
    }
}