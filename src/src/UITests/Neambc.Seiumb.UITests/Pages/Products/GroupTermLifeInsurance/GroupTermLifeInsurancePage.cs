using System;
using System.Threading;
using Neambc.Seiumb.UITests.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Seiumb.UITests.Pages.Products.GroupTermLifeInsurance
{
    public class GroupTermLifeInsurancePage : SeiumbPage
    {
        #region ControlKeys

        private const string RegisterButton = "RegisterButton";
        private const string LoginDesktopButton = "LoginDesktopButton";
        private const string LoginMobileButton = "LoginMobileButton";
        private const string PrimaryButton = "PrimaryButton";
        private const string SecondaryButton = "SecondaryButton";
        private const string PrimarySmallButtonWarn = "PrimarySmallButtonWarn";
        private const string PrimaryMediumButtonWarn = "PrimaryMediumButtonWarn";
        private const string SecondarySmallButtonWarn = "SecondarySmallButtonWarn";
        private const string SecondaryMediumButtonWarn = "SecondaryMediumButtonWarn";
        private const string EmbeddedLink = "EmbeddedLink";
        private const string DownloadLink = "DownloadLink";
        private const string CTALink = "CTALink";
        private const string ChatIcon = "ChatIcon";
        #endregion

        public GroupTermLifeInsurancePage(
            IWebDriver driver,
            ISettings settings) : base(driver, settings)
        {
        }

        public new GroupTermLifeInsurancePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[]
            {
                "RichText"
            });
            return this;
        }
        public new GroupTermLifeInsurancePage CheckGtmTrackingCold(string registerClickAction,string loginDesktopAction, string loginMobileAction)
        {
            var onclickRegister = GetElementFromControlKey(RegisterButton)?
                .GetAttribute("onclick");
            AssertIsTrue(AssertGTMCode(onclickRegister, registerClickAction), $"ClickAction {registerClickAction} doesn't match {onclickRegister}");

            var onclickLoginDesktop = GetElementFromControlKey(LoginDesktopButton)?
                .GetAttribute("onclick");
            AssertIsTrue(AssertGTMCode(onclickLoginDesktop, loginDesktopAction), $"ClickAction {loginDesktopAction} doesn't match {onclickLoginDesktop}");

            var onclickLoginMobile = GetElementFromControlKey(LoginMobileButton)?
                .GetAttribute("onclick");
            AssertIsTrue(AssertGTMCode(onclickLoginMobile, loginMobileAction), $"ClickAction {loginMobileAction} doesn't match {onclickLoginMobile}");

            return this;
        }

        public new GroupTermLifeInsurancePage CheckGtmTrackingWarm(string primaryClickAction, string secondaryClickAction)
        {
            var onclickPrimaryMedium = GetElementFromControlKey(PrimaryMediumButtonWarn)?
                .GetAttribute("onclick");
            AssertIsTrue(AssertGTMCode(onclickPrimaryMedium,primaryClickAction), $"ClickAction {primaryClickAction} doesn't match {onclickPrimaryMedium}");
            var onclickPrimarySmall = GetElementFromControlKey(PrimarySmallButtonWarn)?
                .GetAttribute("onclick");
            AssertIsTrue(AssertGTMCode(onclickPrimarySmall,primaryClickAction), $"ClickAction {primaryClickAction} doesn't match {onclickPrimarySmall}");

            var onclickSecondaryMedium = GetElementFromControlKey(SecondaryMediumButtonWarn)?
                .GetAttribute("onclick");
            AssertIsTrue(AssertGTMCode(onclickSecondaryMedium,secondaryClickAction), $"ClickAction {secondaryClickAction} doesn't match {onclickSecondaryMedium}");
            var onclickSecondarySmall = GetElementFromControlKey(SecondarySmallButtonWarn)?
                .GetAttribute("onclick");
            AssertIsTrue(AssertGTMCode(onclickSecondarySmall,secondaryClickAction), $"ClickAction {secondaryClickAction} doesn't match {onclickSecondarySmall}");

            return this;
        }

        public new GroupTermLifeInsurancePage CheckGtmTrackingHot(string primaryClickAction, string secondaryClickAction)
        {
            var onclickPrimary = GetElementFromControlKey(PrimaryButton)?
                .GetAttribute("onclick");
            AssertIsTrue(AssertGTMCode(onclickPrimary,primaryClickAction), $"ClickAction {primaryClickAction} doesn't match {onclickPrimary}");

            var onclickSecondary = GetElementFromControlKey(SecondaryButton)?
                .GetAttribute("onclick");
            AssertIsTrue(AssertGTMCode(onclickSecondary,secondaryClickAction), $"ClickAction {secondaryClickAction} doesn't match {onclickSecondary}");

            return this;
        }

        public GroupTermLifeInsurancePage CheckGtmActionEmbeddedLink(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            AssertElementExists(EmbeddedLink);
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               EmbeddedLink, "CheckGtmActionEmbeddedLink");
            return this;
        }

        public GroupTermLifeInsurancePage CheckGtmActionDownloadLink(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            AssertElementExists(DownloadLink);
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               DownloadLink, "CheckGtmActionDownloadLink");
            return this;
        }

        public GroupTermLifeInsurancePage CheckGtmActionCTA(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            AssertElementExists(CTALink);
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               CTALink, "CheckGtmActionCTA");
            return this;
        }

        private GroupTermLifeInsurancePage CheckGtmAction(string gtmDataLayer, string gtmEvent,
           string gtmModule, string gtmText, string controlKey, string section)
        {
            var onClickFunction = GetElementFromControlKey(controlKey)?
                .GetAttribute("onclick");

            var hasDatalayerFunction = onClickFunction.Contains(gtmDataLayer);
            var hasGTMEvent = onClickFunction.Contains(gtmEvent);
            var hasGTMModule = onClickFunction.Contains(gtmModule);
            var hasGTMText = onClickFunction.Contains(gtmText);

            AssertIsTrue(hasDatalayerFunction, $"Error in {section} checking {gtmDataLayer}");
            AssertIsTrue(hasGTMEvent, $"Error in {section} checking {gtmEvent}");
            AssertIsTrue(hasGTMModule, $"Error in {section} checking {gtmModule}");
            AssertIsTrue(hasGTMText, $"Error in {section} checking {gtmText}");
            return this;
        }

        //public GroupTermLifeInsurancePage VerifyChatIcon()
        //{
        //    Thread.Sleep(6000);
        //    AssertElementExists(ChatIcon, "There is no Chat Icon present", 10);
        //    return this;
        //}
        public GroupTermLifeInsurancePage VerifyChatIcon()
        {
            Thread.Sleep(3000);
            By elementToCheck = By.XPath(ChatIcon);
            AssertIsTrue(!Exists(elementToCheck), "Chat Icon is present in GroupTermLifeInsurancePage Page");
            return this;
        }

        public bool Exists(By by)
        {
            return Driver.FindElements(by).Count != 0;
        }
    }
}
