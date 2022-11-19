using System.Reflection;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
	public interface IResourcesService {
		string ReadTextResourceFromAssembly(string name);
		string ReadTextResourceFromAssembly(string name, Assembly asm);
	}
}