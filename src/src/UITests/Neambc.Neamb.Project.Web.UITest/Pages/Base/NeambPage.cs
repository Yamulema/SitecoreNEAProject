using System;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Controls;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Base
{
    public class NeambPage : AssertingPageBase
    {
        private const string BasePageName = "NeambPage";
        #region ControlKeys
        private const string SignInButton = "NeambPage_HeaderCold_SignInButton";
        private const string WarmSignInButton = "NeambPage_HeaderWarm_SignInButton";
        private const string SignOutButton = "NeambPage_HeaderHot_SignOutButton";
        private const string NotYouButton = "NeambPage_HeaderWarm_NotYouButton";
        private const string Avatar = "NeambPage_Header_Avatar";
        private const string WarmColdCreateButton = "NeambPage_HeaderWarmCold_CreateButton";
        #endregion

        #region Constructors
        public NeambPage(
            string pageName,
            string urlPath,
            IWebDriver driver,
            ISettings settings)
            : base(pageName, urlPath, driver, settings)
        {
            MergeBaseNeambControls();
        }
        public NeambPage(
            IWebDriver driver,
            ISettings settings)
            : base(driver, settings)
        {
            MergeBaseNeambControls();
        }
        public NeambPage(
            string name,
            IWebDriver driver,
            ISettings settings)
            : base(name, driver, settings)
        {
            MergeBaseNeambControls();
        }
        #endregion

        #region Public Methods
        public virtual NeambPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Header", "Footer"
            });
            return this;
        }
        public LoginPage ClickOnSignInLink(LoginState loginState = LoginState.Cold)
        {
            switch (loginState)
            {
                case LoginState.Cold:
                    AssertClick(SignInButton);
                    break;
                default:
                    //AssertClick(Avatar);
                    AssertClick(WarmSignInButton);
                    break;
            }
            return new LoginPage(this.Driver, this.Settings);
        }
        public NeambPage GoToPage(string pageUrl)
        {
            GoTo(pageUrl);
            return new NeambPage("NeambPage", pageUrl, this.Driver, this.Settings);
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
        public T SetParameterUrl<T>(string parameterName,string parameterValue) where T : class, IPage
        {
            AssertIsTrue(!string.IsNullOrEmpty(parameterName) && !string.IsNullOrEmpty(parameterValue));
            string url = Driver.Url;
            if (!url.Contains("?"))
            {
                url = $"{Driver.Url}?{parameterName}={parameterValue}";
            }
            else
            {
                url = $"{Driver.Url}&{parameterName}={parameterValue}";
            }
            
            this.Driver.Navigate().GoToUrl(url);
            var result = Activator.CreateInstance(typeof(T), this.Driver, this.Settings) as T;
            return result;
        }

        public NeambPage AssertIsLoggedIn()
        {
            AssertHasAllControlsForSections(new[] {
                "HeaderHot"
            });
            return this;
        }

        public NeambPage AssertIsWarm()
        {
            AssertHasAllControlsForSections(new[] {
                "HeaderWarm"
            });
            return this;
        }
        public NeambPage AssertIsWarmCold()
        {
            AssertHasAllControlsForSections(new[] {
                "HeaderWarmCold"
            });
            return this;
        }
        public NeambPage AssertIsCold()
        {
            AssertHasAllControlsForSections(new[] {
                "HeaderCold"
            });
            return this;
        }
        public NeambPage AssertIsLoggedOut()
        {
            AssertHasAllControlsForSections(new[] {
                "HeaderCold"
            });
            return this;
        }

        public NeambPage AssertUrl(string url)
        {
            var currentPage = new Uri(Driver.Url);
            AssertIsTrue(currentPage.AbsolutePath.Contains(url));
            return this;
        }
        public NeambPage ClickOnSignOutLink()
        {
            AssertClick(Avatar);
            AssertClick(SignOutButton, timeoutSeconds: 30);
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

        public bool IsLiveEnvironment()
        {
            return Driver.Url.StartsWith("https://www.neamb.com");
        }

        public bool IsQAEnvironment()
        {
            return Driver.Url.StartsWith("https://qa.neamb.com");
        }

        public bool AssertGTMCode(string onClickEvent, string validGTMEvent)
        {
                       
            if(validGTMEvent.Contains('{') && validGTMEvent.Contains('}'))
            { 
                validGTMEvent = validGTMEvent.Substring(validGTMEvent.IndexOf("dataLayerPush({") + 15);
                validGTMEvent = validGTMEvent.Substring(0, validGTMEvent.IndexOf('}'));
            }
            string[] gtmEvents = validGTMEvent.Split(',');

            
            for(int i=0; i<gtmEvents.Length; i++ )
            {
                if(!onClickEvent.Contains(gtmEvents[i]))
                {
                    return false;
                    
                }
                   
            }

            return true;

        }
        #endregion

        #region Private
        private void MergeBaseNeambControls()
        {
            var neambPageDefinition = base.Settings.Controls.Pages
                .FirstOrDefault(x => x.Name.Equals(BasePageName, StringComparison.InvariantCultureIgnoreCase));
            var pageDefinition = base.Settings.Controls.Pages
                .FirstOrDefault(x => x.Name.Equals(this.Name, StringComparison.InvariantCultureIgnoreCase));

            if (neambPageDefinition == null) return;
            foreach (var section in neambPageDefinition.Sections)
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
    }
}
