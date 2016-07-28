using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace CroquetAustralia.Website.Specifications.Helpers
{
    public class ApplicationWebDriver : IWebDriver
    {
        private readonly IWebDriver _instance;

        public ApplicationWebDriver()
        {
            _instance = new PhantomJSDriver();
        }

        public string CurrentWindowHandle => _instance.CurrentWindowHandle;

        public string PageSource => _instance.PageSource;

        public string Title => _instance.Title;

        public string Url
        {
            get { return _instance.Url; }
            set { _instance.Url = value; }
        }

        public ReadOnlyCollection<string> WindowHandles => _instance.WindowHandles;

        public void Close()
        {
            _instance.Close();
        }

        public void Dispose()
        {
            _instance.Dispose();
        }

        public IWebElement FindElement(By by)
        {
            return _instance.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _instance.FindElements(by);
        }

        public IOptions Manage()
        {
            return _instance.Manage();
        }

        public INavigation Navigate()
        {
            return _instance.Navigate();
        }

        public void Quit()
        {
            _instance.Quit();
        }

        public ITargetLocator SwitchTo()
        {
            return _instance.SwitchTo();
        }
    }
}