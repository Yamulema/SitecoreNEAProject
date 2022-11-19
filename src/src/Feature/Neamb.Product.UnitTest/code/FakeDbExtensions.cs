using System;

namespace Neambc.Neamb.Feature.Product.UnitTest {
	public static class FakeDbExtensions {
		public static T ExecuteAndReturn<T>(this T target, Action<T> action) {
			action(target);
			return target;
		}
	}
}
