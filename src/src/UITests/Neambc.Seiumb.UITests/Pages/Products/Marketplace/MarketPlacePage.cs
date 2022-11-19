using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Neambc.Seiumb.UITests.Pages.Base;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Seiumb.UITests.Pages.Products.Marketplace
{
    public class MarketPlacePage : SeiumbPage
    {
        private static int Timeout => 2;
        private static Regex _regexToValidateMail = new Regex(
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            RegexOptions.CultureInvariant | RegexOptions.Singleline);

        #region ControlKeys
        private const string FirstCard = "MarketPlacePage_FirstCard";
        private const string FirstCardXPath = "//*[@id='browse-offers']/div[2]/div[3]/div[1]/div[1]";
        private const string MarketPlacePage_CheckBoxMemberOnly = "MarketPlacePage_CheckBoxMemberOnly";
        private const string FavoriteStoresCheckbox = "FavoriteStoresCheckbox";
        private const string SeeMoreButton = "SeeMoreButton";
        private const string SuggestionBox = "SuggestionBox";
        private const string SearchInput = "SearchInput";
        private const string FirstSearchResult = "FirstSearchResult";
        private const string ClearSearch = "ClearSearch";
        private const string SortingDropdown = "SortingDropdown";
        private const string NotEligibleBanner = "NotEligibleBanner";
        private const string CloseNotEligibleModalButton = "CloseNotEligibleModalButton";
        private const string CloseRegistrationModalButton = "CloseRegistrationModalButton";
        private const string CloseLoginModalButton = "CloseLoginModalButton";
        private const string UsernameTextBox = "UsernameTextBox";
        private const string PasswordTextBox = "PasswordTextBox";
        private const string SignInButton = "SignInButton";



        #endregion

        #region Constructor
        public MarketPlacePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName,
            urlPath, driver, settings)
        {
        }

        public MarketPlacePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }
        #endregion

        public new MarketPlacePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[]
            {
                "Cards"
            }, 5);
            return this;
        }

        public MarketPlacePage AssertIsExpectedPage(string urlExpected, string param = "")
        {
            Thread.Sleep(3000);
            var urlOpened = this.Driver.Url;
            if (!string.IsNullOrEmpty(param))
            {
                urlExpected += param;
            }

            AssertIsTrue(urlOpened.Contains(urlExpected), "Url " + urlExpected + " was expected, got " + urlOpened + " instead");
            return this;
        }


        #region Stores Markup Methods
        public MarketPlacePage HasStoreCards()
        {
            Thread.Sleep(3000);
            AssertHasAllControlsForSections(new[]
            {
                "Cards"
            });
            return this;
        }

        public MarketPlacePage HasStoreCategories()
        {
            Thread.Sleep(3000);
            AssertHasAllControlsForSections(new[]
            {
                "Categories"
            });
            return this;
        }

        public MarketPlacePage HasFiltersAndSorting()
        {
            Thread.Sleep(3000);
            AssertHasAllControlsForSections(new[]
            {
                "FiltersAndSorting"
            });
            return this;
        }

        public MarketPlacePage HasFavoriteStoresFilter()
        {
            Thread.Sleep(3000);
            AssertElementExists(FavoriteStoresCheckbox, "There is no Favorite Stores Filter", 10);
            return this;
        }

        public MarketPlacePage HasSeeMoreButton()
        {
            Thread.Sleep(3000);
            AssertElementExists(SeeMoreButton, "There is no See More Button", 5);
            return this;
        }

        public bool HasSuggestionBoxVisible()
        {
            Thread.Sleep(3000);
            AssertElementExists(SuggestionBox, "There is no Suggestion Box", 5);
            return Driver.FindElement(By.CssSelector(".suggestion-box-mkp")).Displayed;
        }

        public MarketPlacePage HasNotEligibleBanner()
        {
            AssertElementExists(NotEligibleBanner, "There is no Not Eligible Banner", 3);
            return this;
        }

        public MarketPlacePage IsModalVisible(string modalId)
        {
            Thread.Sleep(1000);
            Assert.IsTrue(Driver.FindElement(By.CssSelector("#" + modalId)).Displayed, 
                "Modal " + modalId + " is not displayed");
            return this;
        }

        public MarketPlacePage IsModalScreenVisible(string modalId, string modalScreen)
        {
            Thread.Sleep(1000);
            Assert.IsTrue(Driver.FindElement(By.CssSelector("#" + modalId + " ." + modalScreen)).Displayed,
                "Modal " + modalId + " with " + modalScreen + " is not visible");
            return this;
        }

        #endregion

        #region Filters Functionality
        public MarketPlacePage PerformSortingByValue(string value)
        {
            AssertSetComboBoxValueByValue(SortingDropdown, value);
            return this;
        }

        public MarketPlacePage PerformSearchAction(string value)
        {
            AssertSetTextBoxValue(SearchInput, value);
            return this;
        }

        public void CheckLoaders()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            By loaderLocator = By.CssSelector(".loader");
            wait.Until(ExpectedConditions.ElementIsVisible(loaderLocator));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loaderLocator));
        }
        #endregion

        #region User Actions
        public MarketPlacePage PerformLoginAction(string username, string password)
        {
            AssertElementExists(UsernameTextBox).Clear();
            AssertSetTextBoxValue(UsernameTextBox, username);
            AssertSetTextBoxValue(PasswordTextBox, password);
            AssertClick(SignInButton, timeoutSeconds: Timeout);
            return this;
        }

        public MarketPlacePage ClickOnFirstCard()
        {
            Thread.Sleep(3000);
            AssertClick(FirstCard, timeoutSeconds: 5);
            return this;
        }
        public MarketPlacePage ClickSeeMoreButton()
        {
            AssertClick(SeeMoreButton, timeoutSeconds: 5);
            return this;
        }

        public MarketPlacePage ClickFirstSearchResult()
        {
            AssertClick(FirstSearchResult, timeoutSeconds: 5);
            return this;
        }

        public MarketPlacePage ClickClearSearchResult()
        {
            AssertClick(ClearSearch, timeoutSeconds: 5);
            return this;
        }

        public MarketPlacePage ClickCloseModal(string modalId)
        {
            switch (modalId)
            {
                case "loginModal":
                    AssertClick(CloseLoginModalButton, timeoutSeconds: 5);
                    break;
                case "marketpalce-modal":
                    AssertClick(CloseRegistrationModalButton, timeoutSeconds: 5);
                    break;
                case "not-elegible-modal":
                    AssertClick(CloseNotEligibleModalButton, timeoutSeconds: 5);
                    break;
            }
            Thread.Sleep(1000);
            return this;
        }

        #endregion

        #region Helper Methods
        public int GetStoresCountBySelector(string cssSelector)
        {
            Thread.Sleep(3000);
            return Driver.FindElements(By.CssSelector(cssSelector)).Count;
        }

        public List<decimal> GetCashBackValueFromCards()
        {
            Thread.Sleep(3000);
            var cards = Driver.FindElements(By.CssSelector(".cards .card"));

            var cardsInfo = cards.Select(x =>
                x.FindElement(By.CssSelector(".cashback-label"))
                    .GetAttribute("innerHTML")).ToList();

            var cashBackValues = cardsInfo.Select(x => decimal.Parse(Regex.Match(x, @"\d+\.?\d*").Value)).ToList();
            return cashBackValues;
        }

        public string GetStoreGuidFromFirstCard()
        {
            var guid = Driver.FindElement(By.XPath(FirstCardXPath))
                .FindElement(By.CssSelector("[data-store-guid]"))
                .GetAttribute("data-store-guid");
            return guid;
        }

        public string GetStoreLinkFromFirstCard()
        {
            var storeLink = Driver.FindElement(By.XPath(FirstCardXPath))
                .GetAttribute("data-store-link");
            return storeLink;
        }

        public MarketPlacePage IsStoreLinkValidUrl()
        {
            var urlOpened = this.Driver.Url;

            Uri uriResult;
            var validUrl = Uri.TryCreate(urlOpened, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            Assert.IsTrue(validUrl, "The url is not valid " + urlOpened);
            return this;
        }

        public MarketPlacePage IsEmailFilledAndValid(string modalId)
        {
            var email = Driver.FindElement(By.CssSelector("#" + modalId))
                .FindElement(By.XPath("//*[@id='marketpalce-modal']/div[1]/input")).GetAttribute("placeholder");
            Assert.IsFalse(string.IsNullOrEmpty(email), "Email in " + modalId + " is Empty");

            var isValidEmail = _regexToValidateMail.IsMatch(email);
            Assert.IsTrue(isValidEmail, "Email in " + modalId + " is Not Valid");

            return this;
        }
        #endregion
    }
}
