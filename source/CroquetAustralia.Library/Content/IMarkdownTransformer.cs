using System.Web;

namespace CroquetAustralia.Library.Content
{
    public interface IMarkdownTransformer
    {
        IHtmlString MarkdownToHtml(string content);
    }
}