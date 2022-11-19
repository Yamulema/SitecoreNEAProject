using Moq;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using SUT = Neambc.Neamb.Feature.Product;

namespace Neambc.Neamb.Feature.Product.UnitTest.Services {
	[TestFixture]
	public class StepperService {
		#region Fields
		private Mock<ISubscriptionsManager> _subscriptionsManager;
		private Mock<ISessionAuthenticationManager> _sessionAuthenticationManager;
		private Db _db;
		private SUT.Services.StepperService _sut;
		#endregion


		[SetUp]
		public void SetUp() {
			_subscriptionsManager = new Mock<ISubscriptionsManager>();
			_sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
			_sut = new Product.Services.StepperService(_subscriptionsManager.Object, _sessionAuthenticationManager.Object);
			_db = new Db{
				new DbItem("Newsletter", new ID("{C4B100BA-AA17-4611-84F3-75EB27800B09}")) {
					{Templates.Newsletters.Fields.Id,"1234"}
				}
			};
		}

		[TearDown]
		public void TearDown() {
			_db.Dispose();
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Run_Should_Execute_When_AccountMembership_IsNull() {
			//Arrange
			_sessionAuthenticationManager.Setup(x => x.GetAccountMembership()).Returns((AccountMembership)null);

			_db.Add(new DbItem("Home", new ID()) { { "Title", "Welcome!" } });
			var currentItem = _db.GetItem("/sitecore/content/home");

			//Act
			Assert.DoesNotThrow(() => _sut.Run(currentItem));

		}
		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Run_Should_Execute_When_Template_HasNoSteps() {
			//Arrange
			var genericPageTemplateId = ID.NewID;
			_sessionAuthenticationManager.Setup(x => x.GetAccountMembership()).Returns(new AccountMembership());
			_db.Add(new DbItem("Home", new ID(), genericPageTemplateId)
				{{"Title", "Welcome!"}, {Templates.NewsletterStep.Fields.Enabled, "1"}});
			var currentItem = _db.GetItem("/sitecore/content/home");

			//Act
			Assert.DoesNotThrow(() => _sut.Run(currentItem));
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Run_Should_Execute_When_Template_HasNotImplementedSteps() {
			//Arrange
			var genericPageTemplateId = ID.NewID;
			var unImplementedStepTemplateId = ID.NewID;
			_sessionAuthenticationManager.Setup(x => x.GetAccountMembership()).Returns((AccountMembership)null);

			_db.Add(new DbTemplate("Step", Templates.Step.ID));
			_db.Add(new DbTemplate("UnknownStep", unImplementedStepTemplateId)
			{
					Templates.NewsletterStep.Fields.Enabled
				}.ExecuteAndReturn(x => x.BaseIDs = new ID[] { Templates.Step.ID }));
			_db.Add(new DbTemplate("GenericPage", genericPageTemplateId) {
				BaseIDs = new ID[] { unImplementedStepTemplateId }
			});
			_db.Add(new DbItem("Home", new ID(), genericPageTemplateId)
				{{"Title", "Welcome!"}, {Templates.NewsletterStep.Fields.Enabled, "1"}});
			var currentItem = _db.GetItem("/sitecore/content/home");

			//Act

			Assert.DoesNotThrow(() => _sut.Run(currentItem));

		}

		[Test, Combinatorial]
		[Ignore("Error in Fakedb to be fixed")]
		public void Run_Should_Execute_When_Template_HasImplementedSteps([Values("0", "1")]string enabled, [Values("{00000000-0000-0000-0000-000000000000}", "{C4B100BA-AA17-4611-84F3-75EB27800B09}", "")] string newsletterId) {
			//Arrange
			var genericPageTemplateId = ID.NewID;
			_sessionAuthenticationManager.Setup(x => x.GetAccountMembership()).Returns((AccountMembership)null);

			_db.Add(new DbTemplate("Step", Templates.Step.ID));
			_db.Add(new DbTemplate("NewsletterStep", Templates.NewsletterStep.ID)
			{
					Templates.NewsletterStep.Fields.Enabled
				}.ExecuteAndReturn(x => x.BaseIDs = new ID[] { Templates.Step.ID }));
			_db.Add(new DbTemplate("GenericPage", genericPageTemplateId) {
				BaseIDs = new ID[] { Templates.NewsletterStep.ID }
			});
			_db.Add(new DbItem("Home", new ID(), genericPageTemplateId)
			{
					{"Title", "Welcome!"},
					{Templates.NewsletterStep.Fields.Enabled, enabled},
					{Templates.NewsletterStep.Fields.Newsletter, newsletterId}
				});
			var currentItem = _db.GetItem("/sitecore/content/home");

			//Act
			Assert.DoesNotThrow(() => _sut.Run(currentItem));

		}
	}
}
