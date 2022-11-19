using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Rakuten.Manager;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.DependencyInjection;
using System;

namespace Neamb.Project.Common
{
    public partial class RakutenRegister : System.Web.UI.Page {
        public virtual ISeiumbProfileManager SeiumbProfileManager => (ISeiumbProfileManager)ServiceLocator.ServiceProvider.GetService(typeof(ISeiumbProfileManager));
        public virtual IRakutenRegistrationSeiumbManager RakutenRegistrationSeiumbManager => (IRakutenRegistrationSeiumbManager)ServiceLocator.ServiceProvider
            .GetService(typeof(IRakutenRegistrationSeiumbManager));

        protected void Page_Load(object sender, EventArgs e) {
            GetRakutenMemberData();
        }
        private void GetRakutenMemberData() {
            var isRakutenMember = SeiumbProfileManager.IsRakutenMember();
            IsRakutenMember.Text = isRakutenMember ? "YES" : "NO";
            if (!isRakutenMember) return;
            var rakutenResponse = SeiumbProfileManager.GetRakutenMemberCreationResponse();
            Id.Text = rakutenResponse.Id;
            EBToken.Text = rakutenResponse.EBtoken;
            CreationDate.Text = rakutenResponse.CreatedDate.ToString();
            Emailaddress.Text = rakutenResponse.EmailAddress;
        }

        protected void buttonRegister_Click(object sender, EventArgs e) {
            var cellCode = System.Web.HttpContext.Current.Session[ConstantsSeiumb.SourceCode] != null ? System.Web.HttpContext.Current.Session[ConstantsSeiumb.SourceCode].ToString() : null;
            RakutenRegistrationSeiumbManager.CheckSignUpRakutenUser(cellCode);
            GetRakutenMemberData();
        }
    }
}