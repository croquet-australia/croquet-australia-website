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

            public static string Index()
            {
                return Index("");
            }

            public static string Index(string requestedDirectory)
            {
                return string.Format("~/admin?directory={0}", requestedDirectory);
            }

            public static string NewPage(string requestedDirectory)
            {
                return string.Format("~/admin/add-page?directory={0}", requestedDirectory);
            }

            public static string EditPage(string requestedDirectory, string fileName)
            {
                return string.Format(@"~/admin/edit?file={0}", Path.Combine(requestedDirectory, fileName));
            }

            public static string ChangeDirectory(string requestedDirectory, string directoryName)
            {
                return string.Format(@"~/admin?directory={0}", Path.Combine(requestedDirectory, directoryName));
            }

            public static string AddNews { get; private set; }
        }
    }
}