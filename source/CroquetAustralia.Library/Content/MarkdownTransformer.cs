using System.Web;
using MarkdownDeep;

namespace CroquetAustralia.Library.Content
{
    public class MarkdownTransformer : IMarkdownTransformer
    {
        public IHtmlString MarkdownToHtml(string content)
        {
            var markdown = new Markdown
            {
                // ExtraMode is required to support tables.
                ExtraMode = true
            };

            var html = markdown.Transform(content);

            return new HtmlString(html);
        }
    }
}