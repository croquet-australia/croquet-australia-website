using System;
using System.Web.Hosting;
using System.Web.WebPages;
using Casper.Data.Git.Infrastructure;
using CroquetAustraliaWebsite.Library.Content;

namespace CroquetAustraliaWebsite.Library.Web.WebPages
{
    public class MarkdownVirtualPathFactory : IVirtualPathFactory
    {
        private readonly IMarkdownTransformer _markdownTransformer;
        private readonly IYamlMarkdown _yamlMarkdown;

        public MarkdownVirtualPathFactory(IYamlMarkdown yamlMarkdown, IMarkdownTransformer markdownTransformer)
        {
            _yamlMarkdown = yamlMarkdown;
            _markdownTransformer = markdownTransformer;
        }

        public object CreateInstance(string virtualPath)
        {
            // LogTo.Trace("CreateInstance(virtualPath: {0})", virtualPath);

            return new MarkdownWebPage(_yamlMarkdown, _markdownTransformer);
        }

        public bool Exists(string virtualPath)
        {
            // LogTo.Trace("Exists(virtualPath: {0})", virtualPath);

            return IsMarkdownFile(virtualPath) && HostingEnvironment.VirtualPathProvider.FileExists(virtualPath);
        }

        private static bool IsMarkdownFile(string virtualPath)
        {
            return virtualPath.EndsWith(".md", StringComparison.OrdinalIgnoreCase);
        }
    }
}