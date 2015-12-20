using System.Web;

namespace CroquetAustralia.Library.Settings
{
    public class GitContentRepositorySettings : AppSettings
    {
        public GitContentRepositorySettings(HttpServerUtility server)
            : base("Content:Repository")
        {
            Url = Get("Url");
            Directory = GetDirectory("FullDirectoryPath", server);
            UserName = Get("UserName");
            Password = Get("Password");
        }

        public string Url { get; set; }
        public string Directory { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}