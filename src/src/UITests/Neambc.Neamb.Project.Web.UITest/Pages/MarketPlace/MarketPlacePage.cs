using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.MarketPlace
{
    public class MarketPlacePage : NeambPage
    {
        public static Regex _regexToValidateMail = new Regex(
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            RegexOptions.CultureInvariant | RegexOptions.Singleline);

        #region ControlKeys
        private const string FirstCard = "MarketPlacePage_FirstCard";
        private const string FirstCardXPath = "//*[@id='marketplace']/section/div/div[4]/div[2]/div[1]/div[1]";
        private const string AllStoresRadio = "AllStoresRadio";
        private const string MarketPlacePage_FirstCardHotDeal = "MarketPlacePage_FirstCardHotDeal";
        private const string MarketPlacePage_DivModalClose = "MarketPlacePage_DivModalClose";
        private const string FavoriteStoresToggle = "FavoriteStoresToggle";
        private const string SeeMoreButton = "SeeMoreButton";
        private const string SuggestionBox = "SuggestionBox";
        private const string SearchInput = "SearchInput";
        private const string FirstSearchResult = "FirstSearchResult";
        private const string ClearSearch = "ClearSearch";
        private const string SortingDropdown = "SortingDropdown";
        private const string NotEligibleBanner = "NotEligibleBanner";
        private const string CloseNotEligibleModalButton = "CloseNotEligibleModalButton";
        private const string CloseRegistrationModalButton = "CloseRegistrationModalButton";
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
                "DivCards"
            });
            return this;
        }

        public MarketPlacePage ClickCardHotState()
        {
            AssertClick(FirstCard, timeoutSeconds: 30);
            return new MarketPlacePage(this.Driver, this.Settings);
        }

        public MarketPlacePage ClickCardHotDealsState()
        {
            AssertClick(MarketPlacePage_FirstCardHotDeal, timeoutSeconds: 30);
            return new MarketPlacePage(this.Driver, this.Settings);
        }

        public MarketPlacePage WaitHotStateHotDeals()
        {
            Thread.Sleep(8000);
            return this;
        }

        public MarketPlacePage AssertHotState(string urlexpected)
        {
            Thread.Sleep(8000);
            var urlOpened = this.Driver.Url;
            AssertIsTrue(urlOpened.Contains(urlexpected));
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

        public LoginPage ClickPrimaryButtonRedirectLogin()
        {
            AssertClick(FirstCard, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public LoginPage ClickLoginButtonRedirect()
        {
            AssertClick(FirstCard, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public LoginPage ClickLoginButtonRedirectHotDeals()
        {
            AssertClick(MarketPlacePage_FirstCardHotDeal, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public MarketPlacePage ClickModalClose()
        {
            AssertClick(MarketPlacePage_DivModalClose, timeoutSeconds: 30);
            return this;
        }

        #region Stores Markup Methods
        public MarketPlacePage HasStoreCards()
        {
            Thread.Sleep(3000);
            AssertHasAllControlsForSections(new[]
            {
                "DivCards"
            });
            return this;
        }

        public MarketPlacePage HasPopularOffersChecked()
        {
            Thread.Sleep(3000);
            AssertHasAllControlsForSections(new[]
            {
                "PopularOffers"
            });

            var email = Driver.FindElement(By.CssSelector("#store-popular-offers-only"));
            var isChecked = email.Selected;
            Assert.IsTrue(isChecked, "Popular Offer is not checked");
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
            AssertElementExists(FavoriteStoresToggle, "There is no Favorite Stores Filter", 5);
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
        public MarketPlacePage ClickOnFirstCard()
        {
            Thread.Sleep(2000);
            AssertClick(FirstCard, timeoutSeconds: 5);
            return this;
        }
        public MarketPlacePage ClickSeeMoreButton()
        {
            Thread.Sleep(3000);
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
                case "eligibilitymodel":
                    AssertClick(CloseNotEligibleModalButton, timeoutSeconds: 5);
                    break;
                case "mktpModal":
                    AssertClick(CloseRegistrationModalButton, timeoutSeconds: 5);
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

        public List<string> GetStoreNamesFromCards()
        {
            Thread.Sleep(3000);
            var cards = Driver.FindElements(By.CssSelector(".cards .card"));

            var cardsStoreNames = cards.Select(x =>
                x.FindElement(By.CssSelector("span.hidden"))
                    .GetAttribute("innerHTML")).ToList();

            return cardsStoreNames;
        }

        public MarketPlacePage IsEmailFilledAndValid(string modalId)
        {
            var email = Driver.FindElement(By.CssSelector("#" + modalId + " #rakuten-email")).GetAttribute("value");
            Assert.IsFalse(string.IsNullOrEmpty(email), "Email in "+ modalId + " is Empty");
            var isValidEmail = _regexToValidateMail.IsMatch(email);
            Assert.IsTrue(isValidEmail, "Email in " + modalId + " is Not Valid");
            return this;
        }

        public LoginPage GetLoginPage()
        {
            return new LoginPage(Driver, Settings);
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
        #endregion

        #region Popular Offers
        public MarketPlacePage ClickAllStoresFilters()
        {
            AssertClick(AllStoresRadio, timeoutSeconds: 5);
            return this;
        }
        #endregion

        #region Favorite stores
        private const string MarketPlacePage_CheckBoxFavoriteStores = "MarketPlacePage_CheckBoxFavoriteStores";

        public MarketPlacePage ClickFavoriteStoresToggle()
        {
            AssertClick(MarketPlacePage_CheckBoxFavoriteStores, timeoutSeconds: 30);
            return this;
        }

        public MarketPlacePage VerifyElementsFavoriteStores()
        {
            Thread.Sleep(3000);
            AssertHasAllControlsForSections(new[]
            {
                "FavoriteStores"
            });
            return this;
        }
        #endregion
    }
}
