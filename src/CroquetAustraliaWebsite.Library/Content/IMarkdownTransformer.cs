using System.Web;

namespace CroquetAustraliaWebsite.Library.Content
{
    public interface IMarkdownTransformer
    {
        IHtmlString MarkdownToHtml(string content);
    }
}