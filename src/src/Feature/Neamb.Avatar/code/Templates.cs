using Sitecore.Data;

namespace Neambc.Neamb.Feature.Avatar
{
	public struct Templates
	{
		public struct Avatar
		{
			public static readonly ID ID = new ID("{BCB561DA-F436-4BB8-812B-B716798B30D4}");

			public struct Fields
			{
				public static readonly ID BackLink = new ID("{F5DD0218-9910-4EB4-8662-E971F40F4F11}");
				public static readonly ID DefaultImage = new ID("{9E748A55-016E-44B1-BC42-E507357F9D76}");
				public static readonly ID Title = new ID("{408D275B-E42B-4662-A07E-97F0C0627E17}");
				public static readonly ID Description = new ID("{58CB6799-5A86-4040-ACC3-FD93088D575F}");
				public static readonly ID UploadButton = new ID("{8503FC15-6163-47CE-BDB0-B64CB464A230}");
				public static readonly ID SendButton = new ID("{AA24174D-3298-4A8F-AC26-19FCEB570D46}");
				public static readonly ID ErrorUpload = new ID("{DE8F48C0-C4F4-4589-843C-0DEE5649DC7E}");
				public static readonly ID ErrorImageSize = new ID("{C831E1FD-5253-466E-A3F3-78FFF1237FEA}");
				public static readonly ID AnonymousUser = new ID("{028A559A-6463-4469-B3AA-B197687DAAFC}");
				public static readonly ID ConfirmationUploadedImage = new ID("{C7C7C062-9BAC-428E-BB97-DA5D340DFDA6}");
			}
		}

		public struct PageAvatar
		{
			public static readonly ID ID = new ID("{60E5F3EF-3BD2-4F89-AED1-CBA9AE75C42A}");
		}

	}
	}