using System;
using System.IO;
using ASP.NET.TestHelpers;
using BoDi;
using CroquetAustralia.Website.Specifications.Helpers;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace CroquetAustralia.Website.Specifications.Steps
{
    [Binding]
    public class DependencyConfigSteps
    {
        private readonly IObjectContainer _container;

        public DependencyConfigSteps(IObjectContainer container)
        {
            _container = container;
            DependencyResolver.Container = _container;
        }

        [BeforeScenario]
        public void RegisterServices()
        {
            _container.RegisterInstanceAs(new WebApplication(GetWebsiteApplicationProjectFile(), IISExpress.GetExeFile()));
            _container.RegisterTypeAs<ApplicationWebDriver, IWebDriver>();
        }

        private static FileInfo GetWebsiteApplicationProjectFile()
        {
            var solutionDirectory = GetSolutionDirectory();
            const string webApplicationProjectName = "CroquetAustralia.Website";
            var projectFile = string.Format(@"{0}\source\{1}\{1}.csproj", solutionDirectory, webApplicationProjectName);

            return new FileInfo(projectFile);
        }

        private static string GetSolutionDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var solutionDirectory = currentDirectory.Substring(0, currentDirectory.IndexOf(@"\tests\", StringComparison.Ordinal));

            return solutionDirectory;
        }
    }
}
