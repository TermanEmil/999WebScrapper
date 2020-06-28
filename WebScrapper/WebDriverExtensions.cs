using System;
using OpenQA.Selenium;

namespace SeleniumSandbox
{
    public static class WebDriverExtensions
    {
        public static IWebElement TryFindElement(this ISearchContext webDriver, By by)
        {
            try
            {
                return webDriver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }
    }
}
