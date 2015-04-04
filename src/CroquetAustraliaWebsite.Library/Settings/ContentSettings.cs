using System.Web;

namespace CroquetAustraliaWebsite.Library.Settings
{
    public class ContentSettings : AppSettings
    {
        public ContentSettings(HttpServerUtility server)
        {
            Repository = new GitContentRepositorySettings(server);
            PubishedRepository = new PublishedContentRepositorySettings(server);
        }

        public GitContentRepositorySettings Repository { get; set; }
        public PublishedContentRepositorySettings PubishedRepository { get; set; }
    }
}
