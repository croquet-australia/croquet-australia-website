using System.IO;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public static class Urls
    {
        public static class Admin
        {
            static Admin()
            {
                AddNews = "~/admin/add-news";
            }

            public static string NewPage(string requestedDirectory)
            {
                return string.Format("~/admin/new-page?directory={0}", requestedDirectory);
            }

            public static string EditPage(string requestedDirectory, FileSystemInfo file)
            {
                return string.Format(@"~/admin/edit?file={0}", Path.Combine(requestedDirectory, file.Name));
            }

            public static string ChangeDirectory(string requestedDirectory, FileSystemInfo directory)
            {
                return string.Format(@"~/admin?directory={0}", Path.Combine(requestedDirectory, directory.Name));
            }

            public static string AddNews { get; private set; }
        }
    }
}