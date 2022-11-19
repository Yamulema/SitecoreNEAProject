using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using NUnit.Framework;
using Neambc.Neamb.Project.Web.UITest.Pages;
using Neambc.Neamb.Project.Web.UITest.Pages.Guide;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.Guide
{
    public class GuidePageTest : NeambTestBaseLarge<GuidePage>
    {
        #region Tests

        [Test, Category("Core")]
        public void DownloadPdfHotState()
        {
            Page.AssertClick("SS_SignIn");
            Page.LoginSS(NeambPageBase.JessicaEmail, NeambPageBase.JessicaPassword);
            Page.AssertIsLoaded();
            Page.AssertElementExists("Download_PDF");
        }

    }
}
#endregion