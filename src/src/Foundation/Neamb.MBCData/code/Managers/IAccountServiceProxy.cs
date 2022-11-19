using System;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers {

	public interface IAccountServiceProxy {

        /// <summary>
		/// Authenticate the user name and password with the webservice AuthenticatePassword
		/// </summary>
		/// <param name="username">Email</param>
		/// <param name="password">Password</param>
		/// <param name="unionId">Seiumb/Neamb</param>
		/// <returns>Webservice return object</returns>
        /// 
	string ExecuteEnrollmentQuery(string mdsid);
		Guid ExecuteEnrollmentGetLogin(Guid uniqueId);
		string ExecuteUpdateEnrollment(string address, string city, string statecode, string zipcode, string loginuserid, string firstname, string lastname, string birthdate, string mdsIndvId);
		
		/// <summary>
		/// Encrypt mdsid with service
		/// </summary>
		/// <param name="mdsid"></param>
		/// <returns></returns>
		string EncryptPartner(string mdsid, string partner);
    }
}