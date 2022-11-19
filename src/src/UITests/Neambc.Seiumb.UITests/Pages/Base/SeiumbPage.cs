using System;
using System.Linq;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Controls;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Seiumb.UITests.Pages.Base
{
    public class SeiumbPage : AssertingPageBase
    {
        #region Constants
        private static int Timeout => 30;
        #endregion
        private const string BasePageName = "SeiumbPage";
        #region ControlKeys
        private const string PasswordTextBox = "SeiumbPage_LoginModal_Password";
        private const string UsernameTextBox = "SeiumbPage_LoginModal_Username";
        private const string SignInButton = "SeiumbPage_LoginModal_LoginButton";
        private const string LoginLink = "SeiumbPage_HeaderCold_LoginLink";
        private const string SignOutLink = "SeiumbPage_SignOutLink";
        #endregion

        #region Constructors
        public SeiumbPage(
            string pageName,
            string urlPath,
            IWebDriver driver,
            ISettings settings)
            : base(pageName, urlPath, driver, settings)
        {
            MergeBaseControls();
        }
        public SeiumbPage(
            IWebDriver driver,
            ISettings settings)
            : base(driver, settings)
        {
            MergeBaseControls();
        }
        public SeiumbPage(
            string name,
            IWebDriver driver,
            ISettings settings)
            : base(name, driver, settings)
        {
            MergeBaseControls();
        }
        #endregion

        #region Public Methods
        public virtual SeiumbPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "HeaderCold"
            });
            return this;
        }

        public T Login<T>(string username, string password) where T : class
        {
            AssertControlClick(LoginLink);
            CleanFields();
            PerformLoginAction(username, password);
            return Activator.CreateInstance(typeof(T), this.Driver, this.Settings) as T;
        }

        public SeiumbPage ClickOnSignOutLink()
        {
            AssertClick(SignOutLink, timeoutSeconds: Timeout);
            return this;
        }

        public SeiumbPage GoToPage(string pageUrl)
        {
            GoTo(pageUrl);
            return new SeiumbPage("SeiumbPage", pageUrl, this.Driver, this.Settings);
        }
        public T GoToPage<T>(string pageUrl) where T : class, IPage
        {
            GoTo(pageUrl);
            var result = Activator.CreateInstance(typeof(T), this.Driver, this.Settings) as T;
            return result;
        }
        public T GoToExternalPage<T>(string pageUrl) where T : class, IPage
        {
            AssertIsTrue(!string.IsNullOrEmpty(pageUrl));
            this.Driver.Navigate().GoToUrl(pageUrl);
            var result = Activator.CreateInstance(typeof(T), this.Driver, this.Settings) as T;
            return result;
        }
        public T GoToPageAsWarm<T>(string pageUrl, string mdsId) where T : class, IPage
        {
            GoTo($"{pageUrl}?ref={mdsId}");
            var result = Activator.CreateInstance(typeof(T), this.Driver, this.Settings) as T;
            return result;
        }
        public T SetAsWarm<T>(string mdsId) where T : class, IPage
        {
            AssertIsTrue(!string.IsNullOrEmpty(mdsId));
            var url = $"{Driver.Url}?ref={mdsId}";
            this.Driver.Navigate().GoToUrl(url);
            var result = Activator.CreateInstance(typeof(T), this.Driver, this.Settings) as T;
            return result;
        }

        public SeiumbPage AssertUrl(string url)
        {
            var currentPage = new Uri(Driver.Url);
            AssertIsTrue(string.Equals(url, currentPage.AbsolutePath));
            return this;
        }

        public override ControlDefinition GetControlByKey(string controlKey)
        {
            var str = controlKey;
            controlKey = str ?? throw new ArgumentNullException(nameof(controlKey));

            TryGetByFullKey(ControlDefinition.NormalizeKey(controlKey, this.Name), (PageDefinition) null,
                out var controlDefinition);
            if (controlDefinition == null)
            {
                TryGetByFullKey(ControlDefinition.NormalizeKey(controlKey, BasePageName), (PageDefinition)null,
                    out controlDefinition);
            }
            return controlDefinition ?? throw new FrameworkConfigurationException(
                       $"No control found by full key [{ControlDefinition.NormalizeKey(controlKey, this.Name)}]");
        }

        public bool AssertGTMCode(string onClickEvent, string validGTMEvent)
        {

            if (validGTMEvent.Contains('{') && validGTMEvent.Contains('}'))
            {
                validGTMEvent = validGTMEvent.Substring(validGTMEvent.IndexOf("dataLayerPush({") + 15);
                validGTMEvent = validGTMEvent.Substring(0, validGTMEvent.IndexOf('}'));
            }
            string[] gtmEvents = validGTMEvent.Split(',');


            for (int i = 0; i < gtmEvents.Length; i++)
            {
                if (!onClickEvent.Contains(gtmEvents[i]))
                {
                    return false;

                }

            }

            return true;

        }
        #endregion

        #region Private
        private void MergeBaseControls()
        {
            var seiumbPageDefinition = base.Settings.Controls.Pages
                .FirstOrDefault(x => x.Name.Equals(BasePageName, StringComparison.InvariantCultureIgnoreCase));
            var pageDefinition = base.Settings.Controls.Pages
                .FirstOrDefault(x => x.Name.Equals(this.Name, StringComparison.InvariantCultureIgnoreCase));

            if (seiumbPageDefinition == null) return;
            foreach (var section in seiumbPageDefinition.Sections)
            {
                pageDefinition?.Merge(section.Value);
            }
        }
        private bool TryGetByFullKey(string fullKey, PageDefinition page, out ControlDefinition controlDefinition)
        {
            try
            {
                controlDefinition = this.Settings.Controls.GetByFullKey(fullKey, page);
                return true;
            }
            catch
            {
                controlDefinition = null;
                return false;
            }
        }
        #endregion
        #region Support Asserts
        private void CleanFields()
        {
            AssertElementExists(UsernameTextBox).Clear();
            AssertElementExists(PasswordTextBox).Clear();
        }

        private void PerformLoginAction(string username, string password)
        {
            AssertSetTextBoxValue(UsernameTextBox, username);
            AssertSetTextBoxValue(PasswordTextBox, password);
            AssertClick(SignInButton, timeoutSeconds: Timeout);
        }
        #endregion
    }
}
