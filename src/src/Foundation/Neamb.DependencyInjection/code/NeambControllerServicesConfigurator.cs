using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Neambc.Neamb.Foundation.DependencyInjection {
	public class NeambControllerServicesConfigurator : IServicesConfigurator {
		public void Configure(IServiceCollection serviceCollection) {
			//serviceCollection.Add(new ServiceDescriptor(
			//	typeof(Sitecore.Data.Database),
			//	(IServiceProvider provider) => Sitecore.Context.Database
			//	, ServiceLifetime.Transient
			//));
			serviceCollection.AddMvcControllers("*.Neamb.Feature.*");
			serviceCollection.AddMvcControllers("*.Neamb.Project.*");
			serviceCollection.AddClassesWithServiceAttribute("*.Neamb.Feature.*");
			serviceCollection.AddClassesWithServiceAttribute("*.Neamb.Foundation.*");
			serviceCollection.AddClassesWithServiceAttribute("*.Neamb.Project.*");
		}
	}
}