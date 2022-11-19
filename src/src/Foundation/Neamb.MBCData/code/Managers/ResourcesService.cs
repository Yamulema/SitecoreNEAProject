using System;
using System.IO;
using System.Reflection;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Foundation.MBCData.Managers {

	[Service(typeof(IResourcesService))]
	public class ResourcesService : IResourcesService {
		public string ReadTextResourceFromAssembly(string name) {
			return ReadTextResourceFromAssembly(name, Assembly.GetCallingAssembly());
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
