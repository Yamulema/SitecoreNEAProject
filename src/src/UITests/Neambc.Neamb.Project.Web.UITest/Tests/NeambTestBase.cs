using NUnit.Framework;
using Oshyn.Framework.UITesting.NUnit;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Tests {
	public abstract class NeambTestBase<T> : TestBase<T> where T : class, IPage {

		[OneTimeSetUp]
		public void OneTimeSetUpLocal() {
			base.OneTimeSetUp();
		}
		[TearDown]
		public void TearDownLocal() {
			base.TearDown();
		}
		[OneTimeTearDown]
		public void OneTimeTearDownLocal() {
			base.OneTimeTearDown();
		}
	}
}
