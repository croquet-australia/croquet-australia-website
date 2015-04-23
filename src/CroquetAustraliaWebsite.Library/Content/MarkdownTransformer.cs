using System.Web;
using MarkdownSharp;

namespace CroquetAustraliaWebsite.Library.Content
{
    public class MarkdownTransformer : IMarkdownTransformer
    {
        public IHtmlString MarkdownToHtml(string content)
        {
            var markdown = new Markdown();
            var html = markdown.Transform(content);

            return new HtmlString(html);
        }
    }
}