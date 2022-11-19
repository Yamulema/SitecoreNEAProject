using NUnit.Framework;
using Neambc.Neamb.Project.Web.UITest.Pages.Account;

namespace Neambc.Neamb.Project.Web.UITest.Tests.LargerBrowser.Account
{
    public class ManageProductandServicesTests : NeambTestBaseLarge<ManageProductandServicesPage>
    {
        #region Tests

        [Test, Category("ManageProductsServices")]
        public void PageOpenssuccessful()
        {
            Page.AssertIsLoaded();
            Page.AssertElementExists("ManagePS_Title");
            Page.AssertElementExists("ManagePS_f_offerlink1");
            Page.AssertElementExists("ManagePS_f_offerlink2");

        }

        #endregion

    }
}
