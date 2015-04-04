using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace CroquetAustraliaWebsite.Application.Specifications.Helpers
{
    public class ApplicationWebDriver : IWebDriver
    {
        private readonly IWebDriver _instance;

        public ApplicationWebDriver()
        {
            _instance = new PhantomJSDriver();
        }

        public IWebElement FindElement(By @by)
        {
            return _instance.FindElement(@by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return _instance.FindElements(@by);
        }

        public void Dispose()
        {
            _instance.Dispose();
        }

        public void Close()
        {
            _instance.Close();
        }

        public void Quit()
        {
            _instance.Quit();
        }

        public IOptions Manage()
        {
            return _instance.Manage();
        }

        public INavigation Navigate()
        {
            return _instance.Navigate();
        }

        public ITargetLocator SwitchTo()
        {
            return _instance.SwitchTo();
        }

        public string Url
        {
            get { return _instance.Url; }
            set { _instance.Url = value; }
        }

        public string Title
        {
            get { return _instance.Title; }
        }

        public string PageSource
        {
            get { return _instance.PageSource; }
        }

        public string CurrentWindowHandle
        {
            get { return _instance.CurrentWindowHandle; }
        }

        public ReadOnlyCollection<string> WindowHandles
        {
            get { return _instance.WindowHandles; }
        }
    }
}