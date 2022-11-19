
using Neambc.Neamb.Foundation.MBCData.Enums;

namespace Neambc.Neamb.Foundation.MBCData.Services.SecurityManagement
{
    public interface ISecurityService
    {
        /// <summary>
        /// Encrypts a given piece of string using Advanced Encryption Standard (AES).
        /// </summary>
        /// <param name="mdsId">Mdsid user</param>
        /// <param name="token">Token type ex: Token/Afinium</param>
        /// <returns></returns>
        string AesEncrypt(string mdsId, Token token);
    }
}
