using System;
using ASP.NET.TestHelpers;
using CroquetAustraliaWebsite.Application.Specifications.Helpers;
using CroquetAustraliaWebsite.Application.Specifications.Pages;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace CroquetAustraliaWebsite.Application.Specifications.Steps
{
    [Binding]
    public class SignInSteps
    {
        private readonly WebApplication _webApplication;
        private readonly IWebDriver _webDriver;
        private readonly GivenData _given;
        private readonly ActualData _actual;

        public SignInSteps(WebApplication webApplication, IWebDriver webDriver, GivenData given, ActualData actual)
        {
            _webApplication = webApplication;
            _webDriver = webDriver;
            _given = given;
            _actual = actual;
        }

        [Given(@"the website is running")]
        public void GivenTheWebsiteIsRunning()
        {
            _webApplication.StartIfNotRunning();
        }

        [Given(@"I am not signed in")]
        public void GivenIAmNotSignedIn()
        {
            _webDriver.Post<SignOut>();
        }

        [Given(@"I am an authorised editor")]
        public void GivenIAmAnAuthorisedEditor()
        {
            _given.EmailAddress = "tim@26tp.com";
        }

        [Given(@"I have a Google account")]
        public void GivenIHaveAGoogleAccount()
        {
            // Nothing to do. Just helps how specification reads.
        }

        [When(@"I visit the sign in page")]
        public void WhenIVisitTheSignInPage()
        {
            _webDriver.Goto<SignIn>();
        }

        [When(@"I complete the Google sign in page")]
        public void WhenICompleteTheGoogleSignInPage()
        {
            throw new NotImplementedException();
        }

        [Then(@"I can see the Google sign in page")]
        public void ThenICanSeeTheGoogleSignInPage()
        {
            throw new NotImplementedException();
        }

        [Then(@"I can see the admin page")]
        public void ThenICanSeeTheAdminPage()
        {
            throw new NotImplementedException();
        }
    }
}