using System;
using NUnit.Framework;
using Oshyn.Framework.UITesting.Page;
using FW = Oshyn.Framework.UITesting.NUnit;
using PageBase = Oshyn.Framework.UITesting.Page.PageBase;

namespace UI_NEAMB.Tests {
	public class TestBase<T> : FW.TestBase<T> where T : class, IPage {

		#region Public Methods
		public void SetupLarge() {
			Setup(PageBase.LargeBrowser, false);
		}
		public void SetupSmall() {
			Setup(PageBase.SmallBrowser);
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
		#endregion
	}
}