using System.Reflection;

namespace Neambc.Neamb.Foundation.Membership.Interfaces {
	public interface IResourcesService {
		string ReadTextResourceFromAssembly(string name);
		string ReadTextResourceFromAssembly(string name, Assembly asm);
	}
}