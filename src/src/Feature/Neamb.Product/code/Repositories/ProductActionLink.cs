using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	public class ProductActionLink
	{
		public ProductAction GetActionData(string productCode, 
			bool hasCheckEligibility,
			Item contextItem,
			ID primaryCtaLinkItemId, ID componentId, ID eligibilityItemId) {
			ProductAction productAction= new ProductAction();

			var urlLink = contextItem.LinkFieldUrl(primaryCtaLinkItemId).Trim();
			if (hasCheckEligibility || urlLink.Contains("[materialid]") || urlLink.Contains("[mdsid]") ||
				urlLink.Contains("[mdsid_mercer]") || urlLink.Contains("[mdsid_clear]") ||
				urlLink.Contains("[mdsid_afinium]") || urlLink.Contains("[cellcode]") || urlLink.Contains("[campcode]") || urlLink.Contains("[medium]") || urlLink.Contains("[mercer]") || urlLink.Contains("[afinium]") || urlLink.Contains("[sob") || urlLink.Contains("[gclid") || urlLink.Contains("[term"))
			{
				var methodLink = $"executelink{componentId}";

				productAction.ActionOnClickLink =
					$"{methodLink}('{primaryCtaLinkItemId}','{contextItem.ID}'," +
					$"'{productCode}','{eligibilityItemId}'," +
					$"'{ConstantsNeamb.Primary}{componentId}');" +
					$"operationprocedureactioncta('{productCode}');return false;";

			}
			else
			{
				productAction.ActionLink = contextItem.LinkFieldUrl(primaryCtaLinkItemId).Trim();
			}
			return productAction;
		}
	}
}