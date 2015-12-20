using CroquetAustralia.Website.Specifications.Pages.Infrastructure;
using OpenQA.Selenium;

namespace CroquetAustralia.Website.Specifications.Helpers
{
    public static class WebDriverExtensions
    {
        public static TPage Goto<TPage>(this IWebDriver webDriver) where TPage : IPage, new()
        {
            var page = new TPage();

            webDriver.Navigate().GoToUrl(page.Url);

            return page;
        }

        public static TPage Post<TPage>(this IWebDriver webDriver) where TPage : IPage, new()
        {
            // todo: implement
            return new TPage();
        }
    }
}