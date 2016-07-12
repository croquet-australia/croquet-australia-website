using System.Web;

namespace CroquetAustralia.Library.Settings
{
    public class ContentSettings : AppSettings
    {
        public ContentSettings(HttpServerUtility server)
            : base("Content")
        {
            BlogDirectoryName = Get("BlogDirectoryName");
            Repository = new GitContentRepositorySettings(server);
            PubishedRepository = new PublishedContentRepositorySettings(server);
        }

        public string BlogDirectoryName { get; set; }

        public GitContentRepositorySettings Repository { get; set; }
        public PublishedContentRepositorySettings PubishedRepository { get; set; }
    }
}