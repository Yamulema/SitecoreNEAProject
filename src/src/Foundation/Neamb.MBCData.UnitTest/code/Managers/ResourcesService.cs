using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Managers;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers {
	[TestFixture]
	public class ResourcesService {

		private SUT.ResourcesService _svc;

		[OneTimeSetUp]
		public void SetUpOnce() {
			_svc = new SUT.ResourcesService();
		}

		[Test]
		public void ReadTextResourceFromAssembly_ReadSuccessfully() {
			Assert.IsNotNull(_svc.ReadTextResourceFromAssembly(
                "Neambc.Neamb.Foundation.MBCData.UnitTest.Managers.SampleTextResource.txt"
            ));
		}

		[Test]
		public void ReadTextResourceFromAssembly_ReturnsNullWhenNotFound() {
			Assert.IsNull(_svc.ReadTextResourceFromAssembly("nope"));
		}
		[Test]
		public void ReadTextResourceFromAssembly_CanUseGivenAssembly() {
			var asm = GetType().Assembly;
			Assert.IsNotNull(_svc.ReadTextResourceFromAssembly(
                "Neambc.Neamb.Foundation.MBCData.UnitTest.Managers.SampleTextResource.txt",
				asm
			));
		}
	}
}
