using System.Web;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public class NavigationItem
    {
        public NavigationItem(string text, string url)
        {
            Text = text;
            Url = url.TrimStart('~');
        }

        public string Text { get; private set; }
        public string Url { get; private set; }

        public IHtmlString RenderHyperlink()
        {
            return new HtmlString(string.Format("<a href=\"{0}\">{1}</a>", Url, Text));
        }
    }
}
