using System.Web.Hosting;
using System.Web.WebPages;
using Anotar.NLog;
using Casper.Data.Git.Infrastructure;
using CroquetAustralia.Library.Content;
using CroquetAustralia.Library.Settings;
using CroquetAustralia.Library.Web.Hosting;
using CroquetAustralia.Library.Web.WebPages;

namespace CroquetAustralia.Website
{
    public class PublishedContentProviderConfig
    {
        private readonly IMarkdownTransformer _markdownTransformer;
        private readonly PublishedContentRepositorySettings _repositorySettings;
        private readonly IYamlMarkdown _yamlMarkdown;

        public PublishedContentProviderConfig(PublishedContentRepositorySettings repositorySettings, IYamlMarkdown yamlMarkdown, IMarkdownTransformer markdownTransformer)
        {
            _repositorySettings = repositorySettings;
            _yamlMarkdown = yamlMarkdown;
            _markdownTransformer = markdownTransformer;
        }

        public void RegisterProviders()
        {
            LogTo.Trace("RegisterProviders()");

            var fileRepository = new FileRepository(_repositorySettings.Directory);
            var repositoryPathProvider = new RepositoryPathProvider(fileRepository);

            HostingEnvironment.RegisterVirtualPathProvider(repositoryPathProvider);
            WebPageHttpHandler.RegisterExtension("md");
            VirtualPathFactoryManager.RegisterVirtualPathFactory(new MarkdownVirtualPathFactory(_yamlMarkdown, _markdownTransformer));
        }
    }
}