using System.Threading;
using Neambc.Seiumb.UITests.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Seiumb.UITests.Pages.Products.StudentLoanRefinanceProgram
{
    public class StudentLoanRefinanceProgramPage : SeiumbPage
    {
        #region ControlKeys

        private const string PrimaryButton = "PrimaryButton";
        
        #endregion

        public StudentLoanRefinanceProgramPage(
            IWebDriver driver,
            ISettings settings) : base(driver, settings)
        {
        }

        public new StudentLoanRefinanceProgramPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[]
            {
                "RichText"
            });
            return this;
        }
        public StudentLoanRefinanceProgramPage ClickCtaButtonHotState()
        {
            AssertClick(PrimaryButton, timeoutSeconds: 30);
            return new StudentLoanRefinanceProgramPage(this.Driver, this.Settings);
        }

        public StudentLoanRefinanceProgramPage AssertHotStateCtaButton(string urlexpected)
        {
            Thread.Sleep(8000);
            var urlOpened = this.Driver.Url;
            AssertIsTrue(urlOpened.Contains(urlexpected));
            return this;
        }

    }
}
