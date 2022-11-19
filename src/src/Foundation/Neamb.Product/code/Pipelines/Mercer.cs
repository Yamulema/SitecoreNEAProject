using System;

namespace Neambc.Neamb.Foundation.Product.Pipelines
{
	public class Mercer : PartnerBase
	{
		public void Process(ProductPipelineArgs args)
		{
			if (programCodes.Contains(args.ProductCode))
			{
				var offering = string.Empty;
				var client = string.Empty;
				switch (args.ProductCode)
				{
					case "070 02":
					{
						offering = "ADD";
						client = "IMF_802";
						break;
					}
					case "070 05":
					{
						offering = "ADDP";
						client = "IMF_802";
						break;
					}
					case "070 10":
					{
						offering = "ADDAV";
						client = "IMF_802";
						break;
					}
					case "035 10":
					{
						offering = "GIF";
						client = "IMF_802";
						break;
					}
					case "035 14":
					{
						offering = "GTL";
						client = "IMF_802";
						break;
					}
					case "041 01":
					{
						offering = "LPGTL";
						client = "IMF_802";
						break;
					}
					case "421 02":
					{
						offering = "HIP";
						client = "EPS_NEA";
						break;
					}
					case "421 03":
					{
						offering = "HIPP";
						client = "EPS_NEA";
						break;
					}
				}
				var actionPrimary = string.Format("https://mags.mercer.com/mags/enroll/?APPLICATION=MAGS&FUNCTIONALITY=ENROLLMENT&FIRM=NEA&CLIENT={0}&OFFERING={1}&MDSID={2}", client,offering,args.MdsId);
				SetAction(actionPrimary, args);
			}
		}
	}
}