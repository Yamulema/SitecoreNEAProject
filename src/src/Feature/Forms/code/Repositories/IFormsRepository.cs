using Neambc.Seiumb.Feature.Forms.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Seiumb.Feature.Forms.Repositories
{
    public interface IFormsRepository
    {
        void ValidateRetrieveUserName(RetrieveUserNameModel retrieveUserNameModel);

        void ValidateResetToken(string userName, string token, ResetPasswordModel resetPasswordModel);

        void ResetPassword(string userName, string newPassword, string confirmPassword, ResetPasswordModel resetPasswordModel);

        string GetDataCalculator(bool smoker, string age, string coverage);

        void CancelResetToken(string userName, PasswordDisavowModel passwordDisavowModel);

        void ChangeUsername(UsernameFormModel userNameFormModel, string mdsid,  string firstName, string lastName,
            string cellCode,  string msrName, string campaignCode, bool isnew);

        void DuplicateRegistrationExactTarget(string mdsid, string selectedUsername, string firstName, string lastName,
            string cellCode, string campaignCD, IEnumerable<string> removedEmails);

        void UnsubscribeList(int listid, string mdsid, UnsubscribeModel unsubscribeModel);

        bool UpdateUserStatus(string username);
    }
}