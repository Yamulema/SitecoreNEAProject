using System.Linq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	[Service(typeof(IBaseUrlActionDatapass))]
	public class BaseUrlActionDatapass : IBaseUrlActionDatapass
	{
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		public BaseUrlActionDatapass(IGlobalConfigurationManager globalConfigurationManager) {
			_globalConfigurationManager = globalConfigurationManager;
		}
		public string GetBaseUrlPartner(string productCode)
		{
			var productCodeAmericanFidelity = _globalConfigurationManager.ProductCodeAmericanFidelity.Split('|');
			var productCodeClickAndSave = _globalConfigurationManager.ProductCodeClickAndSave.Split('|');
			var productCodeJeepZag = _globalConfigurationManager.ProductCodeJeepZag.Split('|');
			var productCodeMercer = _globalConfigurationManager.ProductCodeMercer.Split('|');

			if (productCodeAmericanFidelity.Contains(productCode))
			{
				return _globalConfigurationManager.UrlAmericanFidelity;
			}
			else if (productCodeClickAndSave.Contains(productCode))
			{
				return _globalConfigurationManager.UrlClickAndSave;
			}
			else if (productCodeJeepZag.Contains(productCode))
			{
				return _globalConfigurationManager.UrlJeepZag;
			}
			else if (productCodeMercer.Contains(productCode))
			{
				return _globalConfigurationManager.UrlMercer;
			}
			else
			{
				return "";
			}
		}
	}
}