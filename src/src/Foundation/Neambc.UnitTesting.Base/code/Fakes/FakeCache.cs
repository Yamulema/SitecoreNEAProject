using System;
using System.Collections.Generic;
using Sitecore.Caching;
using Sitecore.Caching.Generics;
using Sitecore.Data;
using Sitecore.Diagnostics.PerformanceCounters;

//  Intended for promotion to the Oshyn.Framework.UnitTest project
#pragma warning disable RECS0083 // Shows NotImplementedException throws in the quick task bar
namespace Neambc.UnitTesting.Base.Fakes {
	public class FakeCache : ICache {
		public void Clear() {
			throw new NotImplementedException();
		}

		public void Scavenge() {
			throw new NotImplementedException();
		}

		public int Count {
			get;
		}
		public bool Enabled {
			get; set;
		}
		public ID Id {
			get;
		}
		public long MaxSize {
			get; set;
		}
		public string Name {
			get;
		}
		public long RemainingSpace {
			get;
		}
		public bool Scavengable {
			get; set;
		}
		public long Size {
			get;
		}
		public AmountPerSecondCounter ExternalCacheClearingsCounter {
			get; set;
		}
		public void Add(string key, object data) {
			throw new NotImplementedException();
		}

		public void Add(string key, object data, TimeSpan slidingExpiration) {
			throw new NotImplementedException();
		}

		public void Add(string key, object data, DateTime absoluteExpiration) {
			throw new NotImplementedException();
		}

		public void Add(string key, object data, EventHandler<EntryRemovedEventArgs<string>> removedHandler) {
			throw new NotImplementedException();
		}

		public void Add(string key, object data, TimeSpan slidingExpiration, DateTime absoluteExpiration) {
			throw new NotImplementedException();
		}

		public void Add(string key, object data, TimeSpan slidingExpiration, DateTime absoluteExpiration, EventHandler<EntryRemovedEventArgs<string>> removedHandler) {
			throw new NotImplementedException();
		}

		public bool ContainsKey(string key) {
			throw new NotImplementedException();
		}

		public string[] GetCacheKeys() {
			throw new NotImplementedException();
		}

		public object GetValue(string key) {
			throw new NotImplementedException();
		}

		public void Remove(string key) {
			throw new NotImplementedException();
		}

		public ICollection<string> Remove(Predicate<string> predicate) {
			throw new NotImplementedException();
		}

		public object this[string key] => throw new NotImplementedException();

		public ICacheSizeCalculationStrategy CacheSizeCalculationStrategy {
			get;
		}
		public void RemovePrefix(string prefix) {
			throw new NotImplementedException();
		}

		public void RemoveKeysContaining(string keyPart) {
			throw new NotImplementedException();
		}
	}
}
#pragma warning restore RECS0083 // Shows NotImplementedException throws in the quick task bar
