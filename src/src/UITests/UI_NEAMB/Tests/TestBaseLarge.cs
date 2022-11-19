using NUnit.Framework;
using Oshyn.Framework.UITesting.Page;
using FW = Oshyn.Framework.UITesting.NUnit;

namespace UI_NEAMB.Tests {

	public abstract class TestBaseLarge<T> : FW.TestBase<T> where T : class, IPage {

		[SetUp]
		public void SetUp() {
			Setup(PageBase.LargeBrowser);
		}

		[OneTimeSetUp]
		public void SetupAll() {
			OneTimeSetUp();
		}
		[TearDown]
		public void Teardown() {
			TearDown();
		}
		[OneTimeTearDown]
		public void TearDownAll() {
			OneTimeTearDown();
		}
	}
}
