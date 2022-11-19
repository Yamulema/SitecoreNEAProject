using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Project.Web.Repositories
{
	public interface IWebRepository
	{
		void SetWarmStatus(string mdsid);
		void ProcessCookieWarm();
		string GetLogoImage(string seaNumber);
	}
}