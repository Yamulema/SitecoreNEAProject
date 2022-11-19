using System;
using System.Reflection;

namespace Neambc.UnitTesting.Base.Fakes {
	public class InstanceFaker {
		public T Create<T>() where T : class {
			var ret = Activator.CreateInstance<T>();
			var pInfos = ret.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.Public);
			foreach (var pInfo in pInfos) {
				var pt = pInfo.PropertyType;
				if (pt == typeof(string)) {
					var name = pInfo.Name;
					pInfo.SetValue(ret, name);
				}
			}
			return ret;
		}
	}
}
