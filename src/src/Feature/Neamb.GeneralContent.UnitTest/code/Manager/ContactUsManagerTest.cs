using Moq;
using Neambc.Neamb.Feature.GeneralContent.Managers;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Feature.GeneralContent.UnitTest.Manager
{
    [TestFixture]
    public class ContactUsManagerTest
	{

		#region Fields
		private Mock<IGtmService> _gtmService;
		#endregion
		[SetUp]
		public void SetUp()
		{
			_gtmService = new Mock<IGtmService>();
		}
		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Return_ActionText_ContactUsUpdated()
		{
			string expectedValue = "dataLayerPush({'event':'account','accountSection':'contact us','accountAction':'Send Us a Message','ctaText':'Send Message'})";
			_gtmService.Setup(x => x.GetGtmEvent(It.IsAny<object>())).Returns(expectedValue);
			var contactUsManager = new ContactUsManager(
				_gtmService.Object);
			using (var db = new Db {
				new DbItem("contactus", new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")) {
					Fields = {
						new DbField("Title", new ID("{4C4363A1-F5F8-4787-9273-4C3AE95C38C3}")) {
							Value = "Send Message"
						},
						new DbField("Submit", new ID("{CBFAAA1F-574F-4FD2-A90D-14D1525A1364}")) {
							Value = "Send Message"
						},
					}
				}
			})
			{
				Sitecore.Data.Items.Item itemTest = db.GetItem(new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}"));
				var result = contactUsManager.GetGtmAction(itemTest);
				Assert.AreEqual(result, expectedValue);
			}
		}
	}
}
