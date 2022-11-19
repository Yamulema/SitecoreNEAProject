using System;

namespace Neambc.Neamb.Foundation.DependencyInjection {
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class ServiceAttribute : Attribute {
		public Lifetime Lifetime { get; set; } = Lifetime.Singleton;
		public readonly Type ServiceType;
		public ServiceAttribute(Type serviceType) {
			ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
		}
	}
}