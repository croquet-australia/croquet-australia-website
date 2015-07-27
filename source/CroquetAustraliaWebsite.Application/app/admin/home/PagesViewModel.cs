using System.Collections.Generic;
using System.Linq;
using Casper.Core;
using Casper.Domain.Features.Pages;
using CroquetAustraliaWebsite.Application.App.Infrastructure;

namespace CroquetAustraliaWebsite.Application.App.admin.home
{
    public class PagesViewModel : AdminViewModel
    {
        public PagesViewModel(string requestedDirectory, IEnumerable<Directory> directories, IEnumerable<Page> pages)
            : base(GetNavigationItems(requestedDirectory))
        {
            Directory = requestedDirectory;
            Directories = directories.Select(d => new NavigationItem(d.Name, Urls.Admin.ChangeDirectory(requestedDirectory, d.Name)));
            Pages = pages.Select(f => new NavigationItem(f.Name, Urls.Admin.EditPage(requestedDirectory, f.Name)));

            DirectoryHeading = string.IsNullOrWhiteSpace(requestedDirectory) ? "Home" : requestedDirectory.ToUnixSlashes();
        }

        public IEnumerable<NavigationItem> Directories { get; private set; }
        public string Directory { get; private set; }
        public string DirectoryHeading { get; private set; }
        public IEnumerable<NavigationItem> Pages { get; private set; }

        private static IEnumerable<NavigationItem> GetNavigationItems(string requestedDirectory)
        {
            return new[]
            {
                new NavigationItem("Add Page", Urls.Admin.NewPage(requestedDirectory)),
                new NavigationItem("Add News", Urls.Admin.AddNews)
            };
        }
    }
}