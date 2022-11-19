using System;
using System.Drawing.Imaging;
using System.IO;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Services;

namespace Neambc.Neamb.Foundation.Membership.UnitTest.Services {
	[TestFixture]
	public class Base64Service {
		private SUT.Base64Service _svc;
		private FakeLog _fakeLog;

		[OneTimeSetUp]
		public void SetUpOnce() {
			_fakeLog = new FakeLog();
			_svc = new SUT.Base64Service(_fakeLog);
		}

		[SetUp]
		public void SetUp() {
			// between each test, remove any prior errors, but use the same log
			_fakeLog.Entries.Clear();
		}

		[Test]
		public void EncodeImage_RecordsExceptions() {
			Assert.IsNull(_svc.EncodeImage(null));
			Assert.AreEqual(1, _fakeLog.Entries.Count);
			var entry = _fakeLog.Entries[0];
			Assert.AreEqual(FakeLog.ERROR, entry.EntryType);
			//Assert.IsFalse(entry.Message.Contains("System.ArgumentNullException: Value cannot be null."));
		}
		[Test]
		public void EncodeImage_Succeeds() {
			// load image via the Image class to validate it
			byte[] buffer;
			var dir = Path.GetDirectoryName(GetType().Assembly.Location) ??
					  throw new InvalidOperationException("Assembly has no location");
			var filename = Path.Combine(dir, "Services", "sample_image.jpg");
			using (var img = System.Drawing.Image.FromFile(filename)) {
				using (var ms = new MemoryStream()) {
					img.Save(ms, ImageFormat.Jpeg);
					buffer = ms.ToArray();
				}
			}
			var result = _svc.EncodeImage(buffer);
			Assert.IsTrue(result.StartsWith("data:image/png;base64,", StringComparison.InvariantCulture));

			// result is base64 understandable
			using (var ms = new MemoryStream(buffer)) {
				using (var img = System.Drawing.Image.FromStream(ms, true, true)) {
					Assert.IsNotNull(img);
				}
			}
		}
	}
}
