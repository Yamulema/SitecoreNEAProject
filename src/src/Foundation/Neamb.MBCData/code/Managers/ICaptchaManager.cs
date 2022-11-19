using System.Collections.Generic;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    public interface ICaptchaManager
	{
		bool ExecutePostRecaptcha(string captchaVerification, string captchaScret);
	}
}