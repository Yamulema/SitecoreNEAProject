using System;
using System.IO;
using System.Reflection;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Membership.Interfaces;

namespace Neambc.Neamb.Foundation.Membership.Services {

	[Service(typeof(IResourcesService))]
	public class ResourcesService : IResourcesService {
		public string ReadTextResourceFromAssembly(string name) {
			return ReadTextResourceFromAssembly(name, Assembly.GetExecutingAssembly());
		}

		public string ReadTextResourceFromAssembly(string name, Assembly assembly) {
			var asm = assembly ?? throw new ArgumentNullException(nameof(assembly));
			string ret = null;
			using (var stream = asm.GetManifestResourceStream(name)) {
				if (null != stream) {
					using (var sr = new StreamReader(stream)) {
						ret = sr.ReadToEnd();
					}
				}
			}
			return ret;
		}
	}
}