using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Neambc.Neamb.Foundation.DependencyInjection {
	public class SeiumbControllerServicesConfigurator : IServicesConfigurator {
		public void Configure(IServiceCollection serviceCollection) {
			serviceCollection.AddMvcControllers("*.Seiumb.Feature.*");
			serviceCollection.AddMvcControllers("*.Seiumb.Project.*");
			serviceCollection.AddClassesWithServiceAttribute("*.Seiumb.Feature.*");
			serviceCollection.AddClassesWithServiceAttribute("*.Seiumb.Foundation.*");
			serviceCollection.AddClassesWithServiceAttribute("*.Seiumb.Project.*");
		}
	}
}