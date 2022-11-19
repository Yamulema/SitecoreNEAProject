using Neambc.Neamb.Project.Web.UITest.Pages.ContactUs;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.LargerBrowser.ContactUs {
	[TestFixture]
	public class ContactUsTests : NeambTestBaseLarge<ContactUsPage> {

		#region Tests

		[Test, Category("ContactUs")]
		public void AllControlExist() { Page.ValidateContactUsFields(); }

		#endregion

	}
}
