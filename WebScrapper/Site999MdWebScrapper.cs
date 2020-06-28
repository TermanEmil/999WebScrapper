using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumSandbox
{
    public class Site999MdWebScrapper
    {
        private const string SiteUrl = "https://999.md/ru/";
        private readonly IWebDriver webDriver;

        public Site999MdWebScrapper(IWebDriver webDriver)
        {
            this.webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
        }

        public IEnumerable<SellableGood> ExtractGoods(string goodsName)
        {
            webDriver.Navigate().GoToUrl(SiteUrl);
            WaitUntilLoaded();

            SearchFor(goodsName);
            return ExtractGoodsFromAllPages();
        }

        private void SearchFor(string something)
        {
            webDriver
                .FindElement(By.Id("js-search-input"))
                .SendKeys(something);

            webDriver
                .FindElement(By.ClassName("header__search__button"))
                .Click();

            WaitUntilLoaded();
        }

        private IEnumerable<SellableGood> ExtractGoodsFromAllPages()
        {
            while (true)
            {
                var allGoodsOnThisPage = webDriver.FindElements(By.ClassName("ads-list-photo-item"));
                foreach (var element in allGoodsOnThisPage)
                    yield return ExtractSellableGood(element);

                if (GotoNextPage() is false)
                    break;
            }
        }

        private static SellableGood ExtractSellableGood(ISearchContext sellableGoodElement)
        {
            var name = sellableGoodElement
                .FindElement(By.ClassName("ads-list-photo-item-title"))
                .FindElement(By.TagName("a"))
                .Text;

            var price = sellableGoodElement
                .TryFindElement(By.ClassName("ads-list-photo-item-price-wrapper"))?
                .Text;

            return new SellableGood(name, price);
        }

        private bool GotoNextPage()
        {
            // Get the current page
            // Get the element with 'current' class from the 'paginator'
            const string currentPageXpath = "//nav[contains(@class, 'paginator')]//li[contains(@class, 'current')]//a";
            var currentPage = webDriver.TryFindElement(By.XPath(currentPageXpath))?.Text;
            if (currentPage is null)
                return false;

            var currentPageNb = int.Parse(currentPage);
            var nextPageNb = currentPageNb + 1;

            // Press on the next page if found
            // From the same paginator, find an element with the nextPageNb
            var nextPageXpath = $"//nav[contains(@class, 'paginator')]//a[text()='{nextPageNb}']";
            var nextPage = webDriver.TryFindElement(By.XPath(nextPageXpath));
            if (nextPage is null)
                return false;

            nextPage.Click();
            WaitUntilLoaded();

            return true;
        }

        /// <summary>
        /// 999.md adds a special 'loading' class when something is loading
        /// </summary>
        private void WaitUntilLoaded()
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
            wait.Until(x => x.TryFindElement(By.ClassName("loading")) is null);
        }
    }
}
