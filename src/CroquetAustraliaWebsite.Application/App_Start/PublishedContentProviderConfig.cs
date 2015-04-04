using System.Web.Hosting;
using Anotar.NLog;
using CroquetAustraliaWebsite.Library.Settings;
using CroquetAustraliaWebsite.Library.Web.Hosting;

namespace CroquetAustraliaWebsite.Application
{
    public class PublishedContentProviderConfig
    {
        private readonly PublishedContentRepositorySettings _repositorySettings;

        public PublishedContentProviderConfig(PublishedContentRepositorySettings repositorySettings)
        {
            _repositorySettings = repositorySettings;
        }

        public void RegisterProviders()
        {
            LogTo.Trace("RegisterProviders()");

            var fileRepository = new FileRepository(_repositorySettings.Directory);
            var repositoryPathProvider = new RepositoryPathProvider(fileRepository);

            HostingEnvironment.RegisterVirtualPathProvider(repositoryPathProvider);
        }
    }
}
