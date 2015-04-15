using Casper.Domain.Infrastructure;
using MarkdownSharp;

namespace CroquetAustraliaWebsite.Library.Content
{
    public class MarkdownParser : IMarkdownParser
    {
        public string ToHtml(string contents)
        {
            var markdown = new Markdown();
            var html = markdown.Transform(contents);

            return html;
        }
    }
}
