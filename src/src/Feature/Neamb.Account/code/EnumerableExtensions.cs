using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Account {
	public static class EnumerableExtensions {
		public static IEnumerable<TResult> SelectWithIndex<TSource, TResult>(
			this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) {
			var ndx = 0;
			foreach (var item in source) {
				yield return selector(item, ndx);
				ndx++;
			}
		}
	}
}