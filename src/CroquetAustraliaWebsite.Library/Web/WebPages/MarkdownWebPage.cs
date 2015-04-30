using System.IO;
using System.Web.Hosting;
using System.Web.WebPages;
using Casper.Data.Git.Infrastructure;
using Casper.Data.Git.Infrastructure.Metadata;
using CroquetAustraliaWebsite.Library.Content;

namespace CroquetAustraliaWebsite.Library.Web.WebPages
{
    public class MarkdownWebPage : WebPage
    {
        private readonly IMarkdownTransformer _markdownTransformer;
        private readonly IYamlMarkdown _yamlMarkdown;

        public MarkdownWebPage(IYamlMarkdown yamlMarkdown, IMarkdownTransformer markdownTransformer)
        {
            _yamlMarkdown = yamlMarkdown;
            _markdownTransformer = markdownTransformer;
        }

        public override void Execute()
        {
            // LogTo.Trace("Execute() - VirtualPath: {0}", VirtualPath);

            // todo: add caching

            var markdownWithFrontMatter = ReadMarkdownWithFrontMatterFromRequestedFile();

            MarkdownMetadata metadata;
            string markdown;

            _yamlMarkdown.Deserialize(markdownWithFrontMatter, out metadata, out markdown);

            var html = _markdownTransformer.MarkdownToHtml(markdown);

            // todo: use metadata
            WriteLiteral(html);
        }

        private string ReadMarkdownWithFrontMatterFromRequestedFile()
        {
            var file = HostingEnvironment.VirtualPathProvider.GetFile(VirtualPath.TrimStart('~'));
            var stream = file.Open();
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}