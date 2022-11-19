using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.NUnit;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Tests {
	public abstract class NeambTestBaseLarge<T> : TestBase<T> where T : class, IPage {

		[SetUp]
		public void Setup() {
            // force login for each test
            Page?.Driver?.Manage().Cookies.DeleteAllCookies();
            //Setup(PageBase.LargeBrowser);
            Setup(new System.Drawing.Size(1600, 900));
            
        }
		[OneTimeSetUp]
		public void OneTimeSetUpLocal() {
			base.OneTimeSetUp();
		}
		[TearDown]
		public void TearDownLocal() {
            TearDown();
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success &&
                Page?.Driver != null)
            {
                Page.Driver.Close();
                Page.Driver.Dispose();
                Page = null;
            }
        }
		[OneTimeTearDown]
		public void OneTimeTearDownLocal() {
			base.OneTimeTearDown();
		}
	}
}
