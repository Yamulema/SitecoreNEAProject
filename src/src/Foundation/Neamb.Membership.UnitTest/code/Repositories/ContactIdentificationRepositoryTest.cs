using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Membership.UnitTest.Managers {
	[TestFixture]
	public class ContactIdentificationRepositoryTest
    {

		#region Fields
		
		private ContactIdentificationRepository _sut;
		#endregion

		[SetUp]
		public void SetUp() {
			
			_sut = new ContactIdentificationRepository(
				
			);
		}
		[OneTimeSetUp]
		public void SetUpOnce() {
		}

		[Test]
		public void IdentifyUser_WhenIsNull() {
            var result = _sut.IdentifyUser(null);
            Assert.IsFalse(result);
        }

        [Test]
        public void IdentifyUser_WhenUserNameEmpty()
        {
            var result = _sut.IdentifyUser("");
            Assert.IsFalse(result);
        }

        //TODO  REVIEW if (Tracker.Current != null && Tracker.Current.IsActive && Tracker.Current.Session != null)
        //[Test]
        //public void IdentifyUser_WhenUserNameNotEmpty()
        //{
        //    var result = _sut.IdentifyUser("nea.owen@gmail.com");
        //    Assert.IsTrue(result);
        //}
    }
}

