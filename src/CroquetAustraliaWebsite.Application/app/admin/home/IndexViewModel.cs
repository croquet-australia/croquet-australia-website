using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustraliaWebsite.Application.App.Infrastructure;

namespace CroquetAustraliaWebsite.Application.App.admin.home
{
    public class IndexViewModel : AdminViewModel
    {
        private IndexViewModel(string requestedDirectory, IEnumerable<DirectoryInfo> enumerateDirectories, IEnumerable<FileInfo> enumerateFiles)
            : base(GetNavigationItems(requestedDirectory))
        {
            Directory = requestedDirectory;
            Directories = enumerateDirectories.Select(d => new NavigationItem(d.Name, Urls.Admin.ChangeDirectory(requestedDirectory, d)));
            Files = enumerateFiles.Select(f => new NavigationItem(f.Name, Urls.Admin.EditPage(requestedDirectory, f)));

            DirectoryHeading = string.IsNullOrWhiteSpace(requestedDirectory) ? "Home" : "todo";
        }

        public IEnumerable<NavigationItem> Directories { get; private set; }
        public string Directory { get; private set; }
        public string DirectoryHeading { get; private set; }
        public IEnumerable<NavigationItem> Files { get; private set; }

        private static IEnumerable<NavigationItem> GetNavigationItems(string requestedDirectory)
        {
            return new[] {new NavigationItem("New Page", Urls.Admin.NewPage(requestedDirectory))};
        }

        public static Task<IndexViewModel> CreateInstance(string gitDirectory, string requestedDirectory)
        {
            var fullDirectory = new DirectoryInfo(Path.Combine(gitDirectory, requestedDirectory.TrimStart('\\')));

            return Task.FromResult(new IndexViewModel(
                requestedDirectory,
                fullDirectory.EnumerateDirectories().Where(d => IsNotHiddenOrSystem(d.Attributes)),
                fullDirectory.EnumerateFiles().Where(d => IsNotHiddenOrSystem(d.Attributes))));
        }

        private static bool IsNotHiddenOrSystem(FileAttributes attributes)
        {
            return !attributes.HasFlag(FileAttributes.Hidden) && !attributes.HasFlag(FileAttributes.System);
        }
    }
}